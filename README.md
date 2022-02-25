# Unity-Kenetic-Art-Platform-Simulation

You are tasked to design and develop a software system that will simulate Kinetic Art, for a mechanical hydraulic system.

Our platform will be of MxN dimensional matrix. This is provided at design time or through the data feed when the application is started. The system is responsible for the control of each node in the matrix for the following properties: Status, Speed, Position in Space, Speed, Color, Time and Iteration at any given time throughout the simulation. Each node will need to transition from its original position to the next position all the way to the end of the simulation.

Now that we have the basics in place, we want to enhance the program to make it more feature rich and also more user friendly. At his point, you should have developed your platform manager and have defined the initial user interface. 

You will be taking several ideas and applying them to the final outcome of the project. To visualize the final objective of your project, here is the video where we got the inspiration for the project:

## User Interface Specifications

You will have to create several several scenes and unique user interfaces for Part 2 of the project.

### Main Menu - Start Scene: 

The following are your UI Functions. 
Platform Setup: This is where you will be configuring your platform. Basically what you have done in Project 1 and Project 2 Part A. 

### Platform Programming: 

This is where you will be programming the individual nodes of your platform to a given shape that will be simulated at a later time. You will need to think about how you will be creating a UI to make it easier for the user to program individual platform units for simulation at run time.
Platform Simulation: This is where the application will take an existing Platform Program and start the simulation.

### Exit

### Platform Setup - Setup Scene

The platform setup as indicated, is what you have been doing for your project during your previous project assignments. However, you will probably have to revisit and change some internal code structure as well as the user interface to fit into the new program flow. You UI specification are based on Part A of Project 2, and perhaps you will need to update it to take away some buttons and features and add some new ones. For instance, you will need to think about how you will be navigating from one scene to the next. In other words, you will need to get to the next scene, main menu, and or exit the program.

### Platform Programming - Programming Scene

This is the core of the project. You will need to think about how you will be handling and managing the data that will be representing the individual units for the simulation.
Platform Simulation - Simulation Scene (We will implement this in Part 3)

This is the scene where we will simulate the program defined for the platform.

We will break the new project into four scenes: Main Menu; Platform Setup; Platform Programming, Platform Simulation Scenes. Each scene will have its own unique user interface.


NOTE: You will need to make sure that your mouse when over a UI element does not interact with the 3D world! Code was provided to you in class on 10/07/2018!!! If you missed it, you need to work with your friends to figure it out.

Basic guidelines for your User Interface, make sure you take the time and think through the whole application process and understand how the user will be interacting with your application. As a general rule, you should keep your User Interface consistant throughout the application. Here are some sample sketch guidelines.

NOTE: You can and should modify and or change your existing User Interface as you are developing on the project to make it better. That is the whole idea!
 

## Basic Specifications for Platform Programming

IMPORTANT: MAKE SURE YOU DESIGN YOUR USER INTERFACE FOR 1024x768 Screen Resolution.

The idea is to select a given platform node and set the properties. The properties that we are currently interested in are the following:

NOTE: The requirements may change!

Position in space on the y-axis when activated, this could be expanded to other properties as well for extra credit. For example, rotation or scale.

Color when activated

Rotation in space on all axis when activated
You will need to create a User Interface that will allow the user to program the selected platform node with the configurable data listed above.

When the user has finished programming the platform, they will have the ability to save their platform program into a file. You are responsible to creating a file structure that will structure the data in a way that you can consume it at a later time for simulating the platform.

Once the platform has been programming and the data has been saved, the user has the ability to load the data from the disk and run the simulation based on the configuration.

### Minimum Class Definitions

You will be working on modular design for your project going forward. You will need to have at a minimum the following classes defined in your project:

PlatformDataNode.cs

PlatformManager.cs

UIManager.cs

PlatformConfigurationData.cs

PlatformCameraControl.cs 

PlatformGenericSinglton.cs 

## Details for each class (Outline):

NOTE: This is just an outline to get you started, you might have to implement other variables and or functions.

PlatformConfigurationData is the class that will store the platform configuration information: M, N, deltaSpace and Height.

PlatformDataNode is the class that will store the programmed data for the node + relative functions to be performed during programming of the node: State of node, NextPosition, NextColor are the key variables, you will also need to create other variables for the management of the state and etc... You need to define and implement the following functions: Start(), ResetDataNode(), SelectNode(), UpdateUI(), Update() at a minimum. You will also need to register UI Events in the OnEnable() and OnDisable() functions.

UIManager is the class that drives the main UI interactions within the application. You will need to have at a minimum a function to handle all of the button click events named ButtonClicked(Button b). It will take in as parameter the button it is being called from. Within the function you will implement a switch statement that will handle different button events based on the name of the button. This class also define a set of delegates and events that will be consumed by other classes to handle UI interactions. At a minimum you will have the following delegates/events defined as an example:

BuildPlatformClicked / OnBuildPlatformClicked

NodeProgramChanged / OnNodeProgramChanged

UpdateCameraPosition / OnUpdateCameraPosition

The UIManager class, will also have the main reference to all of your UI elements on the Canvas / Scene.

PlatformManager is the class that will create/update the platform GameObjects and will be used to handle keyboard and mouse entry from the application. Unlike, previous projects, it is not going to be responsible for the motion of the platform at this moment. PlatformDataNode is the class that manages the motion of each base unit now. 

PlatformCameraControl is the class that will control the movement of your camera during the application. We won't have a free floating camera. But, our camera should be always positioned at an appropriate distance from the platform based on the platform dimensions and always look at the center of the platform. Your will have the freedom to rotate the camera to the left or right and be able to zoom in and out.

File Structure

Your platform will simulate based on the programmed data. The file that stores the programmed data, also contain the platform configuration.

Header: Will contain the configuration for the platform, i.e., dimensions, spacing.

Body: Will contain the data that will be simulated. index, height