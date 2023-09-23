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
    [SerializeField] private float drag;

    [Header("Grounded Check")]
    [SerializeField] private LayerMask whatIsGround;

    [Header("Slope Check")]
    [SerializeField] private float maxSlopeAngle;
    private RaycastHit slopeHit;
    
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
        moveDirection = orientation.right * verticalInput;

        playerAnimator.SetBool("isWalking", true);

        if(Input.GetKey(Controls.Running) && verticalInput > 0){ //only run forward
            playerAnimator.SetBool("isRunning", true);
            moveDirection *= runningSpeedFactor;
        }
        else{
            playerAnimator.SetBool("isRunning", false);
        }

        if(OnSlope()){
            rb.AddForce(GetSlopeMoveDirection() * direction * movementSpeed * 100f, ForceMode.Force);

            //give bit of a force on the down direction to make the player stick to the slope
            if(rb.velocity.y > 0){
                rb.AddForce(Vector3.down * 400f, ForceMode.Force);
            }
        }   
        else if(isGrounded){
            rb.AddForce(moveDirection.normalized * direction * movementSpeed * 100f, ForceMode.Force);           
        }

        rb.useGravity = !OnSlope();
    }

    private void SpeedControl(){
        if(OnSlope()){
            if(rb.velocity.magnitude > movementSpeed){
                if(Input.GetKey(Controls.Running)){
                    rb.velocity = rb.velocity.normalized * movementSpeed * runningSpeedFactor;
                }
                else{
                    rb.velocity = rb.velocity.normalized * movementSpeed;
                }
            }
        }
        else{
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

        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 1f, whatIsGround);
        Debug.DrawRay(transform.position, Vector3.down * (playerHeight * 0.5f + 1f), Color.red);

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

    private bool OnSlope(){
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 1f, whatIsGround)){
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);   //get the angle between the normal of the slope and the up vector
            return angle < maxSlopeAngle && angle != 0f; //if the angle is less than the max slope angle and the angle is not 0, then the player is on a slope
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection(){
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
}
