using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Assets.Scripts;

public class GameOverButton : MonoBehaviour {

	public void PlayAgain_button_click() {
		MazeDatabase.GenerateMaze(GlobalVariable.CurrentLevel);
		SceneManager.LoadScene("game-scene");
	}

	public void MainMenu_button_click() {
		SceneManager.LoadScene("menu-scene");
	}

    public void NextWorld_button_click()
    {
        GlobalVariable.CurrentLevel += 1;
        Debug.Log(GlobalVariable.CurrentLevel);
        if (GlobalVariable.CurrentLevel > 3) GlobalVariable.CurrentLevel = 3;
        MazeDatabase.GenerateMaze(GlobalVariable.CurrentLevel);
        SceneManager.LoadScene("game-scene");
    }

    public void Pause_button_click() {
        GlobalVariable.onPauseGame = true;
    }

    public void Resume_button_click()
    {
        GlobalVariable.onPauseGame = false;
    }

}
