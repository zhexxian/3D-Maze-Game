using UnityEngine;
using System.Collections;

public class CameraMovementControl : MonoBehaviour {

    private CharacterController controller;
    public float walkSpeedFactor = 2.0f;
    public float runSpeedFactor = 6.0f;
    private float speedFactor = 0.0f;
    private bool readMap = false;

    public float getSpeedFactor()
    {
        return (speedFactor * Time.deltaTime);
    }

    public bool isMovementControlKey()
    {
        bool controlKey = Input.GetKey("w") || Input.GetKey("up") || Input.GetKey("a") || Input.GetKey("left");
        controlKey = controlKey || Input.GetKey("s") || Input.GetKey("down") || Input.GetKey("d") || Input.GetKey("right");
        return controlKey;
    }

    public Vector3 getVectorToGo()
    {
        if (Input.GetKey("s") || Input.GetKey("down")) return -Camera.main.transform.forward;
        if (Input.GetKey("d") || Input.GetKey("right")) return Camera.main.transform.right;
        if (Input.GetKey("w") || Input.GetKey("up")) return Camera.main.transform.forward;
        return -Camera.main.transform.right;
    }

    void placeCameraInStartPosition()
    {
        bool isFindStartPosition = false;
        for (int y = 0; y < MazeDatabase.GetMaze[1].GetLength(0); y++)
        {
            for (int x = 0; x < MazeDatabase.GetMaze[1].GetLength(1); x++)
            {
                if (MazeDatabase.GetMaze[1][y, x] == "S")
                {
                    transform.position = new Vector3(x + 30, 2, y-2 );
                    isFindStartPosition = true;
                }
                if (isFindStartPosition) break;

            }
            if (isFindStartPosition) break;
        }
    }

    // Use this for initialization
    void Start () {
        controller = GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!readMap)
        {
            if (MazeDatabase.GetMaze[1] != null)
            {
                readMap = true;
                placeCameraInStartPosition();
            }
        }
        else
        {
            if (isMovementControlKey())
            {
                speedFactor = Input.GetKey("space") ? runSpeedFactor : walkSpeedFactor;
                Vector3 mVector = getVectorToGo();
                controller.Move(new Vector3(mVector.x, 0, mVector.z) * getSpeedFactor());
            }
        }
    }
}
