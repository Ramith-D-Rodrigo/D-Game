using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerMover : MonoBehaviour
{
    // Start is called before the first frame update
    public float movementSpeed = 15f;
    public float rotationSpeed = 10f;
    public float jumpSpeed = 12f;
    public int direction = -1; //player moving direction
    private readonly float gravity = 9.81f;
    private float verticalVelocity = 0;
    private CharacterController characterController;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveForwardBackward();
        //PlayerJump();
        RotatePlayer();
    }

/*     void PlayerJump(){

        if(characterController.isGrounded){
            verticalVelocity = 0;
            if(Input.GetKeyDown(KeyCode.Space)){
                verticalVelocity = jumpSpeed;
            }
        }
        UnityEngine.Vector3 move = UnityEngine.Vector3.up;
        verticalVelocity -= gravity * Time.deltaTime;

        move.Set(0, verticalVelocity, 0);
        characterController.Move(move * Time.deltaTime);
    } */

    void MoveForwardBackward(){
        float userInput = Input.GetAxis("Vertical");
        UnityEngine.Vector3 move = new(userInput * Time.deltaTime * movementSpeed,0, 0);
        if(Input.GetKey(KeyCode.LeftShift) && userInput > 0){ //only run forward
            move *= 2.5f;
        }

        UnityEngine.Vector3 playerMovement = transform.TransformDirection(move);

        characterController.Move(playerMovement * direction);
    }

    void RotatePlayer(){
        transform.Rotate(0, Input.GetAxis("Horizontal") * Time.deltaTime * rotationSpeed, 0);
    }
}
