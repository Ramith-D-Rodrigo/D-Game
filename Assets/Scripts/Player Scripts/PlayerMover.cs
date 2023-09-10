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
    public float runningSpeedFactor = 2f;
    public float jumpSpeed = 12f;
    public int direction = -1; //player moving direction
    private readonly float gravity = 9.81f;
    private float verticalVelocity = 0;
    private Rigidbody rb;
    private Animator playerAnimator;
    private bool isMoving;

    [Header("Grounded Check")]
    [SerializeField] private LayerMask whatIsGround;
    private float drag;
    private float playerHeight;
    private bool isGrounded;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private PlayerCollision playerCollision;

    [SerializeField] private Collider[] bodyCollidersForHeight;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();

        isMoving = false;
        isGrounded = true;

        playerHeight = 0.0f;

        foreach(Collider collider in bodyCollidersForHeight){
            playerHeight += collider.bounds.size.y;
        }   

        drag = rb.drag;  
    }

    // Update is called once per frame
    private void Update(){
        switch(levelManager.CurrLevelIndex){
            case 1:
                ProcessLevelOneMovement();
                break;

            case 2:
                ProcessLevelTwoMovement();
                break;
        }
    }

    void FixedUpdate()
    {
        MoveForwardBackward();

        StopPlayerMovement();

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
        if((Input.GetKey(Controls.GoForward) || Input.GetKey(Controls.GoBackward)) && isGrounded){
            playerAnimator.SetBool("isWalking", true);
            float userInput = Input.GetAxis("Vertical");
            UnityEngine.Vector3 move = new(userInput * Time.fixedDeltaTime, 0f, 0f);
            
            if(Input.GetKey(Controls.Running) && userInput > 0){ //only run forward
                playerAnimator.SetBool("isRunning", true);
                move *= runningSpeedFactor;
            }
            else{
                playerAnimator.SetBool("isRunning", false);
            }

            move = transform.TransformDirection(move);
            move = move * movementSpeed * direction;
            move.y = rb.velocity.y;

            rb.velocity = move;
            isMoving = true;
        }
    }

    private void StopPlayerMovement(){
        //player has just released the control keys or has released the keys but the variable is still moving true
        if(Input.GetKeyUp(Controls.GoForward) || Input.GetKeyUp(Controls.GoBackward) || (!(Input.GetKey(Controls.GoForward) || Input.GetKey(Controls.GoBackward)) && isMoving)){
            playerAnimator.SetBool("isRunning", false);
            playerAnimator.SetBool("isWalking", false);
            rb.velocity = new UnityEngine.Vector3(0.0f, -1.0f, 0.0f);  //stop the player

            isMoving = false;
        }
    }


    void RotatePlayer(){
        float playerRotation = Input.GetAxis("Horizontal") * Time.fixedDeltaTime * rotationSpeed;
        UnityEngine.Quaternion deltaRotation = UnityEngine.Quaternion.Euler(0f, playerRotation, 0f);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    
    private void ProcessLevelTwoMovement()
    {

        isGrounded = Physics.Raycast(transform.position, UnityEngine.Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        if(isGrounded){
            rb.drag = drag;
        }
        else{
            rb.drag = 0.0f;
        }
    }

    private void ProcessLevelOneMovement()
    {

    }
}
