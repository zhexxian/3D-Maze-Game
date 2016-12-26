using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadMapScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp("m") && !GlobalVariable.onPauseGame)
        {
            GlobalVariable.onPauseGame = true;
            //SceneManager.LoadScene("map-scene");
            SceneManager.LoadScene("map-scene", LoadSceneMode.Additive);
            
            
		}
	}
}
