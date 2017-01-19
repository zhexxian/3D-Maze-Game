using UnityEngine;
using System.Collections;

public class CameraMovementControl : MonoBehaviour {

    private CharacterController controller;
    public GameObject target;
    private bool readMap = false;
    public float damping = 1;
    public float xrot = 15;
    public Vector3 offset = new Vector3(0, 2, -2);

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
            transform.eulerAngles = new Vector3(xrot, target.transform.eulerAngles.y, 0);
        }
        
    }
}
