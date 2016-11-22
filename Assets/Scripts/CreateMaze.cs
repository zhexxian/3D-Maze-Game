using UnityEngine;
using System.Collections;
using System;

public class MazeGenerator
{
    static int mazewidth = 0;
    static int mazeheight = 0;
    static string[,] maze;
    static string MAZEWALL = "#";
    static string MAZEPATH = " ";
    static string MAZESTART = "S";
    static string MAZEFINISH = "F";
    static double FINISHDISTANCE = 0.4;
    static double COMPLEXITY = 0.2;
    static int GAP = 3;
    static int[] bigobjectwidth;
    static int[] bigobjectheight;
    static int totalbigobject;

    static System.Random rand = new System.Random();

    #region maze generator - recursive division
    static void makeMaze()
    {
        makeMaze(0, mazewidth - 1, 0, mazeheight - 1);
    }


    //behind the scences actual mazemaking
    static void makeMaze(int left, int right, int top, int bottom)
    {
        int width = right - left;
        int height = bottom - top;

        //makes sure there is still room to divide, then picks the best
        //direction to divide into
        if (width > 2 && height > 2)
        {

            if (width > height)
                divideVertical(left, right, top, bottom);

            else if (height > width)
                divideHorizontal(left, right, top, bottom);

            else if (height == width)
            {
                int pickOne = rand.Next(0, 1);

                if (pickOne == 0)
                    divideVertical(left, right, top, bottom);
                else
                    divideHorizontal(left, right, top, bottom);
            }
        }
        else if (width > 2 && height <= 2)
        {
            divideVertical(left, right, top, bottom);
        }
        else if (width <= 2 && height > 2)
        {
            divideHorizontal(left, right, top, bottom);
        }
    }


    static void divideVertical(int left, int right, int top, int bottom)
    {
        //find a random point to divide at
        //must be even to draw a wall there
        int divide = left + 2 + rand.Next((right - left - 1) / 2) * 2;

        //draw a line at the halfway point
        for (int i = top; i < bottom; i++)
        {
            maze[i, divide] = MAZEWALL;
        }

        //get a random odd integer between top and bottom and clear it
        int clearSpace = top + rand.Next((bottom - top) / 2) * 2 + 1;

        maze[clearSpace, divide] = MAZEPATH;

        makeMaze(left, divide, top, bottom);
        makeMaze(divide, right, top, bottom);
    }

    static void divideHorizontal(int left, int right, int top, int bottom)
    {
        //find a random point to divide at
        //must be even to draw a wall there
        int divide = top + 2 + rand.Next((bottom - top - 1) / 2) * 2;
        if (divide % 2 == 1)
            divide++;

        //draw a line at the halfway point
        for (int i = left; i < right; i++)
        {
            maze[divide, i] = MAZEWALL;
        }

        //get a random odd integer between left and right and clear it
        int clearSpace = left + rand.Next((right - left) / 2) * 2 + 1;

        maze[divide, clearSpace] = MAZEPATH;

        //recur for both parts of the newly split section
        makeMaze(left, right, top, divide);
        makeMaze(left, right, divide, bottom);
    }
    #endregion

    #region insert big object
    static void InsertBigObject(string marker, int width, int height, int gap)
    {
        int startx = 0;
        int starty = 0;
        bool valid = false;
        while (!valid)
        {
            valid = true;
            startx = rand.Next(1, mazewidth - 1 - width);
            starty = rand.Next(1, mazeheight - 1 - height);

            for (int y = starty - gap; y < starty + height + gap; y++)
            {
                for (int x = startx - gap; x < startx + width + gap; x++)
                {
                    if (!((maze[y, x] == MAZEPATH) || (maze[y, x] == MAZEWALL)))
                    {
                        valid = false;
                    }
                }
            }
        }

        for (int y = starty; y < starty + height; y++)
        {
            for (int x = startx; x < startx + width; x++)
            {
                maze[y, x] = marker;
            }
        }
    }
    #endregion

