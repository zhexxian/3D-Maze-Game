﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadMapScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey("m")){
			SceneManager.LoadScene("map-scene");
		}
	}
}
