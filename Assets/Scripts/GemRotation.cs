using UnityEngine;
using System.Collections;

public class GemRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.Rotate(2, -2, 2);
	}
}
