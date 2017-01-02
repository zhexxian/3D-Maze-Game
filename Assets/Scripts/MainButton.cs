﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainButton : MonoBehaviour
{
    enum state{ main_menu,options_menu,play_menu,credit_menu};
    state mState;
    private GameObject mainMenu;
    private GameObject playMenu;
    private GameObject optionsMenu;
    private GameObject creditsMenu;

    void Start() {
        mState = state.main_menu;
        mainMenu = GameObject.Find("main_menu");
        playMenu = GameObject.Find("play_menu");
        optionsMenu = GameObject.Find("options_menu");
        creditsMenu = GameObject.Find("credits_menu");
        Debug.Log("Starting main menu");
        playMenu.SetActive(false);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }

    // Use this for initialization
    public void Play_button_click()
    {
        mState = state.play_menu;
        mainMenu.SetActive(false);
        playMenu.SetActive(true);
    }

    public void Tutorial_button_click()
    {
        Ground_button_click();
    }

    public void Options_button_click()
    {
        mState = state.options_menu;
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void Exit_button_click()
    {
        Application.Quit();
    }

    public void Ground_button_click() {
        GlobalVariable.LevelMap = 1;
        MazeDatabase.GenerateMaze(10);
        SceneManager.LoadScene("intro-scene");
    }

    public void Sea_button_click() {
        GlobalVariable.LevelMap = 2;
        MazeDatabase.GenerateMaze(10);
        SceneManager.LoadScene("game-scene");
    }

    public void Sky_button_click() {
        GlobalVariable.LevelMap = 3;
        MazeDatabase.GenerateMaze(10);
        SceneManager.LoadScene("game-scene");
    }

    public void Credit_button_click()
    {
        mState = state.credit_menu;
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void Back_button_click()
    {
        if (mState == state.credit_menu)
        {
            creditsMenu.SetActive(false);
            optionsMenu.SetActive(true);
            mState = state.options_menu;
        }
        else {
            if(mState == state.play_menu)
                playMenu.SetActive(false);
            else
                optionsMenu.SetActive(false);
            mainMenu.SetActive(true);
            mState = state.main_menu;
        }
            
        
    }
}
