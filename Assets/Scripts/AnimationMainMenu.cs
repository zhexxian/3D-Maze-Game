using UnityEngine;
using System.Collections;

public class AnimationMainMenu : MonoBehaviour
{
    
    public Animator mAnimator;
    public Animation mAnimation;
    public bool useAnimator = true;
    private const int IdleState = 0;
    private const int WalkState = 1;
    private const int RunState = 2;
    private int currState = 2;
    //private int prevState = 0;

    // Use this for initialization
    void Start()
    {
        mAnimator = GetComponent<Animator>();
        mAnimation = GetComponent<Animation>();
        if(!useAnimator)mAnimation.wrapMode = WrapMode.Loop;
        UpdateAnimation();
    }

    void UpdateAnimation() {
        switch (currState) {
            case IdleState:
                if (useAnimator)mAnimator.Play("Idle", -1, 0f);
                else mAnimation.Play("idle");
                break;
            case WalkState:
                if (useAnimator)mAnimator.Play("Walk", -1, 0f);
                else mAnimation.Play("walk");
                break;
            case RunState:
                if (useAnimator)mAnimator.Play("Run", -1, 0f);
                else mAnimation.Play("run");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

}