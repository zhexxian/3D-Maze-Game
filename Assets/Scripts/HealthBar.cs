using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        string text;
        if (gameObject.name == "Timer")
        {
            float timeSinceStart = Time.time;
            text = string.Format("{0}:{1:00}", (int)timeSinceStart / 60, (int)timeSinceStart % 60);
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
