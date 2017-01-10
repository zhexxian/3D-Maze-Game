using UnityEngine;
using System.Collections;

public class TutorialNextStep : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp("enter"))
		{
			GlobalVariable.turnOnTutorialCamera (false);
		}
	}
}
