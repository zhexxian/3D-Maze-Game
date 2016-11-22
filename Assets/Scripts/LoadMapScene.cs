using UnityEngine;
using System.Collections;

public class LoadMapScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey("m")){
			Application.LoadLevel("map-scene");
		}
	}
}
