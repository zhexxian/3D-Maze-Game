using UnityEngine;
using System.Collections;
using System;

public class MazeDatabase
{

    private static string[][,] m_maze = new string[7][,];
    private static int[][,][] m_teleportlink = new int[7][,][];

    public MazeDatabase()
    {
    }

    public static void GenerateMaze(int p_level)
    {
        if (p_level == 0)
        {
            for (int a = 1; a <= 6; a++)
            {
                m_maze[a] = new string[6, 6];
            }
            string maze1 =
                "######" +
                "#TG F#" +
                "#    #" +
                "#    #" +
                "# SS #" +
                "######";
            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 6; x++)
                {
                    m_maze[1][y, x] = maze1[y * 6 + x].ToString();
                }
            }
        }
        else
        {
            string[] leveldata = GlobalVariable.GetLevelData(p_level);
            int mazesize = Convert.ToInt32(leveldata[0]);
            float mazecomplexity = Convert.ToSingle(leveldata[1]);
            int totalgem = Convert.ToInt32(leveldata[2]);

            GenerateMaze(mazesize, mazecomplexity, totalgem);
        }
    }


    public static void GenerateMaze(int p_mazesize, float p_complexity, int p_totalgem)
    {
        System.Random random = new System.Random();
        m_maze[1] = MazeGenerator.CreateMaze(p_mazesize, 0.03f, 0, 3, true, false);
        m_teleportlink[1] = new int[m_maze[1].GetLength(0),m_maze[1].GetLength(1)][];

        int mazeFinishPoint = random.Next(2, 6);

        for (int a = 2; a <= 6; a++)
        {
            m_maze[a] = MazeGenerator.CreateMaze(p_mazesize, 0.03f, 0, 3, false, a == mazeFinishPoint);
            m_teleportlink[a] = new int[m_maze[a].GetLength(0), m_maze[a].GetLength(1)][];
        }

        for (int n = 1; n < MazeDatabase.GetMaze[1].GetLength(0)-1; n++)
        {
            //if maze1.east = maze2.west = MAZEPATH
            if ((MazeDatabase.GetMaze[1][n, MazeDatabase.GetMaze[1].GetLength(0) - 2] == " ") && (MazeDatabase.GetMaze[2][n, 1] == " "))
            {
                MazeDatabase.SetTeleportPoint(1, n, MazeDatabase.GetMaze[1].GetLength(0) - 2, 2, n, 1);
            }

            //if maze2.east = maze3.west = MAZEPATH
            if ((MazeDatabase.GetMaze[2][n, MazeDatabase.GetMaze[2].GetLength(0) - 2] == " ") && (MazeDatabase.GetMaze[3][n, 1] == " "))
            {
                MazeDatabase.SetTeleportPoint(2, n, MazeDatabase.GetMaze[2].GetLength(0) - 2, 3, n, 1);
            }

            //if maze3.east = maze4.west = MAZEPATH
            if ((MazeDatabase.GetMaze[3][n, MazeDatabase.GetMaze[3].GetLength(0) - 2] == " ") && (MazeDatabase.GetMaze[4][n, 1] == " "))
            {
                MazeDatabase.SetTeleportPoint(3, n, MazeDatabase.GetMaze[3].GetLength(0) - 2, 4, n, 1);
            }
            //if maze4.east = maze1.west = MAZEPATH
            if ((MazeDatabase.GetMaze[4][n, MazeDatabase.GetMaze[4].GetLength(0) - 2] == " ") && (MazeDatabase.GetMaze[1][n, 1] == " "))
            {
                MazeDatabase.SetTeleportPoint(4, n, MazeDatabase.GetMaze[4].GetLength(0) - 2, 1, n, 1);
            }
            //if maze1.south = maze5.south = MAZEPATH
            if ((MazeDatabase.GetMaze[1][1, n] == " ") && (MazeDatabase.GetMaze[5][1, n] == " "))
            {
                MazeDatabase.SetTeleportPoint(1, 1, n, 5, 1, n);
            }
            //if maze4.south = reverse maze5.west = MAZEPATH
            if ((MazeDatabase.GetMaze[4][1, n] == " ") && (MazeDatabase.GetMaze[5][MazeDatabase.GetMaze[6].GetLength(0) - 1 - n, 1] == " "))
            {
                MazeDatabase.SetTeleportPoint(4, 1, n, 5, MazeDatabase.GetMaze[6].GetLength(0) - 1 - n, 1);
            }
            //if maze3.south = reverse maze5.north = MAZEPATH
            if ((MazeDatabase.GetMaze[3][1, n] == " ") && (MazeDatabase.GetMaze[5][MazeDatabase.GetMaze[5].GetLength(0) - 2, MazeDatabase.GetMaze[6].GetLength(0) - 1 - n] == " "))
            {
                MazeDatabase.SetTeleportPoint(3, 1, n, 5, MazeDatabase.GetMaze[5].GetLength(0) - 2, MazeDatabase.GetMaze[6].GetLength(0) - 1 - n);
            }
            //if maze2.south = maze5.east = MAZEPATH
            if ((MazeDatabase.GetMaze[2][1, n] == " ") && (MazeDatabase.GetMaze[5][n, MazeDatabase.GetMaze[5].GetLength(1) - 2] == " "))
            {
                MazeDatabase.SetTeleportPoint(2, 1, n, 5, n, MazeDatabase.GetMaze[5].GetLength(1) - 2);
            }
            //if maze1.north = maze6.south = MAZEPATH
            if ((MazeDatabase.GetMaze[1][MazeDatabase.GetMaze[1].GetLength(0) - 2, n] == " ") && (MazeDatabase.GetMaze[6][1, n] == " "))
            {
                MazeDatabase.SetTeleportPoint(1, MazeDatabase.GetMaze[1].GetLength(0) - 2, n, 6, 1, n);
            }
            //if maze4.north = maze6.west = MAZEPATH
            if ((MazeDatabase.GetMaze[4][MazeDatabase.GetMaze[1].GetLength(0) - 2, n] == " ") && (MazeDatabase.GetMaze[6][n, 1] == " "))
            {
                MazeDatabase.SetTeleportPoint(4, MazeDatabase.GetMaze[1].GetLength(0) - 2, n, 6, n, 1);
            }
            //if maze3.north = reverse maze6.north = MAZEPATH
            if ((MazeDatabase.GetMaze[3][MazeDatabase.GetMaze[1].GetLength(0) - 2, n] == " ") && (MazeDatabase.GetMaze[6][MazeDatabase.GetMaze[6].GetLength(0) - 2, MazeDatabase.GetMaze[6].GetLength(1) - 1 - n] == " "))
            {
                MazeDatabase.SetTeleportPoint(3, MazeDatabase.GetMaze[1].GetLength(0) - 2, n, 6, MazeDatabase.GetMaze[6].GetLength(0) - 2, MazeDatabase.GetMaze[6].GetLength(1) - 1 - n);
            }
            //if maze2.north = reverse maze6.east = MAZEPATH
            if ((MazeDatabase.GetMaze[2][MazeDatabase.GetMaze[1].GetLength(0) - 2, n] == " ") && (MazeDatabase.GetMaze[6][MazeDatabase.GetMaze[6].GetLength(0) - 1 - n, MazeDatabase.GetMaze[6].GetLength(1) - 2] == " "))
            {
                MazeDatabase.SetTeleportPoint(2, MazeDatabase.GetMaze[1].GetLength(0) - 2, n, 6, MazeDatabase.GetMaze[6].GetLength(0) - 1 - n, MazeDatabase.GetMaze[6].GetLength(1) - 2);
            }
        }
    }

    public static string[][,] GetMaze
    {
        get
        {
            return m_maze;
        }
    }

    public static void SetTeleportPoint(int ori_a, int ori_y, int ori_x, int des_a, int des_y, int des_x)
    {
        m_teleportlink[ori_a][ori_y, ori_x] = new int[3] { des_a, des_y, des_x };
        m_teleportlink[des_a][des_y, des_x] = new int[3] { ori_a, ori_y, ori_x };
    }

    public static int[] GetTeleportPoint(int ori_a, int ori_y, int ori_x)
    {
        int[] result;
        try
        {
            result = m_teleportlink[ori_a][ori_y, ori_x];
        }
        catch
        {
            result = new int[3] { -1, -1, -1 };
        }
        return result;
    }
}
