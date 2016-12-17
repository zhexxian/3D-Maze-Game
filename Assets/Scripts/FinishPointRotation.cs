using UnityEngine;
using System.Collections;

public class FinishPointRotation : MonoBehaviour {

    int degree;
	// Use this for initialization
	void Start () {
	
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
	}
}
