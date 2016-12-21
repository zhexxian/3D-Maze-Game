using UnityEngine;
using System;
using System.Collections;
using Assets.Scripts;

public class GlobalVariable
{
    private static Vector3 m_playerPosition;
    private static int _maxGemNumber;
    private static int _currGemNumber;
    private static int _levelMap; // 1 - 3
    private static int _indexMap; // 0 - 5
    private static int _unlockedLevel; // 1 - 3

    public static int UnlockedLevel
    {
        get { return _unlockedLevel; }
        set {
            _unlockedLevel = value;
            DataControl.mGameData.unlockedLevel = _unlockedLevel;
            DataControl.Save();
        }
    }

    public static int IndexMap
    {
        get { return _indexMap; }
        set { _indexMap = value; }
    }

    public static int MaxGemNumber
    {
        get { return _maxGemNumber; }
        set { _maxGemNumber = value; }
    }

    public static int CurrGemNumber
    {
        get { return _currGemNumber; }
        set { _currGemNumber = value; }
    }

    public static int LevelMap
    {
        get { return _levelMap; }
        set { _levelMap = value; }
    }

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
