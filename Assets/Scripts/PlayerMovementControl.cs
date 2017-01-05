using UnityEngine;
using System.Collections;

public class PlayerMovementControl : MonoBehaviour
{
    public static bool resetPlayer = false;
    public static bool isPlayerFinish = false;
    // Public for GUI parameter input
    public float walkSpeedFactor = 2.0f;
    public float runSpeedFactor = 6.0f;
    public float rotationSpeed = 2.0f;
    public float sideStepSpeed = 0.1f;
    public bool enableSideStep = true;
    public bool controlByMouse = true;
	public GameObject gameOverOverlay;
    public GameObject infoOverlay;
    public GameObject infoText;
    private float speedFactor = 0.0f;
    private float MaxInfoShowTime = 2.0f;
    private float infoShowTime = 0.0f;
    private bool readMap = false;
    private int indexMap = 1; // 1-6 

    public Animator mAnimator;
    private CharacterController controller;
    private int movingState = 0;    // 0 - iddle   || 1 - walking   || 2 - running
    private int prevMovingState = 0;
    private bool onCollision;

    public float getSpeedFactor()
    {
        return (speedFactor * Time.deltaTime);
    }

    public float getSideStepSpeedFactor()
    {
        return (sideStepSpeed * Time.deltaTime);
    }

    public bool isMovementControlKey(int state)
    {
        // state = 0 -> moving ( W / S )
        // state = 1 -> Facing ( A / D )
        bool controlKeyMoving = Input.GetKey("w") || Input.GetKey("up") || Input.GetKey("s") || Input.GetKey("down");
        if (enableSideStep && controlByMouse) controlKeyMoving = controlKeyMoving || Input.GetKey("a") || Input.GetKey("left") || Input.GetKey("d") || Input.GetKey("right");
        bool controlKeyFacing = (controlByMouse) ? (Input.GetAxis("Mouse X") > 0 || Input.GetAxis("Mouse X") < 0) : Input.GetKey("a") || Input.GetKey("left") || Input.GetKey("d") || Input.GetKey("right");
        return state == 0 ? controlKeyMoving : controlKeyFacing;
    }

    // Use this for initialization
    void Start()
    {
        onCollision = false;
        controller = GetComponent<CharacterController>();
        mAnimator = GetComponent<Animator>();
    }

    void updateAnimation()
    {
        switch (movingState)
        {
            case 0: mAnimator.Play("Idle", -1, 0f); break;
            case 1: mAnimator.Play("Walk", -1, 0f); break;
            case 2: mAnimator.Play("Run", -1, 0f); break;
        }
    }

    void placePlayerInStartPosition()
    {
        int mIndex = 1; // always restart in the first side map index
        for (int y = 0; y < MazeDatabase.GetMaze[mIndex].GetLength(0); y++)
        {
            for (int x = 0; x < MazeDatabase.GetMaze[mIndex].GetLength(1); x++)
            {
                if (MazeDatabase.GetMaze[mIndex][y, x] == MazeGenerator.MAZESTART)
                {
                    transform.position = new Vector3(x + mIndex * MazeDatabase.GetMaze[mIndex].GetLength(0), 0.0f, y);
                    return;
                }
            }
        }
    }

    void cheat() {
        if (Input.GetKey("c")) {
            GlobalVariable.CurrGemNumber = GlobalVariable.RequiredGemNumber;
            transform.position = new Vector3(GlobalVariable.GetFinishNodeCoordinate()[0], 0.0f, GlobalVariable.GetFinishNodeCoordinate()[1]);
        }
    }

    void checkInfoOverlay() {
        if (infoOverlay.activeInHierarchy) {
            infoShowTime -= Time.deltaTime;
            if (infoShowTime <= 0)
                infoOverlay.SetActive(false);
        }
    }
    void checkResetPlayer() {
        if (resetPlayer)
        {
            infoShowTime = MaxInfoShowTime;
            infoOverlay.SetActive(true);
            infoText.GetComponent<UnityEngine.UI.Text>().text = GlobalVariable.getResetPlayerText();
            float fadeTime = GetComponent<Fading>().BeginFade(1);
            float startTime = Time.realtimeSinceStartup;
            while (true)
            {
                if (Time.realtimeSinceStartup - startTime > 1)
                {
                    placePlayerInStartPosition();
                    break;
                }
            }
            GetComponent<Fading>().BeginFade(-1);
            resetPlayer = false;
        }
    }

