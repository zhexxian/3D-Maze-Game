using UnityEngine;
using System.Collections;

public class GemRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.Rotate(0, -2, 0);
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "MainPlayer")
        {
            Destroy(gameObject);
            GlobalVariable.CurrGemNumber += 1;
        }
    }
}
