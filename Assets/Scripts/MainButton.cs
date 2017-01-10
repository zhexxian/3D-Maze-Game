using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Scripts;

public class MainButton : MonoBehaviour
{
    enum state{ main_menu,options_menu,play_menu,credit_menu};
    state mState;
    public Sprite musicOn;
    public Sprite musicOff;
    public Button btnSea;
    public Button btnSky;
    private GameObject mainMenu;
    private GameObject playMenu;
    private GameObject optionsMenu;
    private GameObject creditsMenu;

    void Start() {
        if (GlobalVariable.UnlockedLevel == -1) {
            DataControl.Load();
        }
        GlobalVariable.InitializeLevelData();
        mState = state.main_menu;
        mainMenu = GameObject.Find("main_menu");
        playMenu = GameObject.Find("play_menu");
        optionsMenu = GameObject.Find("options_menu");
        creditsMenu = GameObject.Find("credits_menu");
        if (GlobalVariable.UnlockedLevel > 1)
        {
            btnSea.interactable = true;
            if(GlobalVariable.UnlockedLevel == 3)
                btnSky.interactable = true;
            else
                btnSky.interactable = false;
        }
        else {
            btnSky.interactable = false;
            btnSea.interactable = false;
        }

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
        GlobalVariable.CurrentLevel = 0;
        MazeDatabase.GenerateMaze(GlobalVariable.CurrentLevel);
        SceneManager.LoadScene("game-scene");
		GlobalVariable.turnOnTutorialCamera(true);

    }

    public void Options_button_click()
    {
        mState = state.options_menu;
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void Music_button_click() {
        Image btn_music = GameObject.Find("btn_music").GetComponent<Image>();
        GlobalVariable.useMusic = !GlobalVariable.useMusic;
        if (GlobalVariable.useMusic)
        {
            btn_music.sprite = musicOn;
        }
        else {
            btn_music.sprite = musicOff;
        }
    }

    public void Exit_button_click()
    {
        Application.Quit();
    }

    public void Ground_button_click() {
        GlobalVariable.CurrentLevel = 1;
        MazeDatabase.GenerateMaze(GlobalVariable.CurrentLevel);
        SceneManager.LoadScene("intro-scene");
    }

    public void Sea_button_click() {
        GlobalVariable.CurrentLevel = 2;
        MazeDatabase.GenerateMaze(GlobalVariable.CurrentLevel);
        SceneManager.LoadScene("game-scene");
    }

    public void Sky_button_click() {
        GlobalVariable.CurrentLevel = 3;
        MazeDatabase.GenerateMaze(GlobalVariable.CurrentLevel);
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
