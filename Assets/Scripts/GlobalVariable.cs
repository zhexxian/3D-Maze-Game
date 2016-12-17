using UnityEngine;
using System;
using System.Collections;

public class GlobalVariable
{

    private static Vector3 m_playerPosition;
    
    public static Vector3 PlayerPosition
    {
        get { return m_playerPosition; }
        set { m_playerPosition = value; }
    }

    public static int[] GetPlayerCoordinate()
    {
        float decimalValue = (float)((float)(MazeDatabase.GetMaze[1].GetLength(1))/2f)-(float)(Math.Truncate((float)(MazeDatabase.GetMaze[1].GetLength(1)) / 2f));
        Debug.Log(decimalValue);
        int a = (int)Math.Truncate((m_playerPosition.x+decimalValue) / MazeDatabase.GetMaze[1].GetLength(1));
        int x = (int)Math.Truncate((m_playerPosition.x+decimalValue) % MazeDatabase.GetMaze[1].GetLength(1));
        int z = (int)Math.Truncate(m_playerPosition.z+decimalValue);

        int[] result = new int[3] { a, x, z };
        return result;
    }
}
