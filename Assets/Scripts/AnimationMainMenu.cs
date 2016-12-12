using UnityEngine;
using System.Collections;

public class AnimationMainMenu : MonoBehaviour {
    
    public Animator mAnimator;
    
    // Use this for initialization
    void Start()
    {
        mAnimator = GetComponent<Animator>();
        mAnimator.Play("Run", -1, 0f);
    }

    // Update is called once per frame
    void Update()
    {
    }

}
