using UnityEngine;
using System.Collections;

public class AiScript : MonoBehaviour {

    private Animator mAnimator;
    private Animation mAnimation;
    // Public for GUI parameter input
    public float walkSpeedFactor = 2.0f;
    public float runSpeedFactor = 6.0f;
    private float speedFactor = 2.0f;
    private float stateTime = 0.0f;

    private CharacterController controller;
    private int movingState = 0;    // 0 - iddle   || 1 - walking   || 2 - running
    private int faceDirection = 2;  // 0 - camera direction   || 1 - right direction   || 2 - forward direction   || 3 - left direction
    private int prevMovingState = 0;
    private Vector2 nextTargetKoordinat;

    private bool readMap = false;
    public float getSpeedFactor()
    {
        return (speedFactor * Time.deltaTime);
    }

    public void nextCordinate() {
        // later find available coordinate path 
        nextTargetKoordinat = new Vector2(Random.Range(30f,40f), Random.Range(-5f, 5f));
    }

    public int getDiretionToGo()
    {
        float radius = 0.5f;
        if (nextTargetKoordinat.x - radius > controller.transform.position.x) // go to right
            return 1;
        if (nextTargetKoordinat.x + radius < controller.transform.position.x) // go to left
            return 3;
        if (nextTargetKoordinat.y - radius > controller.transform.position.z) // go to up
            return 2;
        if (nextTargetKoordinat.y + radius < controller.transform.position.z) // go to down
            return 0;

        //movingState = 0;
        //updateAnimation();
        return faceDirection;
    }

    public bool isSeenPlayer() {
        return false;
    }

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<CharacterController>();
        mAnimator = GetComponent<Animator>();
        mAnimation = GetComponent<Animation>();
        mAnimation.wrapMode = WrapMode.Loop;
    }

    void updateAnimation()
    {
        switch (movingState)
        {
            case 0:
                //mAnimator.Play("Idle", -1, 0f); break;
                mAnimation.Play("idle");break;
            case 1:
                //mAnimator.Play("Walk", -1, 0f); break;
                mAnimation.Play("walk"); break;
            case 2:
                //mAnimator.Play("Run", -1, 0f); break;
                mAnimation.Play("run"); break;
        }
    }

    void placeAIInStartPosition()
    {
        int xAI = 0;
        int yAI = 0;
        for (int y = 0; y < MazeDatabase.GetMaze[1].GetLength(0); y++)
        {
            for (int x = 0; x < MazeDatabase.GetMaze[1].GetLength(1); x++)
            {
                if (MazeDatabase.GetMaze[1][y, x] == "S")
                {
                    xAI = x;
                    yAI = y;
                }

            }
        }
        transform.position = new Vector3(xAI + 30, 0, yAI);
    }

    // Update is called once per frame
    void Update()
    {
        if (!readMap)
        {
            if (MazeDatabase.GetMaze[1] != null)
            {
                readMap = true;
                placeAIInStartPosition();
            }
        }
        else
        {

            if (isSeenPlayer())
            {
                // engage player using A* path finding
            }
            else
            {
                stateTime += Time.deltaTime;
                if (stateTime >= 3.0f && movingState == 0)
                {
                    // Start patroling
                    nextCordinate();
                    movingState = 1;
                    stateTime = 0.0f;
                    updateAnimation();
                }
                else if (stateTime >= 5.0f && (movingState == 1 || movingState == 2))
                {
                    // Start to iddle
                    movingState = 0;
                    speedFactor = walkSpeedFactor;
                    stateTime = 0.0f;
                    updateAnimation();
                }

                if (movingState == 1)
                {
                    // Patroling random available path
                    int direction = getDiretionToGo();
                    int rotateDegree = (faceDirection - direction) * 90;
                    controller.transform.Rotate(Vector3.up, rotateDegree);
                    transform.Translate(0.0f, 0, getSpeedFactor());
                    faceDirection = direction;
                }
            }
        }
        


    }
}
