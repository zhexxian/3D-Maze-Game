using UnityEngine;
using System.Collections;

public class AiScript : MonoBehaviour {

    private float speedFactor = 2f;
    private int faceDirection = 0;
    NavMeshAgent agent;
    public Transform target;

    Animator mAnimator;
    Animation mAnimation;
    int jumpHash = Animator.StringToHash("Jump");
	// Use this for initialization
	void Start () {
        mAnimator = GetComponent<Animator>();
        mAnimation = GetComponent<Animation>();
        agent = GetComponent<NavMeshAgent>();
    }

    bool seenPlayer() {
        return false;
    }
	// Update is called once per frame
	void Update () {
        agent.SetDestination(target.position);
        if (seenPlayer()){
            //engage player
        }
        else {
            //random path
            if (Input.anyKeyDown)
            {
                //gameObject.animation.wrapMode = WrapMode.Loop;

                //mAnimation.wrapMode = WrapMode.Loop;
                //mAnimation.Play("Run");
                mAnimator.Play("Run", -1, 0f);
            }
            
            
        }
        
            
        
	}
}
