using UnityEngine;
using System.Collections;

public class FinishPointRotation : MonoBehaviour {

    int degree;
    int[] finishpointCoordinate;
	// Use this for initialization
	void Start () {
        finishpointCoordinate = GlobalVariable.ConvertPositionToCoordinate(transform.position.x, transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.Rotate(0, 1, 2);
        degree++;
        if (degree==360)
        {
            degree = 0;
        }
        float sinValue = Mathf.Sin(Mathf.Deg2Rad * degree);
        gameObject.transform.position = gameObject.transform.position - new Vector3(0, gameObject.transform.position.y - sinValue*0.1f - 0.6f, 0);

        int[] playerCoordinate = GlobalVariable.GetPlayerCoordinate();

        if ((playerCoordinate[0] == finishpointCoordinate[0]) &&
            (playerCoordinate[1] == finishpointCoordinate[1]) &&
            (playerCoordinate[2] == finishpointCoordinate[2]))
        {
            //finish code here

            Destroy(gameObject);
        }
    }
}
