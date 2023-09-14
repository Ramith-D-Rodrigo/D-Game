using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    // Start is called before the first frame update
    public float movementSpeed = 750f;
    public float rotationSpeed = 250f;
    public float runningSpeedFactor = 1.3f;
    public float jumpSpeed = 12f;
    public int direction = -1; //enemy moving direction
    private Rigidbody rb;
    private Animator enemyAnimator;
    private bool isMoving;

    [Header("Grounded Check")]
    [SerializeField] private LayerMask whatIsGround;
    private float drag;
    private float enemyHeight;
    private bool isGrounded;
    [SerializeField] private PlayerCollision playerCollision;

    [SerializeField] private Collider[] bodyCollidersForHeight;

    private SphereCollider terrainCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        enemyAnimator = GetComponent<Animator>();

        isMoving = false;
        isGrounded = true;

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

    }


    public void MoveForwardBackward(Transform movingTransform){       
        if(movingTransform != null && isGrounded){
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
        if(isMoving){
            enemyAnimator.SetBool("isRunning", false);
            enemyAnimator.SetBool("isWalking", false);
            rb.velocity = new Vector3(0.0f, -1.0f, 0.0f);  //stop the player

            isMoving = false;
        }
    }


    public void RotateEnemy(Transform rotatingTransform){
        if(rotatingTransform != null){
            Vector3 targetPosition = rotatingTransform.position;
            targetPosition.y = rb.transform.position.y;
            rb.transform.LookAt(targetPosition);
    
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
