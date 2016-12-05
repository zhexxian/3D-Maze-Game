using UnityEngine;
using System.Collections;
using System;

public class CreateMazeMap : MonoBehaviour
{

	public Material groundMaterial;
	public Material wallMaterial;
	GameObject mapcube = new GameObject();

    // Use this for initialization
    void Start()
    {
		
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
		mapcube.transform.position = new Vector3(100, 0, 10);
		mapcube.transform.localScale = new Vector3(20, 20, 20);
        for (int a = 1; a <= 6; a++)
        {
			cube[a] = new GameObject[MazeDatabase.GetMaze[a].GetLength(0), MazeDatabase.GetMaze[a].GetLength(1)];
			cubeCollider[a] = new BoxCollider[MazeDatabase.GetMaze[a].GetLength(0), MazeDatabase.GetMaze[a].GetLength(1)];
        }

		for (int a = 1; a <= 6; a++)
		{
			plane[a] = GameObject.CreatePrimitive(PrimitiveType.Cube);
			plane[a].transform.position = new Vector3(a * 30 + 10, 0, 10);
			//maze[a].GetLength(0) --> horizontal; (1) --> vertical
			plane[a].transform.localScale = new Vector3(MazeDatabase.GetMaze[a].GetLength(0), 0.01f, MazeDatabase.GetMaze[a].GetLength(1));

			Material[] groundMaterialArr = new Material[1];
			groundMaterialArr [0] = groundMaterial;
			plane [a].GetComponent<MeshRenderer> ().materials = groundMaterialArr;
			plane [a].transform.SetParent (mapcube.transform, true);
			for (int y = 0; y < MazeDatabase.GetMaze[a].GetLength(0); y++)
			{
				for (int x = 0; x < MazeDatabase.GetMaze[a].GetLength(1); x++)
				{
					{
						if (MazeDatabase.GetMaze[a][y, x] == "#")
						{
							cube[a][y, x] = GameObject.CreatePrimitive(PrimitiveType.Cube);

							cube[a][y, x].transform.position = new Vector3(x + a * 30, 0.5f, y);
							cube[a][y, x].transform.localScale = new Vector3(1, 1, 1);
							cube [a] [y, x].transform.SetParent (plane [a].transform, true);

							Material[] wallMaterialArr = new Material[1];
							wallMaterialArr [0] = wallMaterial;
							cube [a][y,x].GetComponent<MeshRenderer> ().materials = wallMaterialArr;

							cubeCollider[a][y,x] = (BoxCollider)cube[a][y,x].AddComponent(typeof(BoxCollider));
							cubeCollider[a][y, x].center = Vector3.zero;

						}
					}

				}
			}
        }
		//Top
		plane[3].transform.Translate(new Vector3(0, 10, 0));
		//Bottom
		plane[1].transform.Translate(new Vector3(60, -10, 0));
		plane[1].transform.Rotate(new Vector3(180, 0, 0));
		//Right
		plane[2].transform.Translate(new Vector3(30, 0, -10));
		plane[2].transform.Rotate(new Vector3(90, 180, 0));
		//Left
		plane[4].transform.Translate(new Vector3(-30, 0, 10));
		plane[4].transform.Rotate(new Vector3(90, 0, 0));
		//Front
		plane[5].transform.Translate(new Vector3(-70, 0, 0));
		plane[5].transform.Rotate(new Vector3(0, 0, 90));
		//Back
		plane[6].transform.Translate(new Vector3(-80, 0, 0));
		plane[6].transform.Rotate(new Vector3(0, 180, 90));

    }

    // Update is called once per frame
    void Update()
    {
		if(Input.GetKey("w")){
			mapcube.transform.Rotate(new Vector3(0, 0, -2));
		}
		if(Input.GetKey("s")){
			mapcube.transform.Rotate(new Vector3(0, 0, 2));
		}
		if(Input.GetKey("a")){
			mapcube.transform.Rotate(new Vector3(-2, 0, 0));
		}
		if(Input.GetKey("d")){
			mapcube.transform.Rotate(new Vector3(2, 0, 0));
		}
    }
}
