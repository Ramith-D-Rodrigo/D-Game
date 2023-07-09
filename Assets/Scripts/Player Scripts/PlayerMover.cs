using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerMover : MonoBehaviour
{
    // Start is called before the first frame update
    public float movementSpeed = 1000f;
    public float rotationSpeed = 250f;
    public float jumpSpeed = 12f;
    public float maximumVelocity = 10f;
    public int direction = -1; //player moving direction
    private readonly float gravity = 9.81f;
    private float verticalVelocity = 0;
    private Rigidbody rigidbody;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.maxLinearVelocity =  maximumVelocity;
    }

    // Update is called once per frame
    void FixedUpdate()
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
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)){
            float userInput = Input.GetAxis("Vertical");
            UnityEngine.Vector3 move = new(userInput * Time.deltaTime, 0, 0);
            if(Input.GetKey(KeyCode.LeftShift) && userInput > 0){ //only run forward
                rigidbody.maxLinearVelocity = maximumVelocity * 2.5f; //set the max velocity for running
                move *= 2.5f;
            }
            else{
                rigidbody.maxLinearVelocity = maximumVelocity;  //change back to normal max velocity
            }

            rigidbody.AddRelativeForce(move * movementSpeed * direction);
        }
        else{
            rigidbody.velocity = UnityEngine.Vector3.zero;  //stop the player
        }

    }

    void RotatePlayer(){
        float playerRotation = Input.GetAxis("Horizontal") * Time.deltaTime * rotationSpeed;
        UnityEngine.Quaternion deltaRotation = UnityEngine.Quaternion.Euler(0f, playerRotation, 0f);
        rigidbody.MoveRotation(rigidbody.rotation * deltaRotation);
    }
}
