using UnityEngine;
using System.Collections;

public class CameraMovementControl : MonoBehaviour {

    private CharacterController controller;
    public GameObject target;
    private bool readMap = false;
    public float damping = 1;
    public Vector3 offset = new Vector3(0, 3, -2);

    void placeCameraInStartPosition()
    {
        bool isFindStartPosition = false;
        for (int y = 0; y < MazeDatabase.GetMaze[1].GetLength(0); y++)
        {
            for (int x = 0; x < MazeDatabase.GetMaze[1].GetLength(1); x++)
            {
                if (MazeDatabase.GetMaze[1][y, x] == MazeGenerator.MAZESTART)
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
        target = GameObject.Find("MainPlayer");
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
    }

    void LateUpdate()
    {
        if (readMap)
        {
            transform.position = target.transform.position;
            transform.eulerAngles = new Vector3(0, target.transform.eulerAngles.y, 0);
            transform.Translate(offset);
            transform.eulerAngles = new Vector3(35, target.transform.eulerAngles.y, 0);
        }
        
    }
}
