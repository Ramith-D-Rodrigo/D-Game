using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerMover : MonoBehaviour
{
    // Start is called before the first frame update
    public float movementSpeed = 20f;
    public float rotationSpeed = 250f;
    public float runningSpeedFactor = 2f;
    public float jumpSpeed = 12f;
    public int direction = -1; //player moving direction
    private Rigidbody rb;
    private Animator playerAnimator;

    [Header("Grounded Check")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float drag;
    private float playerHeight;
    private bool isGrounded;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private PlayerCollision playerCollision;

    [SerializeField] private Collider[] bodyCollidersForHeight;

    [SerializeField] private Transform orientation; //use x direction as the forward of the player (positive x is backward, negative x is forward)
    Vector3 moveDirection = UnityEngine.Vector3.zero;
    private float horizontalInput;
    private float verticalInput;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();

        isGrounded = true;

        playerHeight = 0.0f;

        foreach(Collider collider in bodyCollidersForHeight){
            playerHeight += collider.bounds.size.y;
        }   

        rb.drag = drag;
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

        GetUserInput();

        SpeedControl();
    }

    void FixedUpdate()
    {
        MoveForwardBackward();

        ProcessStopPlayerMovement();

        //PlayerJump();
        RotatePlayer();
    }

    private void GetUserInput(){
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    void MoveForwardBackward(){       
        if(isGrounded){
            playerAnimator.SetBool("isWalking", true);

            Vector3 moveDirection = orientation.right * verticalInput;
            if(Input.GetKey(Controls.Running) && verticalInput > 0){ //only run forward
                playerAnimator.SetBool("isRunning", true);
                moveDirection *= runningSpeedFactor;
            }
            else{
                playerAnimator.SetBool("isRunning", false);
            }

            rb.AddForce(moveDirection.normalized * direction * movementSpeed * 100f, ForceMode.Force);
        }
    }

    private void SpeedControl(){
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(Input.GetKey(Controls.Running)){
            if(flatVel.magnitude > movementSpeed * runningSpeedFactor){
                Vector3 maxVel = flatVel.normalized * movementSpeed * runningSpeedFactor;
                rb.velocity = new Vector3(maxVel.x, rb.velocity.y, maxVel.z);
            }
        }
        else{
            if(flatVel.magnitude > movementSpeed){
                Vector3 maxVel = flatVel.normalized * movementSpeed;
                rb.velocity = new Vector3(maxVel.x, rb.velocity.y, maxVel.z);
            }
        }
    }

    private void ProcessStopPlayerMovement(){
        if(verticalInput == 0){
            StopMovement();
        }
    }

    public void StopMovement(){
        playerAnimator.SetBool("isRunning", false);
        playerAnimator.SetBool("isWalking", false);
    }

    void RotatePlayer(){
        float playerRotation = horizontalInput * Time.deltaTime * rotationSpeed;
        Quaternion deltaRotation = Quaternion.Euler(0f, playerRotation, 0f);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    
    private void ProcessLevelTwoMovement()
    {

        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

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
