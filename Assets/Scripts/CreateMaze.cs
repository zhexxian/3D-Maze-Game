using UnityEngine;
using System.Collections;

public class CreateMaze : MonoBehaviour
{
    public float wallHeight;
    public Material groundMaterial;
    public Material wallMaterial;
    public Material MaterialFloor;
    public Material MaterialGem;
    public GameObject PrefabGem1;
    public GameObject PrefabGem2;
    public GameObject PrefabGem3;
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
            plane[a].GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(40, 40);

            for (int y = 0; y < MazeDatabase.GetMaze[a].GetLength(0); y++)
            {
                for (int x = 0; x < MazeDatabase.GetMaze[a].GetLength(1); x++)
                {
                    //if (a == 1)
                    {
                        if (MazeDatabase.GetMaze[a][y, x] == "#")
                        {
                            cube[a][y, x] = (GameObject)Instantiate(PrefabWall);

                            cube[a][y, x].transform.position = new Vector3(x + a * MazeDatabase.GetMaze[a].GetLength(0), wallHeight/2f, y);
                            cube[a][y, x].transform.localScale = new Vector3(1, wallHeight, 1);
                            cube[a][y, x].transform.SetParent(plane[a].transform, true);

                            //Material[] wallMaterialArr = new Material[1];
                            //wallMaterialArr[0] = wallMaterial;
                            //cube[a][y, x].GetComponent<MeshRenderer>().materials = wallMaterialArr;

                            cubeCollider[a][y, x] = (BoxCollider)cube[a][y, x].AddComponent(typeof(BoxCollider));
                            cubeCollider[a][y, x].center = Vector3.zero;

                        }
                        if (MazeDatabase.GetMaze[a][y, x] == "G")
                        {
                            GameObject go;
                            int random = Mathf.FloorToInt(Random.Range(0, 2.99f));
                            if (random==0)
                            {
                                go = (GameObject)Instantiate(PrefabGem1);
                            }
                            else if (random==1)
                            {
                                go = (GameObject)Instantiate(PrefabGem2);
                            }
                            else
                            {
                                go = (GameObject)Instantiate(PrefabGem3);
                            }
                            go.transform.localPosition = new Vector3(x + a * MazeDatabase.GetMaze[a].GetLength(0), 0.5f, y);
                            go.name = "gem_" + gemNumber;
                            gemNumber++;
                        }
                        if (MazeDatabase.GetMaze[a][y, x] == "F")
                        {
                            GameObject.Find("Finish Point").transform.localPosition = new Vector3(x + a * MazeDatabase.GetMaze[a].GetLength(0), 0.5f, y);
                            GlobalVariable.SetFinishNodeIndex(new int[3] { a, x, y });
                            GlobalVariable.SetFinishNodeCoordinate(new int[2] { x + a * MazeDatabase.GetMaze[a].GetLength(0), y });
                        }

                    }

                }
            }
        }
        GlobalVariable.MaxGemNumber = gemNumber;
        //create maze  teleport spot

        for (int a = 1; a <= 6; a++)
        {
            for (int z = 1; z < MazeDatabase.GetMaze[a].GetLength(0)-1; z++)
            {
                for (int x = 1; x < MazeDatabase.GetMaze[a].GetLength(1)-1; x++)
                {
                    int[] teleportPoint = MazeDatabase.GetTeleportPoint(a, z, x);
                    if (teleportPoint != null)
                    {
                        int des_a = teleportPoint[0];
                        int des_y = teleportPoint[1];
                        int des_x = teleportPoint[2];
                        GameObject go1 = (GameObject)Instantiate(PrefabTeleport);
                        go1.transform.localPosition = new Vector3(a*MazeDatabase.GetMaze[a].GetLength(0)+x,0.16f,z);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