    public static string[,] CreateMaze(int p_mazesize, float p_complexity, int p_totalbigobject, bool p_hasstartpoint, bool p_hasfinishpoint) //this.exe <mazesize> <complexity> <totalbigobject>
    {
        int mazesize = 0;
        bool debug = false;
        if (debug)
        {

        }
        else
        {
            mazesize = p_mazesize;

        }

        mazewidth = mazesize;
        mazeheight = mazesize;
        mazewidth = mazewidth * 2 + 1;
        mazeheight = mazeheight * 2 + 1;

        //Console.Write("Maze Complexity (Easy=0.3 to Difficult=0) = ");
        COMPLEXITY = p_complexity;
        //totalbigobject = Convert.ToInt32(Console.ReadLine());
        try
        {
            totalbigobject = p_totalbigobject;
        }
        catch
        {
            totalbigobject = 0;
        }

        bigobjectwidth = new int[totalbigobject];
        bigobjectheight = new int[totalbigobject];
        for (int a = 0; a < totalbigobject; a++)
        {
            string inputtemp = Console.ReadLine();
            bigobjectwidth[a] = Convert.ToInt32(inputtemp.Split(' ')[0]);
            bigobjectheight[a] = Convert.ToInt32(inputtemp.Split(' ')[1]);
        }

        maze = new string[mazeheight, mazewidth];

        int homewidth = 9;
        int homeheight = 5;

        #region maze initialization
        //initialize maze
        for (int y = 0; y < mazeheight; y++)
        {
            for (int x = 0; x < mazewidth; x++)
            {
                maze[y, x] = " ";
                if ((y % 2 == 0) && (x % 2 == 0))
                {
                    maze[y, x] = "o";
                }
            }
        }

        //draw border horizontal wall
        for (int x = 0; x < mazewidth; x++)
        {
            maze[0, x] = "#";
            maze[mazeheight - 1, x] = "#";
        }

        //draw border vertical wall
        for (int y = 0; y < mazeheight; y++)
        {
            maze[y, 0] = "#";
            maze[y, mazewidth - 1] = "#";
        }
        #endregion

        makeMaze();

        #region complexity calculations
        //make hole based on complexity
        int nWALL = (mazewidth - 1) * (mazeheight - 1) - (homewidth * homeheight);
        int nHOLE = 0;
        while (nHOLE < nWALL * COMPLEXITY)
        {
            int holeX = rand.Next(2, mazewidth - 3);
            int holeY = rand.Next(2, mazeheight - 3);

            if (maze[holeY, holeX] == MAZEWALL)
            {
                int nNEIGHBOURS = 0;
                if (maze[holeY - 1, holeX] == MAZEPATH)
                {
                    if (maze[holeY + 1, holeX] == MAZEPATH)
                    {
                        nNEIGHBOURS++;
                    }
                    nNEIGHBOURS++;
                    if (maze[holeY, holeX - 1] == MAZEPATH)
                    {
                        continue;
                    }
                    if (maze[holeY, holeX + 1] == MAZEPATH)
                    {
                        continue;
                    }
                }
                else if (maze[holeY, holeX - 1] == MAZEPATH)
                {
                    if (maze[holeY, holeX + 1] == MAZEPATH)
                    {
                        nNEIGHBOURS++;
                    }
                    nNEIGHBOURS++;
                    if (maze[holeY + 1, holeX] == MAZEPATH)
                    {
                        continue;
                    }
                    if (maze[holeY - 1, holeX] == MAZEPATH)
                    {
                        continue;
                    }
                }
                if (nNEIGHBOURS == 2)
                {
                    maze[holeY, holeX] = MAZEPATH;
                    nHOLE++;
                }
            }
        }
        #endregion

        if (p_hasstartpoint)
        {
            #region start and finish position
            //randomize start position
            int homex = rand.Next(3, mazewidth - 4 - homewidth);
            //homex = (mazewidth-homewidth) / 2;
            homex = homex + (homex + 1) % 2;
            int homey = rand.Next(3, mazeheight - 4 - homeheight);
            //homey = (mazeheight -homeheight)/ 2;
            homey = homey + (homey + 1) % 2;

            for (int y = 0; y < homeheight; y++)
            {
                for (int x = 0; x < homewidth; x++)
                {
                    maze[homey + y, homex + x] = MAZESTART;
                }
            }

            int homexc = (2 * homex + homewidth) / 2;
            int homeyc = (2 * homey + homeheight) / 2;
        }
        //set finish line
        bool finishlinefound = false;
        double distanceMaze = (Math.Sqrt(mazewidth * mazewidth + mazeheight * mazeheight)) * FINISHDISTANCE;
        //Console.WriteLine(distanceMaze.ToString());
        while (!finishlinefound)
        {
            int flx = rand.Next(1, mazewidth - 2);
            int fly = rand.Next(1, mazeheight - 2);

            //double distanceF = Math.Sqrt(Math.Pow(flx - homexc, 2) + Math.Pow(fly - homeyc, 2)) + 2;

            //Console.Write((flx - homexc).ToString()+" "+ (fly - homeyc).ToString() + " ");
            //Console.WriteLine(flx.ToString()+" " + fly.ToString()+" "+distanceF.ToString());

            //if (distanceF > distanceMaze)
            //{
                if (maze[fly, flx] == MAZEPATH)
                {
                    maze[fly, flx] = MAZEFINISH;
                    break;
                }
            //}
        }
        #endregion

        #region Big Object Insertion

        for (int a = 0; a < totalbigobject; a++)
        {
            InsertBigObject(a.ToString(), bigobjectwidth[a], bigobjectheight[a], GAP);
        }

        #endregion


        return maze;
        //Console.ReadLine();
        //Console.Clear();
    }
}

