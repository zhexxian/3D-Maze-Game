using UnityEngine;
using System.Collections;

public class PlayerMovementControl : MonoBehaviour {

    // Public for GUI parameter input
    public float walkSpeedFactor = 2.0f;
    public float runSpeedFactor = 6.0f;
    private float speedFactor = 0.0f;
    private bool readMap = false;
    

    public Animator mAnimator;
    private CharacterController controller;
    private int movingState = 0;    // 0 - iddle   || 1 - walking   || 2 - running
    private int faceDirection = 2;  // 0 - camera direction   || 1 - right direction   || 2 - forward direction   || 3 - left direction

    private int prevMovingState = 0;

    public float getSpeedFactor(){
		return (speedFactor*Time.deltaTime);
	}

    public bool isMovementControlKey() {
        bool controlKey = Input.GetKey("w") || Input.GetKey("up") || Input.GetKey("a") || Input.GetKey("left");
        controlKey = controlKey || Input.GetKey("s") || Input.GetKey("down") || Input.GetKey("d") || Input.GetKey("right");
        return controlKey;
    }

    public int getDiretionToGo() {
        if (Input.GetKey("s") || Input.GetKey("down"))  return 0;
        if (Input.GetKey("d") || Input.GetKey("right")) return 1;
        if (Input.GetKey("w") || Input.GetKey("up"))    return 2;
        return 3;
    }

    // Use this for initialization
    void Start () {
		controller = GetComponent<CharacterController>();
        mAnimator = GetComponent<Animator>();
        locatePlayer();
    }

    void locatePlayer(){
        bool isFindStartLocation = false;
        for (int y = 0; y < MazeDatabase.GetMaze[1].GetLength(0); y++)
        {
            for (int x = 0; x < MazeDatabase.GetMaze[1].GetLength(1); x++)
            {
                if (MazeDatabase.GetMaze[1][y, x] == "S")
                {
                    transform.position = new Vector3(y * 30 + 10, 0.0f, x);
                    isFindStartLocation = true;
                }
            }
        }


    }

    void updateAnimation() {
        switch (movingState)
        {
            case 0: mAnimator.Play("Idle", -1, 0f); break;
            case 1: mAnimator.Play("Walk", -1, 0f); break;
            case 2: mAnimator.Play("Run", -1, 0f); break;
        }    
    }
    void placePlayerInStartPosition()
    {
        bool isFindStartPosition = false;
        for (int y = 0; y < MazeDatabase.GetMaze[1].GetLength(0); y++)
        {
            for (int x = 0; x < MazeDatabase.GetMaze[1].GetLength(1); x++)
            {
                if (MazeDatabase.GetMaze[1][y, x] == "S")
                {
                    transform.position = new Vector3(x+30, 0.0f, y);
                    isFindStartPosition = true;
                }
                    if (isFindStartPosition) break;

                }
            if (isFindStartPosition) break;
        }
    }
    // Update is called once per frame
    void Update () {
        if (!readMap)
        {
            if (MazeDatabase.GetMaze[1] != null)
            {
                readMap = true;
                placePlayerInStartPosition();
            }
        }
        else {
            if (isMovementControlKey())
            {
                speedFactor = Input.GetKey("space") ? runSpeedFactor : walkSpeedFactor;
                movingState = Input.GetKey("space") ? 2 : 1;
                int direction = getDiretionToGo();
                int rotateDegree = (faceDirection - direction) * 90;
                controller.transform.Rotate(Vector3.up, rotateDegree);
                transform.Translate(0.0f, 0, getSpeedFactor());
                faceDirection = direction;
            }
            else
            {
                movingState = 0;
                speedFactor = 0f;
            }
            if (!(prevMovingState == movingState))
            {
                prevMovingState = movingState;
                updateAnimation();
            }
        }
       
    }



}
