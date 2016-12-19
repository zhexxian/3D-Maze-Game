using UnityEngine;
using System.Collections;

public class PlayerMovementControl : MonoBehaviour {

    // Public for GUI parameter input
    public float walkSpeedFactor = 2.0f;
    public float runSpeedFactor = 6.0f;
    public float rotationSpeed = 2.0f;
    private float speedFactor = 0.0f;
    private bool readMap = false;
    private int indexMap = 1; // 1-6 
    

    public Animator mAnimator;
    private CharacterController controller;
    private int movingState = 0;    // 0 - iddle   || 1 - walking   || 2 - running
    private int faceDirection = 2;  // 0 - camera direction   || 1 - right direction   || 2 - forward direction   || 3 - left direction

    private int prevMovingState = 0;

    public float getSpeedFactor(){
		return (speedFactor*Time.deltaTime);
	}

    public bool isMovementControlKey(int state) {
        // state = 0 -> moving ( W / S )
        // state = 1 -> Facing ( A / D )
        bool controlKeyMoving = Input.GetKey("w") || Input.GetKey("up") || Input.GetKey("s") || Input.GetKey("down");
        bool controlKeyFacing = Input.GetKey("a") || Input.GetKey("left") || Input.GetKey("d") || Input.GetKey("right");
        return state==0? controlKeyMoving : controlKeyFacing;
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
        for (int y = 0; y < MazeDatabase.GetMaze[indexMap].GetLength(0); y++)
        {
            for (int x = 0; x < MazeDatabase.GetMaze[indexMap].GetLength(1); x++)
            {
                if (MazeDatabase.GetMaze[indexMap][y, x] == "S")
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
            
            if (isMovementControlKey(0) || isMovementControlKey(1))
            {

                movingState = 0;
                if (isMovementControlKey(1)) {
                    float deltaY = rotationSpeed;
                    if (Input.GetKey("a") || Input.GetKey("left")) deltaY *= -1;
                    controller.transform.Rotate(Vector3.up, deltaY);
                }
                if (isMovementControlKey(0)) {
                    movingState = Input.GetKey("space") && ((Input.GetKey("w") || Input.GetKey("up"))) ? 2 : 1;
                    speedFactor = Input.GetKey("space") && ((Input.GetKey("w") || Input.GetKey("up"))) ? runSpeedFactor : walkSpeedFactor;
                    var z = Input.GetAxis("Vertical") * getSpeedFactor();
                    transform.Translate(0, 0, z);
                }
                
                
                //int direction = getDiretionToGo();
                //int rotateDegree = (faceDirection - direction) * 90;
                //controller.transform.Rotate(Vector3.up, rotateDegree);
                //transform.Translate(0.0f, 0, getSpeedFactor());
                //faceDirection = direction;
                
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
