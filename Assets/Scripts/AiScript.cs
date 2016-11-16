using UnityEngine;
using System.Collections;

public class AiScript : MonoBehaviour {

    private Animator mAnimator;
	void Start () {
        mAnimator = GetComponent<Animator>();
    }

    bool seenPlayer() {
        return false;
    }
	// Update is called once per frame
	void Update () {
      
            if (Input.GetKeyDown("s") || Input.GetKeyDown("down"))
            {
                //mAnimation.wrapMode = WrapMode.Loop;
                //mAnimation.Play("Run");
                mAnimator.Play("Walk", -1, 0f);
            }
            
            
            
        
	}
}
