using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class PlatformDataNode : MonoBehaviour
{
    ///VAR NAMES AND EVENTS ARE TAKEN FROM SLIDE 6 ###########################################################
    public delegate void UpdatePlatformDataNodeUI(PlatformDataNode data);
    public static event UpdatePlatformDataNodeUI OnUpdatePlatformDataNodeUI;

    public bool Selected;
    public bool Program;

    public Color NextColor;

    public int i;
    public int j;
    public float NextPosition = 0.0f;

    public Text txtSelectedNodeName;
    public Text txtSelectedNodePosition;

    public Slider sliderSelectedProgramNodeHeight;

    Canvas myCanvas;
    private void OnEnable()//Function will be called when attached gameobject is toggled, letting the system know something changed
    {
        UIManager.OnNodeProgramChanged += UIManager_OnNodeProgramChanged;

    }

    private void OnDisable()//Function will be called when attached gameobject is toggled, letting the system know something changed
    {
        UIManager.OnNodeProgramChanged -= UIManager_OnNodeProgramChanged;
    }
    private void UIManagerWithEvents_OnToggleProgram(Toggle t)
    {
        Program = !Program;
    }
    private void UIManager_OnNodeProgramChanged(Slider s)
    {
        if (Program && Selected)
        {
            transform.position = new Vector3(transform.position.x, s.value, transform.position.z);//changes Y axis on slider change
            NextPosition = s.value;//Used for recording Write File
            transform.gameObject.GetComponentInChildren<Text>().text = string.Format("{0:0.00}", NextPosition);//Hover yellow text on slider change
        }
    }
    void Start()
    {
        if (transform.GetComponentInChildren<Canvas>() != false)//if object attatched to has children
        {
            myCanvas = transform.GetComponentInChildren<Canvas>();//set children to myCanvas
            myCanvas.enabled = false;//turn children invisible
        }
        NextPosition = transform.position.y;
        ResetDataNode();//Makes sure everything is at default value
    }

    public void ResetDataNode()//Set items back to default parameters
    {
        if (!Program)
        {
            Selected = false;

            NextColor = Color.white;
            NextPosition = 0.0f;
        }
        else
        {
            Selected = false;
        }
    }

    public void SelectNode()//On Mouseclick of cube gameobject
    {
        Selected = true;//mouse click on game object
        Program = true;//in programming scene and selecting a node
        myCanvas.enabled = true;//initiates yellow text nested in object

        OnUpdatePlatformDataNodeUI(this);

        sliderSelectedProgramNodeHeight.value = transform.position.y;//slider change value adjust game object height
    }

    public void ChangeNodesPosition(PlatformDataNode data)
    {
        Selected = false;
        NextColor = Color.white;
        myCanvas.enabled = true;
        NextPosition = data.NextPosition;
        OnUpdatePlatformDataNodeUI(this);
    }
    void UpdateUI()//updates front end ui
    {
        txtSelectedNodeName.text = transform.name;//Displays what node is selected by showing its designated name Node[M.N]
        txtSelectedNodePosition.text = string.Format("Height: {0:0.00}f", transform.position.y);//Makes height visible and displays slider value
    }


    void FixedUpdate()
    {
        if (Program)//While in programming scene
        {

            if (Selected)//on mouseclick on cube game object
            {
                if (transform.GetComponentInChildren<Canvas>().isActiveAndEnabled)
                {
                    transform.GetComponentInChildren<Canvas>().enabled = true;
                }
                transform.gameObject.GetComponent<Renderer>().material.color = Color.blue;//selected cube turns blue on selection

                UpdateUI();//updates front end ui
            }
            else
            {
                transform.gameObject.GetComponent<Renderer>().material.color = Color.white;
                UpdateUI();
            }
            return;
        }
        else
        {
            if (transform.gameObject.GetComponent<Renderer>() != null)
            {
                transform.gameObject.GetComponent<Renderer>().material.color =
                    Color.Lerp(
                        transform.gameObject.GetComponent<Renderer>().material.color, NextColor, Time.deltaTime);
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, NextPosition, transform.position.z), Time.deltaTime);

                if ((Math.Abs(transform.position.y - NextPosition) < 0.01f))
                {
                    NextPosition = 0;
                    NextColor = Color.white;
                }
                transform.gameObject.GetComponentInChildren<Text>().text = string.Format("{0:0.00}", transform.position.y);
            }
        }
    }
    public override string ToString()//write to file helper method for writing node name and position
    {
        return string.Format("{0},{1},{2}", i, j, NextPosition);
    }

}
