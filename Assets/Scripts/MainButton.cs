﻿using UnityEngine;
using System.Collections;

public class MainButton : MonoBehaviour
{

    // Use this for initialization
    public void Play_button_click()
    {
        MazeDatabase.GenerateMaze(10);
        Application.LoadLevel("game-scene");
    }

    public void Exit_button_click()
    {
        Application.Quit();
    }
}
