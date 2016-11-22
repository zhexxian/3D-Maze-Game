using UnityEngine;
using System.Collections;
using System;

public class MazeDatabase{

	private static string[][,] m_maze = new string[7][,];	

	public MazeDatabase()
	{
	}

	public static void GenerateMaze(int maze_size)
	{
		System.Random random = new System.Random ();
		m_maze[1] = MazeGenerator.CreateMaze(maze_size, 0, 0,true, false);

		int mazeFinishPoint = random.Next(2, 6);

		for (int a = 2; a <= 6; a++)
		{
			if (mazeFinishPoint == a)
			{
				m_maze[a] = MazeGenerator.CreateMaze(maze_size, 0, 0, false, true);
			}
			else
			{
				m_maze[a] = MazeGenerator.CreateMaze(maze_size, 0, 0, false, false);
			}
		}
	}

	public static string[][,] GetMaze
	{
		get {
			return m_maze;
		}
	}
}
