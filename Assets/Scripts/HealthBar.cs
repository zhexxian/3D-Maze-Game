using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour {
	public GameObject gameOverOverlay;
	private float gameTimeInMinutes = 5;

	void Update () {
        if (GlobalVariable.onPauseGame) return;
        string text;
        if (gameObject.name == "Timer")
        {
            float timeSinceStart = Time.time;
			float timeRemaining = gameTimeInMinutes * 60 - timeSinceStart;
			if (timeRemaining < 0) {
				gameOverOverlay.SetActive (true);
				timeRemaining = 0;
			}
			text = string.Format("{0}:{1:00}", (int)timeRemaining / 60, (int)timeRemaining % 60);
        }
        else if (gameObject.name == "Level")
        {
            text = "Level : " + GlobalVariable.CurrentLevel;
        }
        else {
            text = GlobalVariable.CurrGemNumber + "/" + GlobalVariable.RequiredGemNumber;
        }

        GetComponent<UnityEngine.UI.Text>().text = text;

    }
}
