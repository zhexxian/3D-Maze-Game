using UnityEngine;
using System.Collections;
using System;

public class CreateMaze : MonoBehaviour
{
    public Material groundMaterial;
    public Material wallMaterial;
    public Material MaterialFloor;
    public Material MaterialGem;
    public GameObject PrefabGem;
    public GameObject PrefabTeleport;
    public GameObject PrefabWall;

    // Use this for initialization
    void Start()
    {
        //initialize map
        int gemNumber = 0;

        const int mazesize = 10;
        //MazeDatabase.GenerateMaze (mazesize);
        //string[,] maze = MazeGenerator.CreateMaze(mazesize, 0, 0);

        const int NORTH = 1;
        const int WEST = 1;
        const int EAST = mazesize - 2;
        const int SOUTH = mazesize - 2;
        const int LOOPBEGIN = 1;
        const int LOOPEND = mazesize - 2;

        GameObject[] plane = new GameObject[7];
        GameObject[][,] cube = new GameObject[7][,];
        GameObject[][,] ground = new GameObject[7][,];
        BoxCollider[][,] cubeCollider = new BoxCollider[7][,];

        for (int a = 1; a <= 6; a++)
        {
            cube[a] = new GameObject[MazeDatabase.GetMaze[a].GetLength(0), MazeDatabase.GetMaze[a].GetLength(1)];
            ground[a] = new GameObject[MazeDatabase.GetMaze[a].GetLength(0), MazeDatabase.GetMaze[a].GetLength(1)];
            cubeCollider[a] = new BoxCollider[MazeDatabase.GetMaze[a].GetLength(0), MazeDatabase.GetMaze[a].GetLength(1)];
        }

        for (int a = 1; a <= 6; a++)
        {
            plane[a] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            plane[a].transform.position = new Vector3(a * MazeDatabase.GetMaze[a].GetLength(0) + 10, 0, 10);
            //maze[a].GetLength(0) --> horizontal; (1) --> vertical
            plane[a].transform.localScale = new Vector3(MazeDatabase.GetMaze[a].GetLength(0), 0.01f, MazeDatabase.GetMaze[a].GetLength(1));

            Material[] groundMaterialArr = new Material[1];
            groundMaterialArr[0] = groundMaterial;
            plane[a].GetComponent<MeshRenderer>().materials = groundMaterialArr;

            for (int y = 0; y < MazeDatabase.GetMaze[a].GetLength(0); y++)
            {
                for (int x = 0; x < MazeDatabase.GetMaze[a].GetLength(1); x++)
                {
                    //if (a == 1)
                    {
                        if (MazeDatabase.GetMaze[a][y, x] == "#")
                        {
                            cube[a][y, x] = (GameObject)Instantiate(PrefabWall);

                            cube[a][y, x].transform.position = new Vector3(x + a * MazeDatabase.GetMaze[a].GetLength(0), 0.5f, y);
                            cube[a][y, x].transform.localScale = new Vector3(1, 0.5f, 1);
                            cube[a][y, x].transform.SetParent(plane[a].transform, true);

                            //Material[] wallMaterialArr = new Material[1];
                            //wallMaterialArr[0] = wallMaterial;
                            //cube[a][y, x].GetComponent<MeshRenderer>().materials = wallMaterialArr;

                            cubeCollider[a][y, x] = (BoxCollider)cube[a][y, x].AddComponent(typeof(BoxCollider));
                            cubeCollider[a][y, x].center = Vector3.zero;

                        }
                        /*
                        else
                        {
                            ground[a][y, x] = (GameObject)Instantiate(PrefabFloor);
                            ground[a][y, x].transform.position = new Vector3(x + a * MazeDatabase.GetMaze[a].GetLength(0), 0.5f, y);
                            ground[a][y, x].transform.localScale = new Vector3(1, 0.5f, 1);
                            ground[a][y, x].transform.SetParent(plane[a].transform, true);
                        }
                        */
                        if (MazeDatabase.GetMaze[a][y, x] == "G")
                        {
                            GameObject go = (GameObject)Instantiate(PrefabGem);
                            //go.GetComponent<Renderer>().material = MaterialGem;
                            go.transform.localPosition = new Vector3(x + a * MazeDatabase.GetMaze[a].GetLength(0), 0.5f, y);
                            //go.transform.localScale = new Vector3(10, 10, 10);
                            gemNumber++;
                        }
                        if (MazeDatabase.GetMaze[a][y, x] == "F")
                        {
                            GameObject.Find("Finish Point").transform.localPosition = new Vector3(x + a * MazeDatabase.GetMaze[a].GetLength(0), 0.5f, y);
                        }
                        
                    }
                    
                }
            }
        }
        GlobalVariable.MaxGemNumber = gemNumber;
        //create maze  teleport spot
        for (int n = 0; n < MazeDatabase.GetMaze[1].GetLength(0); n++)
        {
            //if maze1.east = maze2.west = MAZEPATH
            if ((MazeDatabase.GetMaze[1][n, MazeDatabase.GetMaze[1].GetLength(0) - 2] == " ") && (MazeDatabase.GetMaze[2][n, 1] == " "))
            {
                GameObject go1 = (GameObject)Instantiate(PrefabTeleport);
                go1.transform.localPosition = new Vector3(2 * MazeDatabase.GetMaze[1].GetLength(0) - 2, 0.5f, n);
                GameObject go2 = (GameObject)Instantiate(PrefabTeleport);
                go2.transform.localPosition = new Vector3(2 * MazeDatabase.GetMaze[2].GetLength(0) + 1, 0.5f, n);
            }

            //if maze2.east = maze3.west = MAZEPATH
            if ((MazeDatabase.GetMaze[2][n, MazeDatabase.GetMaze[2].GetLength(0) - 2] == " ") && (MazeDatabase.GetMaze[3][n, 1] == " "))
            {
                GameObject go1 = (GameObject)Instantiate(PrefabTeleport);
                go1.transform.localPosition = new Vector3(3 * MazeDatabase.GetMaze[1].GetLength(0) - 2, 0.5f, n);
                GameObject go2 = (GameObject)Instantiate(PrefabTeleport);
                go2.transform.localPosition = new Vector3(3 * MazeDatabase.GetMaze[2].GetLength(0) + 1, 0.5f, n);
            }

            //if maze3.east = maze4.west = MAZEPATH
            if ((MazeDatabase.GetMaze[3][n, MazeDatabase.GetMaze[3].GetLength(0) - 2] == " ") && (MazeDatabase.GetMaze[4][n, 1] == " "))
            {
                GameObject go1 = (GameObject)Instantiate(PrefabTeleport);
                go1.transform.localPosition = new Vector3(4 * MazeDatabase.GetMaze[1].GetLength(0) - 2, 0.5f, n);
                GameObject go2 = (GameObject)Instantiate(PrefabTeleport);
                go2.transform.localPosition = new Vector3(4 * MazeDatabase.GetMaze[2].GetLength(0) + 1, 0.5f, n);
            }
            //if maze4.east = maze1.west = MAZEPATH
            if ((MazeDatabase.GetMaze[4][n, MazeDatabase.GetMaze[4].GetLength(0) - 2] == " ") && (MazeDatabase.GetMaze[1][n, 1] == " "))
            {
                GameObject go1 = (GameObject)Instantiate(PrefabTeleport);
                go1.transform.localPosition = new Vector3(5 * MazeDatabase.GetMaze[1].GetLength(0) - 2, 0.5f, n);
                GameObject go2 = (GameObject)Instantiate(PrefabTeleport);
                go2.transform.localPosition = new Vector3(1 * MazeDatabase.GetMaze[2].GetLength(0) + 1, 0.5f, n);
            }
            //if maze1.south = maze5.south = MAZEPATH
            if ((MazeDatabase.GetMaze[1][1, n] == " ") && (MazeDatabase.GetMaze[5][1, n] == " "))
            {
                GameObject go1 = (GameObject)Instantiate(PrefabTeleport);
                go1.transform.localPosition = new Vector3(MazeDatabase.GetMaze[1].GetLength(0) + n, 0.5f, 1);
                GameObject go2 = (GameObject)Instantiate(PrefabTeleport);
                go2.transform.localPosition = new Vector3(5 * MazeDatabase.GetMaze[5].GetLength(0) + n, 0.5f, 1);
            }
            //if maze4.south = reverse maze5.west = MAZEPATH
            if ((MazeDatabase.GetMaze[4][1, n] == " ") && (MazeDatabase.GetMaze[5][MazeDatabase.GetMaze[6].GetLength(0) - 1 - n, 1] == " "))
            {
                GameObject go1 = (GameObject)Instantiate(PrefabTeleport);
                go1.transform.localPosition = new Vector3(4 * MazeDatabase.GetMaze[4].GetLength(0) + n, 0.5f, 1);
                GameObject go2 = (GameObject)Instantiate(PrefabTeleport);
                go2.transform.localPosition = new Vector3(5 * MazeDatabase.GetMaze[5].GetLength(0) + 1, 0.5f, MazeDatabase.GetMaze[6].GetLength(0) - 1 - n);
            }
            //if maze3.south = reverse maze5.north = MAZEPATH
            if ((MazeDatabase.GetMaze[3][1, n] == " ") && (MazeDatabase.GetMaze[5][MazeDatabase.GetMaze[5].GetLength(0) - 2, MazeDatabase.GetMaze[6].GetLength(0) - 1 - n] == " "))
            {
                GameObject go1 = (GameObject)Instantiate(PrefabTeleport);
                go1.transform.localPosition = new Vector3(3 * MazeDatabase.GetMaze[3].GetLength(0) + n, 0.5f, 1);
                GameObject go2 = (GameObject)Instantiate(PrefabTeleport);
                go2.transform.localPosition = new Vector3(6 * MazeDatabase.GetMaze[5].GetLength(0) - 1 - n, 0.5f, MazeDatabase.GetMaze[5].GetLength(0) - 2);
            }
            //if maze2.south = maze5.east = MAZEPATH
            if ((MazeDatabase.GetMaze[2][1, n] == " ") && (MazeDatabase.GetMaze[5][n, MazeDatabase.GetMaze[5].GetLength(1) - 2] == " "))
            {
                GameObject go1 = (GameObject)Instantiate(PrefabTeleport);
                go1.transform.localPosition = new Vector3(2 * MazeDatabase.GetMaze[2].GetLength(0) + n, 0.5f, 1);
                GameObject go2 = (GameObject)Instantiate(PrefabTeleport);
                go2.transform.localPosition = new Vector3(6 * MazeDatabase.GetMaze[5].GetLength(1) - 2, 0.5f, n);
            }
            //if maze1.north = maze6.south = MAZEPATH
            if ((MazeDatabase.GetMaze[1][MazeDatabase.GetMaze[1].GetLength(0) - 2, n] == " ") && (MazeDatabase.GetMaze[6][1, n] == " "))
            {
                GameObject go1 = (GameObject)Instantiate(PrefabTeleport);
                go1.transform.localPosition = new Vector3(MazeDatabase.GetMaze[1].GetLength(0) + n, 0.5f, MazeDatabase.GetMaze[1].GetLength(0) - 2);
                GameObject go2 = (GameObject)Instantiate(PrefabTeleport);
                go2.transform.localPosition = new Vector3(6 * MazeDatabase.GetMaze[6].GetLength(0) + n, 0.5f, 1);
            }
            //if maze4.north = maze6.west = MAZEPATH
            if ((MazeDatabase.GetMaze[4][MazeDatabase.GetMaze[1].GetLength(0) - 2, n] == " ") && (MazeDatabase.GetMaze[6][n, 1] == " "))
            {
                GameObject go1 = (GameObject)Instantiate(PrefabTeleport);
                go1.transform.localPosition = new Vector3(4 * MazeDatabase.GetMaze[4].GetLength(0) + n, 0.5f, MazeDatabase.GetMaze[1].GetLength(0) - 2);
                GameObject go2 = (GameObject)Instantiate(PrefabTeleport);
                go2.transform.localPosition = new Vector3(6 * MazeDatabase.GetMaze[6].GetLength(0) + 1, 0.5f, n);
            }
            //if maze3.north = reverse maze6.north = MAZEPATH
            if ((MazeDatabase.GetMaze[3][MazeDatabase.GetMaze[1].GetLength(0) - 2, n] == " ") && (MazeDatabase.GetMaze[6][MazeDatabase.GetMaze[6].GetLength(0) - 2, MazeDatabase.GetMaze[6].GetLength(1) - 1 - n] == " "))
            {
                GameObject go1 = (GameObject)Instantiate(PrefabTeleport);
                go1.transform.localPosition = new Vector3(3 * MazeDatabase.GetMaze[3].GetLength(0) + n, 0.5f, MazeDatabase.GetMaze[1].GetLength(0) - 2);
                GameObject go2 = (GameObject)Instantiate(PrefabTeleport);
                go2.transform.localPosition = new Vector3(7 * MazeDatabase.GetMaze[6].GetLength(0) - 1 - n, 0.5f, MazeDatabase.GetMaze[6].GetLength(0) - 2);
            }
            //if maze2.north = reverse maze6.east = MAZEPATH
            if ((MazeDatabase.GetMaze[2][MazeDatabase.GetMaze[1].GetLength(0) - 2, n] == " ") && (MazeDatabase.GetMaze[6][MazeDatabase.GetMaze[6].GetLength(0) - 1 - n, MazeDatabase.GetMaze[6].GetLength(1) - 2] == " "))
            {
                GameObject go1 = (GameObject)Instantiate(PrefabTeleport);
                go1.transform.localPosition = new Vector3(2 * MazeDatabase.GetMaze[2].GetLength(0) + n, 0.5f, MazeDatabase.GetMaze[1].GetLength(0) - 2);
                GameObject go2 = (GameObject)Instantiate(PrefabTeleport);
                go2.transform.localPosition = new Vector3(7 * MazeDatabase.GetMaze[6].GetLength(1) - 2, 0.5f, MazeDatabase.GetMaze[6].GetLength(1) - 1 - n);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
