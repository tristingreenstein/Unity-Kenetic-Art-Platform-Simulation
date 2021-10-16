using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeCreation : MonoBehaviour
{
    // Start is called before the first frame update
    public int length = 0;//User Defined Length of # of Cubes 
    public int width = 0;// User Defined Width of # of Cubes
    public float speed = 0.02f;//speed of y axis movement

    private GameObject[,] listOfCubes;//list of all game objects
    public Vector3[,] cubeNextPosition;//goal of listOfCube to move to on y axis
    private bool[,] cubeMoving;//stores bool true or false of whether cube is on its way to cubeNextPostion

    private Color[,] currentColor;
    private Color[,] nextColor;
    private bool[,] movingColor;
    private Color initalColor = Color.white;
    private Color colorChangeRGB = Color.white;//set what color should the random generation head towards, 'r' red ,'g' green ,'b' blue ,'h' grayscale, 'g' random rgb
    private bool firstIteration = true;//for moving method, allowing color to change after first iteration
    private float colorChangeSpeed = 0.01f;

    public float yLowerLimit = -1.0f;//limit for random generator
    public float yUpperLimit = 1.0f;//limit for random generator
    private int counter = 0;//counter for turning of and on goHome()
    private bool endPossible = false;//for update(), after first iteration of start, allows end possible

    public bool start = false;//inilizes movement

    void Start()
    {
        float x = 0;
        float y = 0;
        float z = 0;
        float spacing = 0.025f;
        listOfCubes = new GameObject[length, width]; //ARRAY OF ALL GAME OBJECTS
        cubeNextPosition = new Vector3[length, width];//ARRAY OF PLANNED NEXT POSITION
        cubeMoving = new bool[length, width];//ARRAY LISTING IF CUBE IS ON ITS WAY TO cubeNextPosition

        currentColor = new Color[length, width];
        nextColor = new Color[length, width];
        movingColor = new bool[length, width];


        if (length < 0) { length = 0; }//make sure length and width stay above 0;
        if (width < 0) { width = 0; }
        for (int i = 0; i < length; i++)//loop through array of gameobjects
        {
            x = 0;
            for (int j = 0; j < width; j++)
            {
                var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                listOfCubes[i, j] = cube;///stores cube game objects into array

                listOfCubes[i, j].transform.position = new Vector3(x, y, z); //sets initial positions of cubes
                listOfCubes[i, j].transform.localScale = new Vector3(1.0f, 0.1f, 1.0f);//sets initial size of cubes

                nextPostion(i, j);//initilizing the cubeNextPosition array with random values
                cubeMoving[i, j] = true;//initilizing the cubeMoving array at true to start off

                currentColor[i, j] = initalColor;
                nextColor[i, j] = new Color(1f, 1f, 1f, 1f);
                movingColor[i, j] = false;

                listOfCubes[i, j].gameObject.GetComponent<Renderer>().material.color = currentColor[i, j];//sets default color of all cubes
                x = x + 1 + spacing;
            }
            z = z + 1 + spacing;
        }
        Debug.Log("listOfCubes & nextPosition initilized");
    }

    private void nextPostion(int i, int j)//Randomly change y vector between upper and lower limit and stores it in cubeNextPosition array
    {
        //Debug.Log("[" + i + "," + j + "] old:"+ cubeNextPosition[i, j]);
        cubeNextPosition[i, j] = new Vector3(listOfCubes[i, j].transform.position.x, Random.Range(yLowerLimit, yUpperLimit), listOfCubes[i, j].transform.position.z);
        //Debug.Log("[" + i + "," + j + "] new:" + cubeNextPosition[i, j]);
    }
    private void moving(int i, int j)//movement method. moves the cube up and down
    {
        //Debug.Log("[" + i + "," + j + "] moving " + listOfCubes[i, j].transform.position + " to " + debug + "Goal: " + cubeNextPosition[i, j]);
        if (cubeNextPosition[i, j].y > listOfCubes[i, j].transform.position.y)//if goal is higher than current y
        {
            listOfCubes[i, j].transform.position = new Vector3(listOfCubes[i, j].transform.position.x, listOfCubes[i, j].transform.position.y + speed, listOfCubes[i, j].transform.position.z);
        }
        else//if goal is lower than current y
        {
            listOfCubes[i, j].transform.position = new Vector3(listOfCubes[i, j].transform.position.x, listOfCubes[i, j].transform.position.y - speed, listOfCubes[i, j].transform.position.z);
        }
        //Debug.Log(movingColor[i, j]);
        if(movingColor[i,j] == true)
        {
            firstIteration = false;
            if (Vector4.Distance(listOfCubes[i, j].gameObject.GetComponent<Renderer>().material.color, nextColor[i, j]) < 0.01f)
            {
                movingColor[i, j] = false;
            }
            else
            {
                listOfCubes[i, j].gameObject.GetComponent<Renderer>().material.color = Color.Lerp(listOfCubes[i, j].gameObject.GetComponent<Renderer>().material.color, nextColor[i, j], colorChangeSpeed);
            }

        }
        else if(movingColor[i, j] == false && firstIteration == false)
        {
            changeNextColor(i,j);
        }
        
    }
    private void goHome()//Sends all cubes to initial position
    {
        Debug.Log("Go Home Initiated");
        int counterGoHome = 0;
        while (counterGoHome < length * width)//while # of cubes are not in right position, (# being height * width)
        {
            //Debug.Log(counterGoHome);
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    listOfCubes[i, j].gameObject.GetComponent<Renderer>().material.color = Color.white;
                    nextColor[i, j] = Color.white;
                    cubeNextPosition[i, j].y = 0.0f;//tells all cubes that there next position will be 0.0f in hieght
                    if (Vector3.Distance(listOfCubes[i, j].transform.position, cubeNextPosition[i, j]) > 0.01f )//if cubes are not in correct position
                    {
                        //Debug.Log("[" + i + "," + j + "] cubeMoving True");
                        colorChangeRGB = Color.white;
                        moving(i, j);//call movement method to move cubes to right position
                        
                    }
                    else
                    {
                        counterGoHome += 1;//increase counter if cube is in correct position
                    }
                }
            }
        }
        
    }
    private void changeNextColor(int i, int j)
    {
        Debug.Log("changeNextColor Reached;");

        float rand = (Random.Range(0f, 1f));//creates random value between 0f and 1f
        if(colorChangeRGB == Color.red)
        {
            nextColor[i, j] = new Color(rand, 0f, 0f, 1f);//sets random value of red
            movingColor[i, j] = true;
        }
        else if(colorChangeRGB == Color.green)
        {
            nextColor[i, j] = new Color(0f, rand, 0f, 1f);//sets random value of red
            movingColor[i, j] = true;
        }
        else if(colorChangeRGB == Color.blue)
        {
            nextColor[i, j] = new Color(0f, 0f, rand, 1f);//sets random value of red
            movingColor[i, j] = true;
        }
        else if(colorChangeRGB == Color.gray)
        {
            nextColor[i, j] = new Color(rand, rand, rand, 1f);//sets random value of red
            movingColor[i, j] = true;
        }
        else if(colorChangeRGB == Color.cyan)
        {
            nextColor[i, j] = new Color((Random.Range(0f, 1f)), (Random.Range(0f, 1f)), (Random.Range(0f, 1f)), 1f);//sets random value of red
            movingColor[i, j] = true;
        }
        else if (colorChangeRGB == Color.white)
        {
            nextColor[i, j] = new Color(1f,1f,1f, 1f);//sets random value of red
            movingColor[i, j] = true;
        }
        else
        {
            Debug.Log("You should never see this");
        }

    }
    //private void moveToNextColor(int i, int j)//similar to moving method, if color.rgb is not in right position, increment or decrement it
    //{ 
    //    //Change color
    //    listOfCubes[i, j].gameObject.GetComponent<Renderer>().material.color = Color.Lerp(currentColor[i, j], nextColor[i, j], Mathf.PingPong(Time.time, 0.5f));

    //    currentColor[i, j] = listOfCubes[i, j].gameObject.GetComponent<Renderer>().material.color;
    //}
    // Update is called once per frame
    void Update()
    {
        if (speed < 0)//speed safeguard to prevent going under 0
        {
            speed = 0;

        }
        if (yLowerLimit >= 0)//lower limit for random generation safeguard to prevent it going above 0
        {
            yLowerLimit = 0;
        }
        if (yUpperLimit <= 0)//upper limit for random generation safeguard to prevent it going below 0
        {
            yUpperLimit = 0;
        }

        if (Input.GetKeyDown("t"))//on click t, either start or stop program
        {
            counter += 1;
            start = !start;
            endPossible = true;//after first click of t, it is now possible to end program
            //Debug.Log((counter % 2 == 0 && start == false && endPossible == true));
            Debug.Log("H Pushed");
            colorChangeRGB = Color.gray;
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (movingColor[i, j] == false)
                    {

                        changeNextColor(i, j);
                    }
                }
            }
        }

        if (counter % 2 == 1 && start == true)//on start
        {
            colorChangeSpeed = 0.01f;
            //Debug.Log("we started");
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (cubeMoving[i, j] == false)//if im  already at my goal position
                    {
                        //Debug.Log("[" + i + "," + j + "] cubeMoving False");
                        nextPostion(i, j);//gives a new next position
                        cubeMoving[i, j] = true;
                    }
                    else//im not at my goal position
                    {
                        //Debug.Log((!Mathf.Approximately(listOfCubes[i, j].transform.position.x, cubeNextPosition[i, j].x) && !Mathf.Approximately(listOfCubes[i, j].transform.position.y, cubeNextPosition[i, j].y) && !Mathf.Approximately(listOfCubes[i, j].transform.position.z, cubeNextPosition[i, j].z)));
                        if (Vector3.Distance(listOfCubes[i, j].transform.position, cubeNextPosition[i, j]) > 0.01f)//if the distance between current position and next postion is greater than 0.1f
                        {
                            //Debug.Log("[" + i + "," + j + "] cubeMoving True");
                            moving(i, j);//calls movement code to start moving to nextposition
                        }
                        else
                        {
                            cubeMoving[i, j] = false;//this is here mostly to safeguard on first iteration of program
                            //changeNextColor(i, j);
                        }

                    }
                }
            }
            if (Input.GetKeyUp("r") )//on r click: change next Red Value color needs to go to
            {
                colorChangeRGB = Color.red;
                Debug.Log("R Pushed");
                for (int i = 0; i < length; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        if (movingColor[i, j] == false)
                        {
                            
                            changeNextColor(i, j);
                        }
                    }
                }

            }
            if (Input.GetKeyDown("g"))
            {
                colorChangeRGB = Color.green;
                Debug.Log("G Pushed");
                for (int i = 0; i < length; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        if (movingColor[i, j] == false)
                        {

                            changeNextColor(i, j);
                        }
                    }
                }
            }
            if (Input.GetKeyDown("b"))
            {
                colorChangeRGB = Color.blue;
                Debug.Log("B Pushed");
                for (int i = 0; i < length; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        if (movingColor[i, j] == false)
                        {

                            changeNextColor(i, j);
                        }
                    }
                }
            }
            if (Input.GetKeyDown("h"))
            {
                colorChangeRGB = Color.gray;
                Debug.Log("H Pushed");
                for (int i = 0; i < length; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        if (movingColor[i, j] == false)
                        {

                            changeNextColor(i, j);
                        }
                    }
                }
            }
            if (Input.GetKeyDown("e"))
            {
                colorChangeRGB = Color.cyan;
                Debug.Log("H Pushed");
                for (int i = 0; i < length; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        if (movingColor[i, j] == false)
                        {

                            changeNextColor(i, j);
                        }
                    }
                }
            }
            if (Input.GetKeyUp("w"))
            {
                yUpperLimit += 0.5f;
                yLowerLimit -= 0.5f;

            }
            if (Input.GetKeyUp("s"))
            {
                yUpperLimit -= 0.5f;
                yLowerLimit += 0.5f;
            }
        }
        if (counter % 2 == 0 && start == false && endPossible == true)
        {
            goHome();
            start = false;
        }
    }
}
