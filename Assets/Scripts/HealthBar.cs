using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour {
	private float gameTimeInMinutes = 5;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (GlobalVariable.onPauseGame) return;
        string text;
        if (gameObject.name == "Timer")
        {
            float timeSinceStart = Time.time;
			float timeRemaining = gameTimeInMinutes * 60 - timeSinceStart;
			if (timeRemaining < 0) {
				timeRemaining = 0;
			}
			text = string.Format("{0}:{1:00}", (int)timeRemaining / 60, (int)timeRemaining % 60);
        }
        else if (gameObject.name == "Level")
        {
            text = "Level : " + GlobalVariable.LevelMap;
        }
        else {
            text = GlobalVariable.CurrGemNumber + "/" + GlobalVariable.MaxGemNumber;
        }

        GetComponent<UnityEngine.UI.Text>().text = text;

    }
}
