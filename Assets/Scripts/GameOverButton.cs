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


}
