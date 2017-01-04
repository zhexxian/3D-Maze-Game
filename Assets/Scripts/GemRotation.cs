using UnityEngine;
using System.Collections;

public class GemRotation : MonoBehaviour {

    int[] gemCoordinate;
	// Use this for initialization
	void Start () {
        gemCoordinate = GlobalVariable.ConvertPositionToCoordinate(transform.position.x, transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.Rotate(0, -2, 0);

        /*
        print(GlobalVariable.GetPlayerCoordinate()[0].ToString() + " " +
            GlobalVariable.GetPlayerCoordinate()[1].ToString() + " " +
            GlobalVariable.GetPlayerCoordinate()[2].ToString() + " " +
            gemCoordinate[0].ToString() + " " +
            gemCoordinate[1].ToString() + " " +
            gemCoordinate[2].ToString()
            );
        */
        int[] playerCoordinate = GlobalVariable.GetPlayerCoordinate();
        if ((playerCoordinate[0]==gemCoordinate[0]) &&
            (playerCoordinate[1] == gemCoordinate[1]) &&
            (playerCoordinate[2] == gemCoordinate[2]))
        {
            Debug.Log("adsf");
            GameObject.Find("MainPlayer/PSGemCollect").GetComponent<ParticleSystem>().Play();
            Destroy(gameObject);            
        }
	}
}
