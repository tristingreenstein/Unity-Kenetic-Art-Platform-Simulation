using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIManager : MonoBehaviour
{
    ///VAR NAMES AND EVENTS ARE TAKEN FROM SLIDE 6###########################################################
    public delegate void BuildPlatformClicked(PlatformConfigurationData pcd);//UI MANAGER bullet point 1
    public static event BuildPlatformClicked BuildPlatformOnClicked;//UI MANAGER bullet point 1

    public delegate void UpdateCameraPosition(PlatformConfigurationData pcd);// Listed on assignment under UI Manager
    public static event UpdateCameraPosition OnUpdateComeraPosition;// Listed on assignment under UI Manager

    public delegate void NodeProgramChanged(Slider s);//UI MANAGER bullet point 2
    public static event NodeProgramChanged OnNodeProgramChanged;//UI MANAGER bullet point 2

    public delegate void ToggleProgram();
    public static event ToggleProgram OnToggleProgram;

    public delegate void WriteProgramData();//Exporting Delegate for writing data
    public static event WriteProgramData OnWriteProgramData;//Exporting Event for writing data


    public Text txtPlatformDimensions;
    public Text txtPlatformDeltaSpacing;
    public Text txtPlatformYAxisRange;

    public InputField inputPlatformMDimension;
    public InputField inputPlatformNDimension;

    public Slider sliderDeltaSpacing;
    public Slider sliderYRange;
    public Dropdown dropDownColorSelection;

    public Text txtSelectedNodeName;
    public Text txtSelectedNodePosition;
    //#######################################################################################################
    //METHOD NAMES TAKEN FROM SLIDE 6
    private void OnEnable() //Handles Events , Listed on Assignment
    {
        PlatformManager.OnPlatformManagerChanged += PlatformManager_OnPlatformManagerChanged;
        PlatformManager.OnPlatformManagerUpdateUI += PlatformManager_OnPlatformManagerUpdateUI;

        PlatformDataNode.OnUpdatePlatformDataNodeUI += PlatformDataNode_OnUpdatePlatformDataNodeUI;
    }

    private void OnDisable()//Handles Events , Listed on Assignment
    {
        PlatformManager.OnPlatformManagerChanged -= PlatformManager_OnPlatformManagerChanged;
        PlatformManager.OnPlatformManagerUpdateUI -= PlatformManager_OnPlatformManagerUpdateUI;

        PlatformDataNode.OnUpdatePlatformDataNodeUI -= PlatformDataNode_OnUpdatePlatformDataNodeUI;
    }


    private void PlatformDataNode_OnUpdatePlatformDataNodeUI(PlatformDataNode data)
    {
        data.sliderSelectedProgramNodeHeight = sliderYRange;

        if (data.sliderSelectedProgramNodeHeight != null)
        {
            data.sliderSelectedProgramNodeHeight.minValue = 0;

            data.sliderSelectedProgramNodeHeight.maxValue = PlatformManager.Instance.configurationData.carriedHeight;//Random Height being set max y axis during setup scene
        }
        data.txtSelectedNodeName = txtSelectedNodeName;
        data.txtSelectedNodePosition = txtSelectedNodePosition;
    }

    private void PlatformManager_OnPlatformManagerUpdateUI()
    {
        if (sliderYRange != null)
        {
            if (!PlatformManager.Instance.Program)
            {
                sliderYRange.value = PlatformManager.Instance.configurationData.carriedHeight; //Random Height being set max y axis during setup scene
            }
            else
            {
                sliderYRange.value = 0.0f;
            }

            txtSelectedNodeName.text = "";
            txtSelectedNodePosition.text = string.Format("Height:");
        }
    }


    private void PlatformManager_OnPlatformManagerChanged(PlatformConfigurationData data)
    {
        if (data != null)
        {
            if (inputPlatformMDimension != null)
            {
                inputPlatformMDimension.text = data.M.ToString();
                inputPlatformNDimension.text = data.N.ToString();

                sliderDeltaSpacing.value = data.deltaSpace;

                sliderYRange.value = data.carriedHeight;

            }
            txtPlatformDeltaSpacing.text = string.Format("{0:0.00}f", data.deltaSpace);

            txtPlatformDimensions.text = string.Format("{0}x{1}", data.M, data.N);

            OnUpdateComeraPosition(data);
        }
    }
    public void ButtonClicked(Button data)// Listed on assignment under UI Manager, says to implement switch statements
    {
        switch (data.name)
        {

            case "BuildPlatformButton":
                {
                    if (BuildPlatformOnClicked != null)
                    {
                        PlatformConfigurationData pcd = new PlatformConfigurationData();
                        pcd.M = Convert.ToInt32(inputPlatformMDimension.text);
                        pcd.N = Convert.ToInt32(inputPlatformNDimension.text);
                        pcd.deltaSpace = sliderDeltaSpacing.value;
                        pcd.carriedHeight = sliderYRange.value;

                        BuildPlatformOnClicked(pcd);
                    }

                    break;
                }
            case "ProgramPlatformButton":
                {
                    if (OnWriteProgramData != null)
                        OnWriteProgramData();

                    break;
                }
            case "MainMenuButton":
                {
                    SceneManager.LoadScene("Start");
                    break;
                }
            case "ConfigButton":
                {
                    SceneManager.LoadScene("Setup");
                    break;
                }
            case "ProgramButton":
                {
                    if (OnToggleProgram != null)
                    {
                        OnToggleProgram();
                    }
                    SceneManager.LoadScene("Programming");
                    break;
                }
            case "SimulationButton":
                {
                    SceneManager.LoadScene("Simulation");
                    break;
                }
            case "ExitButton":
                {
                    Application.Quit();
                    break;
                }
        }
    }



    public void SliderValueChanged(Slider data)
    {
        switch (data.name)
        {
            case "DeltaSpacingSlider":
                {
                    txtPlatformDeltaSpacing.text = string.Format("{0:0.00}f", data.value);
                    break;
                }
            case "YAxisSlider":
                {
                    txtPlatformYAxisRange.text = string.Format("{0:0.00}f", data.value);

                    break;
                }
            case "HeightSlider":
                {
                    if (OnNodeProgramChanged != null)
                        OnNodeProgramChanged(data);

                    break;
                }
        }
    }
}
