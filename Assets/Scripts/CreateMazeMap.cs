using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class CreateMazeMap : MonoBehaviour
{
    public static string gameObjectCubeName = "mapcube";
    public Material groundMaterial;
    public Material wallMaterial;
	public Material playerMaterial;
	public Material goalMaterial;
    GameObject mapcube = new GameObject();
	int[] playerCoordinates; //a, x, z

    public void unsetGameScene() {
        GameObject[] gameSceneObjects = SceneManager.GetSceneByName("game-scene").GetRootGameObjects();
        foreach (GameObject gameObj in gameSceneObjects)
        {
                gameObj.SetActive(false);
        }
    }
    // Use this for initialization
    void Start()
    {
        unsetGameScene();
        GameObject[] plane = new GameObject[7];
        GameObject[][,] cube = new GameObject[7][,];
        //GameObject mapcube = new GameObject();
        BoxCollider[][,] cubeCollider = new BoxCollider[7][,];

        //map cube orientation
        //Top: 3
        //Bottom: 1
        //Left: 2
        //Right: 4
        //Front: 5
        //Back: 6
        mapcube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        mapcube.name = gameObjectCubeName;
        mapcube.transform.position = new Vector3(0, 0, 0);
        mapcube.transform.localScale = new Vector3(20, 20, 20);
        for (int a = 1; a <= 6; a++)
        {
            cube[a] = new GameObject[MazeDatabase.GetMaze[a].GetLength(0), MazeDatabase.GetMaze[a].GetLength(1)];
            cubeCollider[a] = new BoxCollider[MazeDatabase.GetMaze[a].GetLength(0), MazeDatabase.GetMaze[a].GetLength(1)];
        }

        for (int a = 1; a <= 6; a++)
        {
            plane[a] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //plane[a].transform.position = new Vector3(a * 30 + 10, 0, 10);
            //maze[a].GetLength(0) --> horizontal; (1) --> vertical
            plane[a].transform.localScale = new Vector3(MazeDatabase.GetMaze[a].GetLength(0), 0.01f, MazeDatabase.GetMaze[a].GetLength(1));

            Material[] groundMaterialArr = new Material[1];
            groundMaterialArr[0] = groundMaterial;
            plane[a].GetComponent<MeshRenderer>().materials = groundMaterialArr;
            plane[a].transform.SetParent(mapcube.transform, true);
            for (int y = 0; y < MazeDatabase.GetMaze[a].GetLength(0); y++)
            {
                for (int x = 0; x < MazeDatabase.GetMaze[a].GetLength(1); x++)
                {

					if (MazeDatabase.GetMaze [a] [y, x] == "#") {
						cube [a] [y, x] = GameObject.CreatePrimitive (PrimitiveType.Cube);

						cube [a] [y, x].transform.position = new Vector3 (x - ((float)(MazeDatabase.GetMaze [a].GetLength (1)) / 2.0f) + 0.5f, 0.5f, y - ((float)(MazeDatabase.GetMaze [a].GetLength (0)) / 2.0f) + 0.5f);
						cube [a] [y, x].transform.localScale = new Vector3 (1, 1, 1);
						cube [a] [y, x].transform.SetParent (plane [a].transform, true);

						Material[] wallMaterialArr = new Material[1];
						wallMaterialArr [0] = wallMaterial;
						cube [a] [y, x].GetComponent<MeshRenderer> ().materials = wallMaterialArr;

						cubeCollider [a] [y, x] = (BoxCollider)cube [a] [y, x].AddComponent (typeof(BoxCollider));
						cubeCollider [a] [y, x].center = Vector3.zero;

					} else if (MazeDatabase.GetMaze [a] [y, x] == "F") {
						print ("[F] " + "a: " + a + " y: " + y + " x: " + x);
						//this is the finish point
						cube [a] [y, x] = GameObject.CreatePrimitive (PrimitiveType.Cube);

						cube [a] [y, x].transform.position = new Vector3 (x - ((float)(MazeDatabase.GetMaze [a].GetLength (1)) / 2.0f) + 0.5f, 0.5f, y - ((float)(MazeDatabase.GetMaze [a].GetLength (0)) / 2.0f) + 0.5f);
						cube [a] [y, x].transform.localScale = new Vector3 (1, 1, 1);
						cube [a] [y, x].transform.SetParent (plane [a].transform, true);

						Material[] goalMaterialArr = new Material[1];
						goalMaterialArr [0] = goalMaterial;
						cube [a] [y, x].GetComponent<MeshRenderer> ().materials = goalMaterialArr;

						cubeCollider [a] [y, x] = (BoxCollider)cube [a] [y, x].AddComponent (typeof(BoxCollider));
						cubeCollider [a] [y, x].center = Vector3.zero;
					}

                }
            }
        }
		playerCoordinates = GlobalVariable.GetPlayerCoordinate ();
		int aa = playerCoordinates [0]; 
		int xx = playerCoordinates [1];
		int yy = playerCoordinates [2];
		print ("aa: "+aa+" yy: "+yy+" xx: "+xx);
		cube [aa] [yy, xx] = GameObject.CreatePrimitive (PrimitiveType.Sphere);

		cube [aa] [yy, xx].transform.position = new Vector3 (xx - ((float)(MazeDatabase.GetMaze [aa].GetLength (1)) / 2.0f) + 0.5f, 0.5f, yy - ((float)(MazeDatabase.GetMaze [aa].GetLength (0)) / 2.0f) + 0.5f);
		cube [aa] [yy, xx].transform.localScale = new Vector3 (1, 1, 1);
		cube [aa] [yy, xx].transform.SetParent (plane [aa].transform, true);

		Material[] playerMaterialArr = new Material[1];
		playerMaterialArr [0] = playerMaterial;
		cube [aa] [yy, xx].GetComponent<MeshRenderer> ().materials = playerMaterialArr;

		cubeCollider [aa] [yy, xx] = (BoxCollider)cube [aa] [yy, xx].AddComponent (typeof(BoxCollider));
		cubeCollider [aa] [yy, xx].center = Vector3.zero;


        //Bottom
        plane[1].transform.position = new Vector3(0, -0.5f * 20, 0);
        //Right
        plane[2].transform.position = new Vector3(0.5f * 20, 0, 0);
        //Top
        plane[3].transform.position = new Vector3(0, 0.5f * 20, 0);
        //Left
        plane[4].transform.position = new Vector3(-0.5f * 20, 0, 0);
        //Front
        plane[5].transform.position = new Vector3(0, 0, 0.5f * 20);
        //Back
        plane[6].transform.position = new Vector3(0, 0, -0.5f * 20);


        plane[1].transform.Rotate(new Vector3(0, -180, 180));
        plane[2].transform.Rotate(new Vector3(-180, 0, -90));
        plane[3].transform.Rotate(new Vector3(0, 180, 0));
        plane[4].transform.Rotate(new Vector3(-180, 0, 90));
        plane[5].transform.Rotate(new Vector3(90, -90, -90));
        plane[6].transform.Rotate(new Vector3(-90, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("a"))
        {
            mapcube.transform.Rotate(new Vector3(0, 0, -2));
        }
        if (Input.GetKey("d"))
        {
            mapcube.transform.Rotate(new Vector3(0, 0, 2));
        }
        if (Input.GetKey("s"))
        {
            mapcube.transform.Rotate(new Vector3(-2, 0, 0));
        }
        if (Input.GetKey("w"))
        {
            mapcube.transform.Rotate(new Vector3(2, 0, 0));
        }
        if (Input.GetKey("space"))
        {
            mapcube.transform.rotation = new Quaternion(0, 0, 0, 1);
        }
    }
}
