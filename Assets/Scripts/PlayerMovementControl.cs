using UnityEngine;
using System.Collections;

public class PlayerMovementControl : MonoBehaviour {

	private CharacterController controller;
	private float speedFactor = 2f;
    private int faceDirection = 0;
    // 0 - camera direction
    // 1 - right direction
    // 2 - forward direction
    // 3 - left direction

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
    public Vector3 getVectorToGo(){
        if (Input.GetKey("s") || Input.GetKey("down")) return -Camera.main.transform.forward;
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
            int direction = getDiretionToGo();
            int rotateDegree = (faceDirection - direction) * 90;
            controller.transform.Rotate(Vector3.up, rotateDegree);

            Vector3 mVector = getVectorToGo();
            controller.Move(new Vector3(mVector.x, 0, mVector.z) * getSpeedFactor());
            faceDirection = direction;
        }
		
    }
}
