using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadGameScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp("m") && GlobalVariable.onPauseGame)
        {
            GlobalVariable.onPauseGame = false;
            GameObject[] mapSceneObjects = SceneManager.GetSceneByName("map-scene").GetRootGameObjects();
            GameObject[] gameSceneObjects = SceneManager.GetSceneByName("game-scene").GetRootGameObjects();
            GameObject mapCube = GameObject.Find(CreateMazeMap.gameObjectCubeName);
            Destroy(mapCube);
            foreach (GameObject gameObj in gameSceneObjects)
            {
                gameObj.SetActive(true);
            }
            foreach (GameObject gameObj in mapSceneObjects)
            {  
                Destroy(gameObj);
            }
            
            SceneManager.UnloadScene("map-scene");
            //SceneManager.LoadScene("game-scene");
		}
	}
}
