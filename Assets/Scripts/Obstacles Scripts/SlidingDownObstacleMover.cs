using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDownObstacleMover : MonoBehaviour
{
    // Start is called before the first frame update
    //public int minSpeed = 30;
    //public int maxSpeed = 50;
    public float maxYPos = 16f;

    [SerializeField] private float moveSpeed;
    public float MoveSpeed {get { return moveSpeed; } set { moveSpeed = value; } }
    private int moveDirection;

    private Rigidbody sliderRigidBody;
    private float startYPos;

    void Awake()
    {
        sliderRigidBody = this.GetComponent<Rigidbody>();
        startYPos = this.transform.position.y;

        moveDirection = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
/*         if(sliderRigidBody.position.y == startYPos){ //cycle start
            moveSpeed = Random.Range(minSpeed, maxSpeed);
        } */
    
        Vector3 target = sliderRigidBody.position + Vector3.down * Time.deltaTime * moveSpeed * moveDirection;
        sliderRigidBody.MovePosition(target);

        Vector3 rbPos = sliderRigidBody.position;

        if(rbPos.y < maxYPos){    //reached the range boundary
            moveDirection = -1;
        }
        else if(rbPos.y > startYPos){
            moveDirection = 1;
        }
    }
}