    void checkGameOver(){
		if (Input.GetKey("o")) {
			gameOverOverlay.SetActive (true);
		}
	
	}

    void checkFinish() {
        int[] playerCoordinate = GlobalVariable.GetPlayerCoordinate();
        int[] finishCoordinate = GlobalVariable.ConvertPositionToCoordinate(GlobalVariable.GetFinishNodeCoordinate()[0], GlobalVariable.GetFinishNodeCoordinate()[1]);
        if (playerCoordinate[0] == finishCoordinate[0] &&
            playerCoordinate[1] == finishCoordinate[1] &&
            playerCoordinate[2] == finishCoordinate[2] &&
            GlobalVariable.RequiredGemNumber <= GlobalVariable.CurrGemNumber
            ) {
            infoShowTime = MaxInfoShowTime;
            infoOverlay.SetActive(true);
            infoText.GetComponent<UnityEngine.UI.Text>().text = GlobalVariable.getFinishPlayerText();
            isPlayerFinish = true;
        }
    }


    void Update()
    {
		checkGameOver();
        if (GlobalVariable.onPauseGame || isPlayerFinish) return;
        if (!readMap)
        {
            if (MazeDatabase.GetMaze[1] != null)
            {
                readMap = true;
                movingState = 0;
                placePlayerInStartPosition();
                updateAnimation();
            }
        }
        else
        {
            cheat();
            checkInfoOverlay();
            checkResetPlayer();
            checkFinish();
            if (isMovementControlKey(0) || isMovementControlKey(1))
            {
                movingState = 0;
                if (isMovementControlKey(1))
                {

                    if (controlByMouse)
                    {   // Direction Controled By Mouse
                        controller.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * rotationSpeed);
                        //transform.LookAt(mycam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mycam.nearClipPlane)), Vector3.up);
                    }
                    else
                    {
                        // Direction Controled By keyboard
                        float deltaY = rotationSpeed;
                        if (Input.GetKey("a") || Input.GetKey("left")) deltaY *= -1;
                        controller.transform.Rotate(Vector3.up, deltaY);
                    }

                }
                if (isMovementControlKey(0))
                {

                    //movingState = Input.GetKey("space") && ((Input.GetKey("w") || Input.GetKey("up"))) ? 2 : 1;
                    movingState = 1;
                    //speedFactor = Input.GetKey("space") && ((Input.GetKey("w") || Input.GetKey("up"))) ? runSpeedFactor : walkSpeedFactor;
                    speedFactor = Input.GetKey("space") && ((Input.GetKey("w") || Input.GetKey("up"))) ? walkSpeedFactor : walkSpeedFactor;
                    var z = Input.GetAxis("Vertical") * getSpeedFactor();
                    var x = enableSideStep ? Input.GetAxis("Horizontal") * getSideStepSpeedFactor() : 0;
                    //transform.Translate(x, 0, z);
                    
                    gameObject.GetComponent<CharacterController>().Move(transform.TransformDirection(new Vector3(x,0,z)));
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

        
        if (Input.GetKeyDown(KeyCode.T))
        {
            int[] playerPosition = GlobalVariable.GetPlayerCoordinate();
            int posA = playerPosition[0];
            int posX = playerPosition[1];
            int posZ = playerPosition[2];

            int[] teleportPoint = MazeDatabase.GetTeleportPoint(posA, posZ, posX);
            if (teleportPoint != null)
            {
                float fadeTime = GetComponent<Fading>().BeginFade(1);
                float startTime = Time.realtimeSinceStartup;
                while (true)
                {
                    //print ("teleporting")
                    if (Time.realtimeSinceStartup - startTime > 1)
                    {
                        int des_a = teleportPoint[0];
                        int des_z = teleportPoint[1];
                        int des_x = teleportPoint[2];
                        gameObject.transform.position = new Vector3(des_a * MazeDatabase.GetMaze[des_a].GetLength(1) + des_x, 0, des_z);
                        break;
                    }
                }

                GetComponent<Fading>().BeginFade(-1);
            }
        }
        // Update position to global variable
        GlobalVariable.PlayerPosition = this.transform.localPosition;
    }
}
