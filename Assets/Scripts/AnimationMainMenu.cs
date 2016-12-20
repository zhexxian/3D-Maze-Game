using UnityEngine;
using System.Collections;

public class AnimationMainMenu : MonoBehaviour
{
    
    public Animator mAnimator;
    private const int IdleState = 0;
    private const int WalkState = 1;
    private const int RunState = 2;
    private int currState = 1;
    private int prevState = 0;

    // Use this for initialization
    void Start()
    {
        mAnimator = GetComponent<Animator>();
        UpdateAnimation();
        //mAnimator.Play("Run", -1, 0f);
    }

    void UpdateAnimation() {
        switch (currState) {
            case IdleState: mAnimator.Play("Idle", -1, 0f); break;
            case WalkState: mAnimator.Play("Walk", -1, 0f); break;
            case RunState: mAnimator.Play("Run", -1, 0f); break;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

}