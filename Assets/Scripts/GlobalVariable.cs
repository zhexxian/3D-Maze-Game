using UnityEngine;
using System;
using System.Collections;
using Assets.Scripts;
using System.Collections.Generic;

public class GlobalVariable
{
    private static bool _onPauseGame = false;
    private static bool _useMusic = true;
	private static bool _turnOnTutorialCamera = false;
    private static Vector3 m_playerPosition;
    private static int _maxGemNumber;
    private static int _requiredGemNumber;
    private static int _currGemNumber;
    private static int _unlockedLevel = -1; // 0 - 3 || 
    private static int m_currentlevel;
    private static int[] _finishNodeCoordinate = { 0, 0 };
    private static string finishText;
    public static GameObject[] mGem;
    public static List<string> nonActiveGem = new List<string>();


    public static string getResetPlayerText() {
        return "You have been caught by the guardian! Some of your gems are lost";
    }
    public static string getFinishPlayerText() {
        finishText = "Congratulations! You are success in escaping from the ";
        if (m_currentlevel == 1) {
            finishText += "ground's world maze!";
        }
        if (m_currentlevel == 2)
        {
            finishText += "sea's world maze!";
        }
        if (m_currentlevel == 3)
        {
            finishText += "sky's world maze! You are free now!";
        }
        return finishText;
    }


    //level
    private static string[][] m_leveldata = new string[4][];

    public static int[] GetFinishNodeCoordinate()
    {
        return _finishNodeCoordinate;
    }

    public static void SetFinishNodeCoordinate(int[] coordinate)
    {
        _finishNodeCoordinate = coordinate;
    }

    public static int UnlockedLevel
    {
        get { return _unlockedLevel; }
        set
        {
            _unlockedLevel = value;
            DataControl.Save();
        }
    }

    public static bool useMusic
    {
        get { return _useMusic; }
        set { _useMusic = value; }
    }

    public static bool onPauseGame
    {
        get { return _onPauseGame; }
        set { _onPauseGame = value; }
    }

    public static int getIndexMap()
    {
        return GetPlayerCoordinate()[0];
    }

    public static int MaxGemNumber
    {
        get { return _maxGemNumber; }
        set { _maxGemNumber = value; }
    }

    public static int RequiredGemNumber
    {
        get { return _requiredGemNumber; }
        set { _requiredGemNumber = value; }
    }

    public static int CurrGemNumber
    {
        get { return _currGemNumber; }
        set { _currGemNumber = value; }
    }

    public static Vector3 PlayerPosition
    {
        get { return m_playerPosition; }
        set { m_playerPosition = value; }
    }

    public static int[] GetPlayerCoordinate()
    {
        float decimalValue = 0.5f;
        int a = (int)Math.Truncate((m_playerPosition.x + decimalValue) / MazeDatabase.GetMaze[1].GetLength(1));
        int x = (int)Math.Truncate((m_playerPosition.x + decimalValue) % MazeDatabase.GetMaze[1].GetLength(1));
        int z = (int)Math.Truncate(m_playerPosition.z + decimalValue);

        int[] result = new int[3] { a, x, z };
        return result;
    }

    public static int[] ConvertPositionToCoordinate(float p_x, float p_z)
    {
        float decimalValue = 0.5f;
        int a = (int)Math.Truncate((p_x + decimalValue) / MazeDatabase.GetMaze[1].GetLength(1));
        int x = (int)Math.Truncate((p_x + decimalValue) % MazeDatabase.GetMaze[1].GetLength(1));
        int z = (int)Math.Truncate(p_z + decimalValue);

        int[] result = new int[3] { a, x, z };
        return result;
    }

    public static int CurrentLevel
    {
        get { return m_currentlevel; }
        set { m_currentlevel = value; }
    }

    public static void InitializeLevelData()
    {
        //                       level   |1        |2          |3
        //                               +---------+-----------+----------
        //array[0] = maze size           |8,       |11,        |15
        //array[1] = maze complexity     |0.1,     |0.07,      |0.04
        //array[2] = total gem per side  |3,       |5,         |8
        //array[3] = time                |300,     |600,       |1200
        //array[4] = monster lookup size |5,       |6,         |8
        //array[5] = area type           |land,    |water,     |sky

        m_leveldata[0] = new string[6];
        m_leveldata[1] = new string[6];
        m_leveldata[2] = new string[6];
        m_leveldata[3] = new string[6];

        m_leveldata[1][0] = "8";
        m_leveldata[2][0] = "11";
        m_leveldata[3][0] = "15";

        m_leveldata[1][1] = "0.1";
        m_leveldata[2][1] = "0.07";
        m_leveldata[3][1] = "0.04";

        m_leveldata[1][2] = "3";
        m_leveldata[2][2] = "5";
        m_leveldata[3][2] = "8";

        m_leveldata[1][3] = "300";
        m_leveldata[2][3] = "600";
        m_leveldata[3][3] = "1200";

        m_leveldata[1][4] = "5";
        m_leveldata[2][4] = "6";
        m_leveldata[3][4] = "8";

        m_leveldata[1][5] = "land";
        m_leveldata[2][5] = "water";
        m_leveldata[3][5] = "sky";

    }

    public static string[] GetLevelData(int p_level)
    {
        return m_leveldata[p_level];
    }

	public static void turnOnTutorialCamera(bool useCamera){
		_turnOnTutorialCamera = useCamera;
	}

	public static bool tutorialCameraIsOn(){
		if (_turnOnTutorialCamera) {
			return true;
		}
		return false;
	}
}
