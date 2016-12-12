using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowTimeSinceStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float timeSinceStart = Time.time;
		string minSec = string.Format("{0}:{1:00}", (int)timeSinceStart / 60, (int)timeSinceStart % 60); 
		GetComponent<UnityEngine.UI.Text>().text = minSec;
	}
}
