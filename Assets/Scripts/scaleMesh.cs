using UnityEngine;
using System.Collections;

public class scaleMesh : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
        if(gameObject.name == "Ground")GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(transform.localScale.x, transform.localScale.z);
        else GetComponent<MeshRenderer>().material.mainTextureScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);

    }
	
	// Update is called once per frame
	void Update () {
        //transform.Translate(new Vector3(0,0,0.5f));
	}
}
