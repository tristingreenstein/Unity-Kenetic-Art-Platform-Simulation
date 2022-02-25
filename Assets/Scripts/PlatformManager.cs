using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlatformManager : PlatformGenericSinglton<PlatformManager>
{
    ///VAR NAMES AND EVENTS ARE TAKEN FROM SLIDE 6###########################################################
    public delegate void PlatformManagerChanged(PlatformConfigurationData data);
    public static event PlatformManagerChanged OnPlatformManagerChanged;

    public delegate void PlatformManagerUpdateUI();
    public static event PlatformManagerUpdateUI OnPlatformManagerUpdateUI;

    public enum ColorShade
    {
        GrayScale,
        RedScale,
        GreenScale,
        BlueScale,
        Random
    }
    public PlatformConfigurationData configurationData = new PlatformConfigurationData();
    GameObject currentSelection = null;

    public int oldM;
    public int oldN;
    public GameObject PlatformBasePref;
    public GameObject[,] platformNode;
    public PlatformDataNode[,] platformProgram;
    public bool Program = false;
    public bool Simulate = false;
    float spacing_x = 0.0f;
    float spacing_z = 0.0f;

    //#######################################################################################################
    //METHOD NAMES TAKEN FROM SLIDE 6
    private void OnEnable()
    {
        UIManager.BuildPlatformOnClicked += UIManager_BuildPlatformOnClicked;
        UIManager.OnWriteProgramData += UIManager_OnWriteProgramData;

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void OnDisable()
    {
        UIManager.BuildPlatformOnClicked -= UIManager_BuildPlatformOnClicked;
        UIManager.OnWriteProgramData -= UIManager_OnWriteProgramData;

        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    private void UIManager_BuildPlatformOnClicked(PlatformConfigurationData data)
    {
        configurationData = data;

        BuildPlatform();

        oldM = data.M;
        oldN = data.N;
    }
    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (SceneManager.GetActiveScene().name.Contains("Simulation"))
        {
            Simulate = true;
            Program = false;
            BuildPlatformFromFile();

            if (OnPlatformManagerUpdateUI != null)
            {
                OnPlatformManagerUpdateUI();
            }
        }
        else
        {
            Simulate = false;
            if (platformNode != null)
            {
                if (SceneManager.GetActiveScene().name.Contains("Programming"))
                {
                    Program = true;
                    Simulate = false;
                }
                else
                {
                    Program = false;
                }
                BuildPlatform();

                if (OnPlatformManagerUpdateUI != null)
                    OnPlatformManagerUpdateUI();
            }
        }

    }

    private void UIManager_OnWriteProgramData()
    {

        using (StreamWriter outputFile = new StreamWriter(Path.Combine(Application.dataPath, "WriteLines.txt")))
        {
            outputFile.WriteLine(configurationData.ToString());
            for (int i = 0; i < oldM; i++)
            {
                for (int j = 0; j < oldN; j++)
                {
                    outputFile.WriteLine(platformNode[i, j].GetComponent<PlatformDataNode>().ToString());//calls toString of class PlatformDataNode
                }
            }
        }
    }
    public void DestroyPlatform()//called by BuildPlatform to remove all old objects
    {
        if (platformNode != null)
        {
            for (int i = 0; i < oldM; i++)
            {
                for (int j = 0; j < oldN; j++)
                {
                    Destroy(platformNode[i, j], 0.1f);
                }
            }

            platformNode = null;
        }
        if (platformProgram != null)
        {
            for (int i = 0; i < platformProgram.GetLength(0); i++)
            {
                for (int j = 0; j < platformProgram.GetLength(1); j++)
                {
                    Destroy(platformProgram[i, j], 0.1f);
                }
            }

            platformProgram = null;
        }

    }
    void BuildPlatformFromFile()
    {
        using (System.IO.StreamReader sr = new System.IO.StreamReader(Path.Combine(Application.dataPath, "WriteLines.txt")))
        {
            String line;

            line = sr.ReadLine();
            String[] configData = line.Split(',');
            PlatformConfigurationData pcd = new PlatformConfigurationData();
            pcd.M = Convert.ToInt32(configData[0]);
            pcd.N = Convert.ToInt32(configData[1]);
            pcd.deltaSpace = (float)Convert.ToDouble(configData[2]);
            pcd.carriedHeight = (float)Convert.ToDouble(configData[3]);

            configurationData = pcd;
            BuildPlatform();

            platformProgram = new PlatformDataNode[configurationData.M, configurationData.N];

            String[] data;
            while ((line = sr.ReadLine()) != null)
            {
                data = line.Split(',');
                PlatformDataNode nodeData = new PlatformDataNode();
                nodeData.i = Convert.ToInt32(data[0]);
                nodeData.j = Convert.ToInt32(data[1]);
                nodeData.NextPosition = (float)Convert.ToDouble(data[2]);

                platformProgram[nodeData.i, nodeData.j] = nodeData;
            }
        }
    }
    public void BuildPlatform()
    {
        DestroyPlatform();//make sure no existing objects are left on scene besides UI

        platformNode = new GameObject[configurationData.M, configurationData.N];//array of cubes based on user input

        spacing_x = 0;
        for (int i = 0; i < configurationData.M; i++)
        {
            spacing_z = 0;
            for (int j = 0; j < configurationData.N; j++)
            {
                platformNode[i, j] = Instantiate(PlatformBasePref,
                                               new Vector3(i + spacing_x, 0, j + spacing_z),
                                               Quaternion.identity); ;
                platformNode[i, j].name = string.Format("Node[{0},{1}]", i, j);
                platformNode[i, j].AddComponent<PlatformDataNode>();

                PlatformDataNode pdn = platformNode[i, j].transform.GetComponent<PlatformDataNode>();//carries to different classes
                pdn.Program = false;//Setting attributes to the Node, is it in programming mode
                pdn.Selected = false;
                pdn.i = i;//Setting attributes to the Node, location in array
                pdn.j = j;//Setting attributes to the Node, location in array

                spacing_z += configurationData.deltaSpace;
            }
            spacing_x += configurationData.deltaSpace;
        }
        OnPlatformManagerChanged(configurationData);//Calls event to notify of change
    }

    void Start()
    {
    }
    bool MakeMove = false;
    void FixedUpdate()
    {
        if (Simulate)
        {
            if (!MakeMove)
            {
                MakeMove = true;
                ApplyChangeToArray();
                moveColFoward(platformProgram);
                for (int i = 0; i < configurationData.M; i++)
                {
                    for (int j = 0; j < configurationData.N; j++)
                    {
                        if (platformProgram[i, j].NextPosition > 0)
                        {
                            if (NearlyEquals(platformNode[i, j].GetComponent<PlatformDataNode>().transform.position.y, platformProgram[i, j].NextPosition))
                            {
                                PlatformDataNode program = platformProgram[i, j];
                                platformNode[i, j].GetComponent<PlatformDataNode>().ChangeNodesPosition(program);
                            }
                        }
                        if (NearlyEquals(platformNode[i, j].GetComponent<PlatformDataNode>().transform.position.y, platformProgram[i, j].NextPosition))
                        {
                            ApplyChangeToArray();
                            moveColFoward(platformProgram);
                        }
                    }
                }
                MakeMove = false;
            }
        }
        if (Program && Input.GetMouseButtonUp(0))//while in programming mode and clicking on scene
        {
            if (IsPointerOverUIObject())//end if on ui element
            {
                return;
            }
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {

                if (currentSelection != null)
                {
                    PlatformDataNode pdn = currentSelection.transform.GetComponent<PlatformDataNode>();
                    pdn.ResetDataNode();
                }

                currentSelection = hitInfo.transform.gameObject;
                PlatformDataNode newPdn = currentSelection.transform.GetComponent<PlatformDataNode>();
                newPdn.SelectNode();
            }

        }
    }
    private void ApplyChangeToArray()
    {
        for (int i = 0; i < configurationData.M; i++)
        {
            for (int j = 0; j < configurationData.N; j++)
            {
                platformNode[i, j].GetComponent<PlatformDataNode>().ChangeNodesPosition(platformProgram[i, j]);
            }
        }
    }
    void moveColFoward(PlatformDataNode[,] array)
    {
        for (int row = 0; row < configurationData.M; row++)
        {
            for (int column = 0; column < configurationData.N; column++)
            {
                array[row, column] = array[(row) % configurationData.M, (column + 1) % configurationData.N];
            }
        }
    }

    public static bool NearlyEquals(float? value1, float? value2, float unimportantDifference = 0.01f)
    {
        if (value1 != value2)
        {
            if (value1 == null || value2 == null)
                return false;

            return Math.Abs(value1.Value - value2.Value) < unimportantDifference;
        }

        return true;
    }
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;//return true or false
    }
}
