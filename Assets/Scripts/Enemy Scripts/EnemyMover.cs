using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    // Start is called before the first frame update
    public float movementSpeed = 750f;
    public float rotationSpeed = 250f;
    public float runningSpeedFactor = 1.5f;
    public float jumpSpeed = 12f;
    public int direction = -1; //enemy moving direction
    private Rigidbody rb;
    private Animator enemyAnimator;
    private bool isMoving;
    public bool IsMoving {get { return isMoving;} set{ isMoving = value;} }

    [Header("Grounded Check")]
    [SerializeField] private LayerMask whatIsGround;
    private float drag;
    private float enemyHeight;
    private bool isGrounded;
    [SerializeField] private Collider[] bodyCollidersForHeight;

    private SphereCollider terrainCollider;

    private GameObject targettingPlayer; //player transform to rotate towards
    public GameObject TargettingPlayer {get { return targettingPlayer;} set{ targettingPlayer = value;} }

    [SerializeField] private EnemyState enemyState;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        enemyAnimator = GetComponent<Animator>();

        isMoving = false;
        isGrounded = true;
        targettingPlayer = null;

        enemyHeight = 0.0f;

        foreach(Collider collider in bodyCollidersForHeight){
            enemyHeight += collider.bounds.size.y;
        }   

        drag = rb.drag;  

    }

    // Update is called once per frame
    private void Update(){
        CheckForGrounded();
    }

    void FixedUpdate()
    {
        RotateEnemy();

        MoveForwardBackward();

        StopEnemyMovement();
    }


    public void MoveForwardBackward(){       
        if(isGrounded && enemyState.CurrentState == EnemyState.EnemyStates.FollowPlayer){
            enemyAnimator.SetBool("isWalking", true);
            enemyAnimator.SetBool("isRunning", true);
            UnityEngine.Vector3 moveDirectionVec = new(0.0f, 0.0f, Time.fixedDeltaTime);
            moveDirectionVec *= runningSpeedFactor;

            moveDirectionVec = transform.TransformDirection(moveDirectionVec);
            moveDirectionVec = moveDirectionVec * movementSpeed;
            moveDirectionVec.y = rb.velocity.y;

            rb.velocity = moveDirectionVec;
            isMoving = true;
        }
    }

    public void StopEnemyMovement(){
        //enemy has just stopped targetting the player and was moving
        if(isMoving && enemyState.CurrentState != EnemyState.EnemyStates.FollowPlayer){
            enemyAnimator.SetBool("isRunning", false);
            enemyAnimator.SetBool("isWalking", false);
            rb.velocity = new Vector3(0.0f, -1.0f, 0.0f);  //stop the player

            isMoving = false;
        }
    }


    public void RotateEnemy(){
        if(targettingPlayer != null){

            //rotate the enemy towards the player
            Vector3 targetDirection = targettingPlayer.transform.position - transform.position;
            targetDirection.y = 0.0f;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            Quaternion newRotation = Quaternion.Lerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            rb.MoveRotation(newRotation);
    
        }
    }

    
    private void CheckForGrounded()
    {

        isGrounded = Physics.Raycast(transform.position, Vector3.down, enemyHeight * 0.5f + 0.2f, whatIsGround);
        if(isGrounded){
            rb.drag = drag;
        }
        else{
            rb.drag = 0.0f;
        }
    }
}
