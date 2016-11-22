using UnityEngine;
using System.Collections;
using System;

public class CreateMaze : MonoBehaviour
{
	public Material groundMaterial;
	public Material wallMaterial;

    // Use this for initialization
    void Start()
    {
		//initialize map


        const int mazesize = 10;
		MazeDatabase.GenerateMaze (mazesize);
        //string[,] maze = MazeGenerator.CreateMaze(mazesize, 0, 0);

        const int NORTH = 1;
        const int WEST = 1;
        const int EAST = mazesize - 2;
        const int SOUTH = mazesize - 2;
        const int LOOPBEGIN = 1;
        const int LOOPEND = mazesize - 2;

		GameObject[] plane = new GameObject[7];
		GameObject[][,] cube = new GameObject[7][,];
		BoxCollider[][,] cubeCollider = new BoxCollider[7][,];

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

			for (int y = 0; y < MazeDatabase.GetMaze[a].GetLength(0); y++)
			{
				for (int x = 0; x < MazeDatabase.GetMaze[a].GetLength(1); x++)
				{
					//if (a == 1)
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
    }

    // Update is called once per frame
    void Update()
    {

    }
}