public class CreateMaze : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        System.Random random = new System.Random();

        const int mazesize = 10;
        //string[,] maze = MazeGenerator.CreateMaze(mazesize, 0, 0);

        string[][,] maze = new string[7][,];

        maze[1] = MazeGenerator.CreateMaze(mazesize, 0, 0,true, false);

        int mazeFinishPoint = random.Next(2, 6);

        for (int a = 2; a <= 6; a++)
        {
            if (mazeFinishPoint == a)
            {
                maze[a] = MazeGenerator.CreateMaze(mazesize, 0, 0, false, true);
            }
            else
            {
                maze[a] = MazeGenerator.CreateMaze(mazesize, 0, 0, false, false);
            }
        }

        const int NORTH = 1;
        const int WEST = 1;
        const int EAST = mazesize - 2;
        const int SOUTH = mazesize - 2;
        const int LOOPBEGIN = 1;
        const int LOOPEND = mazesize - 2;

        //similaring edge
        /*
        
        CUBE SIMILARITY
         ___
        | N |
        |W5E|
        |_S_|___________
        | N | N | N | N |
        |W1E|W2E|W3E|W4E|
        |_S_|_S_|_S_|_S_|
        | N |
        |W6E|
        |_S_|

        1N = 5S     2N = 5E     3N = 5N     4N = 5W     5N = 3N     6N = 1S
        1E = 2W     2E = 3W     3E = 4W     4E = 1W     5E = 2N     6E = 2S
        1S = 6N     2S = 6E     3S = 6S     4S = 6W     5S = 1N     6S = 3S
        1W = 4E     2S = 1E     3W = 2E     4W = 3E     5W = 4N     6W = 4S

        1N = 5S
        1E = 2W
        1S = 6N
        1W = 4E

        2N = 5E
        2E = 3W
        2S = 6E

        3N = 5N
        3E = 4W
        3S = 6S

        4N = 5W
        4S = 6W

        
        
        #region 1N = 5S
        for (int n = 1; n < mazesize - 2; n++)
        {
            if ((maze[1][NORTH, n] == " ") && (maze[5][SOUTH, n] == " "))
            {
                maze[1][NORTH, n] = "T";
                maze[5][SOUTH, n] = "T";
            }

            if ((maze[1][n, EAST] == " ") && (maze[2][n, WEST] == " "))
            {
                maze[1][n, EAST] = "T";
                maze[2][n, WEST] = "T";
            }

            if ((maze[1][SOUTH, n] == " ") && (maze[6][NORTH, n] == " "))
            {
                maze[1][SOUTH, n] = "T";
                maze[6][NORTH, n] = "T";
            }

            if ((maze[1][n, WEST] == " ") && (maze[4][n, EAST] == " "))
            {
                maze[1][n, WEST] = "T";
                maze[4][n, EAST] = "T";
            }

            if ((maze[2][NORTH, n] == " ") && (maze[5][n, EAST] == " "))
            {
                maze[2][NORTH, n] = "T";
                maze[5][n, EAST] = "T";
            }

            if ((maze[2][n, EAST] == " ") && (maze[3][n, WEST] == " "))
            {
                maze[2][n, EAST] = "T";
                maze[3][n, WEST] = "T";
            }

            if ((maze[2][SOUTH, n] == " ") && (maze[6][n, EAST] == " "))
            {
                maze[2][SOUTH, n] = "T";
                maze[6][n, EAST] = "T";
            }
        }
        #endregion
        */


        GameObject[][,] cube = new GameObject[7][,];
        BoxCollider[][,] cubeCollider = new BoxCollider[7][,];
        
        for (int a = 1; a <= 6; a++)
        {
            cube[a] = new GameObject[maze[a].GetLength(0), maze[a].GetLength(1)];
            cubeCollider[a] = new BoxCollider[maze[a].GetLength(0), maze[a].GetLength(1)];
        }

        for (int a = 1; a <= 6; a++)
        {
            for (int y = 0; y < maze[a].GetLength(0); y++)
            {
                for (int x = 0; x < maze[a].GetLength(1); x++)
                {
                    //if (a == 1)
                    {
                        if (maze[a][y, x] == "#")
                        {
                            cube[a][y, x] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            cube[a][y, x].transform.position = new Vector3(x + a * 30, 0.1f, y);
                            cube[a][y, x].transform.localScale = new Vector3(1, 0.01f, 1);
                            cubeCollider[a][y,x] = (BoxCollider)cube[a][y,x].AddComponent(typeof(BoxCollider));
                            cubeCollider[a][y, x].center = Vector3.zero;
                        }
                    }

                }
            }
        }

        GameObject[] planeGround = new GameObject[7];

        for (int a=1; a<=6; a++)
        {
            planeGround[a] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            planeGround[a].transform.position = new Vector3((float)(a * 30)+(float)(maze[a].GetLength(0)-1)/2.0f, 0, (float)(maze[a].GetLength(1)-1)/2.0f);
            planeGround[a].transform.localScale = new Vector3(maze[a].GetLength(0), 0.1f , maze[a].GetLength(1));

            planeGround[a].GetComponent<MeshRenderer>().materials = GameObject.Find("Sample_Ground").GetComponent<MeshRenderer>().materials;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
