using UnityEngine;
using System.Collections;

public class CameraMovementControl : MonoBehaviour {

    private CharacterController controller;
    public float walkSpeedFactor = 2.0f;
    public float runSpeedFactor = 6.0f;
    private float speedFactor = 0.0f;

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
        if (Input.GetKey("s") || Input.GetKey("downa")) return -Camera.main.transform.forward;
        if (Input.GetKey("d") || Input.GetKey("right")) return Camera.main.transform.right;
        if (Input.GetKey("w") || Input.GetKey("up")) return Camera.main.transform.forward;
        return -Camera.main.transform.right;
    }

    // Use this for initialization
    void Start () {
        controller = GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (isMovementControlKey()){
            speedFactor = Input.GetKey("space") ? runSpeedFactor : walkSpeedFactor;
            Vector3 mVector = getVectorToGo();
            controller.Move(new Vector3(mVector.x, 0, mVector.z) * getSpeedFactor());
        }
    }
}
