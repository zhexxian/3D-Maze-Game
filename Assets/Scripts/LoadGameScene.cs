using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadGameScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp("m") && GlobalVariable.onMapScene)
        {
            GlobalVariable.onMapScene = false;
            GameObject[] mapSceneObjects = SceneManager.GetSceneByName("map-scene").GetRootGameObjects();
            GameObject[] gameSceneObjects = SceneManager.GetSceneByName("game-scene").GetRootGameObjects();
            GameObject mapCube = GameObject.Find(CreateMazeMap.gameObjectCubeName);
            Destroy(mapCube);
            foreach (GameObject gameObj in gameSceneObjects)
            {
                if(gameObj.name != "GameOver" && gameObj.name != "Congrat" && gameObj.name != "PauseOverlay" && !gameObj.name.StartsWith("gem_") )
                gameObj.SetActive(true);
            }

            foreach (GameObject gameObj in mapSceneObjects)
            {  
                Destroy(gameObj);
            }


			if (GlobalVariable.tutorialCameraIsOn ()) {
				GameObject.FindWithTag ("TimerAndHealthBar").SetActive (false);
			}
			else {
				GameObject.FindWithTag ("Tutorial").SetActive (false);
			}

            SceneManager.UnloadScene("map-scene");
		}
	}
}
