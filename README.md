# Unity-Kenetic-Art-Platform-Simulation

You are tasked to design and develop a software system that will simulate Kinetic Art, for a mechanical hydraulic system.

Our platform will be of MxN dimensional matrix. This is provided at design time or through the data feed when the application is started. The system is responsible for the control of each node in the matrix for the following properties: Status, Speed, Position in Space, Speed, Color, Time and Iteration at any given time throughout the simulation. Each node will need to transition from its original position to the next position all the way to the end of the simulation.


In the first phase of the project, you will need to study the video simulation and start analyzing the necessary program structure you will need to properly simulate the platform.

That is the internal representation of the MxN matrix and the appropriate data for each node for the platform. Your data structure.
The visual representation of the platform. All visual geometry for the platform.
Data feed that will generate the motion.
NOTE: The platform can be configurable on the dimensions and also the spacing between each node.

# Basic Specifications for the Platform Base

Your platform base units will need to have the following dimensions: 1m x 0.1m x 1m.

To test out the initial setup of the platform, you will randomly translate each node using a randomly generated number between -1 and 1.

Functions and Features of the Platform to be Implemented

Your Platform Manager is supposed to have at the minimum the following features and functions implemented:

# Configuration of the MxN platform
Spacing between each base unit for the platform
Input Keys for Testing Features:
Keyboard T: Used to start/stop the test simulation
Keyboard W: Used to increase the Random Range for the displacement on the Y-Axis.
Keyboard S: Used to decrease the Random Range for the displacement on the Y-Axis. Minimum values cannot go below 0.
Keyboard Q: Used to quit/end the whole application.
Keyboard R: Used to generate random colors in the red spectrum.
Keyboard G: Used to generate random colors in the green spectrum.
Keyboard B: Used to generate random colors in the blue spectrum.
Keyboard H: Used to generate random colors in grayscale.
Keyboard E: Used to generate random RGB colors.
During the test simulation, each one of your platform base units will need to maintain the following data:

Current Position
Next Position
Current Color
Next Color
The simulation will randomly generate the next position and color of a given platform base unit when the Current Position and Next Position are equal to one another.

Key elements to consider for your simulation are the smooth transition from one position to the next. Similar with your color transformation.
