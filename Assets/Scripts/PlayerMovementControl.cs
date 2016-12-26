using UnityEngine;
using System.Collections;

public class PlayerMovementControl : MonoBehaviour {
    public static bool resetPlayer = false;
    // Public for GUI parameter input
    public float walkSpeedFactor = 2.0f;
    public float runSpeedFactor = 6.0f;
    public float rotationSpeed = 2.0f;
    public float sideStepSpeed = 0.1f;
    public bool enableSideStep = true;
    public bool controlByMouse = true;
    private float speedFactor = 0.0f;
    private bool readMap = false;
    private int indexMap = 1; // 1-6 
    

    public Animator mAnimator;
    private CharacterController controller;
    private int movingState = 0;    // 0 - iddle   || 1 - walking   || 2 - running
    private int prevMovingState = 0;

    public float getSpeedFactor(){
		return (speedFactor*Time.deltaTime);
	}

    public bool isMovementControlKey(int state) {
        // state = 0 -> moving ( W / S )
        // state = 1 -> Facing ( A / D )
        bool controlKeyMoving = Input.GetKey("w") || Input.GetKey("up") || Input.GetKey("s") || Input.GetKey("down");
        if(enableSideStep && controlByMouse) controlKeyMoving = controlKeyMoving || Input.GetKey("a") || Input.GetKey("left") || Input.GetKey("d") || Input.GetKey("right");
        bool controlKeyFacing = (controlByMouse) ? (Input.GetAxis("Mouse X") > 0 || Input.GetAxis("Mouse X") < 0) : Input.GetKey("a") || Input.GetKey("left") || Input.GetKey("d") || Input.GetKey("right");
        return state==0? controlKeyMoving : controlKeyFacing;
    }

    // Use this for initialization
    void Start () {
        controller  = GetComponent<CharacterController>();
        mAnimator   = GetComponent<Animator>();
        GlobalVariable.CurrGemNumber = 0;     
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
        for (int y = 0; y < MazeDatabase.GetMaze[indexMap].GetLength(0); y++)
        {
            for (int x = 0; x < MazeDatabase.GetMaze[indexMap].GetLength(1); x++)
            {
                if (MazeDatabase.GetMaze[indexMap][y, x] == MazeGenerator.MAZESTART)
                {
                    transform.position = new Vector3(x + indexMap*MazeDatabase.GetMaze[indexMap].GetLength(0), 0.0f, y);
                    return;
                }
            }
        }
    }
    
    void Update () {
        if (GlobalVariable.onPauseGame) return;
        if (!readMap)
        {
            if (MazeDatabase.GetMaze[indexMap] != null)
            {
                readMap = true;
                movingState = 0;
                placePlayerInStartPosition();
                updateAnimation();
            }
        }
        else {
            if (resetPlayer) {
                placePlayerInStartPosition();
                resetPlayer = false;
            }
            if (isMovementControlKey(0) || isMovementControlKey(1))
            {
                movingState = 0;
                if (isMovementControlKey(1)) {
                    
                    if (controlByMouse)
                    {   // Direction Controled By Mouse
                        controller.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * rotationSpeed);
                        //transform.LookAt(mycam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mycam.nearClipPlane)), Vector3.up);
                    }
                    else {
                        // Direction Controled By keyboard
                        float deltaY = rotationSpeed;
                        if (Input.GetKey("a") || Input.GetKey("left")) deltaY *= -1;
                        controller.transform.Rotate(Vector3.up, deltaY);
                    }

                }
                if (isMovementControlKey(0)) {
                    movingState = Input.GetKey("space") && ((Input.GetKey("w") || Input.GetKey("up"))) ? 2 : 1;
                    speedFactor = Input.GetKey("space") && ((Input.GetKey("w") || Input.GetKey("up"))) ? runSpeedFactor : walkSpeedFactor;
                    var z = Input.GetAxis("Vertical") * getSpeedFactor();
                    var x = enableSideStep ? Input.GetAxis("Horizontal") * sideStepSpeed  : 0;
                    transform.Translate(x, 0, z);
                }
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
        // Update position to global variable
        GlobalVariable.PlayerPosition = transform.position;
    }

}
