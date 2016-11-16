using UnityEngine;
using System.Collections;

public class SimpleMovementScript : MonoBehaviour {

    //Variables
    public float speed = 6.0F;
    public float jumpSpeed = 10f;
    public float gravity = 20.0F;

    private float xPlayer = 0f;
    private float yPlayer = 0f;
    private float zPlayer = 0f;

    private Vector3 moveDirection = Vector3.zero;

    void Update1()
    {
        CharacterController controller = GetComponent<CharacterController>();
        // is the controller on the ground?
        if (controller.isGrounded)
        {
            //Feed moveDirection with input.
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            //Multiply it by speed.
            moveDirection *= speed;
            //Jumping
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;

        }
        //Applying gravity to the controller
        //moveDirection.y -= gravity * Time.deltaTime;
        //Making the character move
        transform.Translate(moveDirection.x, moveDirection.y, moveDirection.z);
        //controller.Move(moveDirection * Time.deltaTime);
    }

    // Update is called once per frame
    void Update() {
        CharacterController controller = GetComponent<CharacterController>();
            xPlayer = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
            zPlayer = Input.GetAxis("Vertical") * Time.deltaTime * speed;
       
      
        
        //       transform.Rotate(0, x, 0);
        transform.Translate(xPlayer, 0, zPlayer);
     //   
    }
}
