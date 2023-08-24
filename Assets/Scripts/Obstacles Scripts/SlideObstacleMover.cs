using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideObstacleMover : MonoBehaviour
{
    // Start is called before the first frame update
    public int minSpeed = 40;
    public int maxSpeed = 65;
    public float maxZPos = 14;

    public float moveSpeed;
    private float zPos;
    private float moveDirection = 1;
    private bool isLeft = true;
    private Rigidbody sliderRigidBody;
    void Awake()
    {
        //if zPos is negative is obstacle at the left side, otherwise right side
        zPos = transform.position.z;

        if(zPos < 0){ //left side obstacle
            maxZPos *= -1; //to know the maximum place it can reach
        }
        else{ //right side obstacle
            moveDirection = -1;
            isLeft = false;
        }

        sliderRigidBody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(sliderRigidBody.position.z == zPos){ //cycle start
            moveSpeed = UnityEngine.Random.Range(minSpeed, maxSpeed);
        }
    
        Vector3 target = sliderRigidBody.position + Vector3.forward * Time.deltaTime * moveSpeed * moveDirection;
        sliderRigidBody.MovePosition(target);

        Vector3 rbPos = sliderRigidBody.position;

        if(isLeft){ //left obstacle 
            if(rbPos.z > maxZPos){    //reached the range boundary
                moveDirection = -1;
                sliderRigidBody.position = new Vector3(rbPos.x, rbPos.y, maxZPos);
            }
            else if(rbPos.z < zPos){
                moveDirection = 1;
                sliderRigidBody.position = new Vector3(rbPos.x, rbPos.y, zPos);
            }
        }
        else{   //right obstacle
            if(rbPos.z < maxZPos){    //reached the range boundary
                moveDirection = 1;
                sliderRigidBody.position = new Vector3(rbPos.x, rbPos.y, maxZPos);
            }
            else if(rbPos.z > zPos){
                moveDirection = -1;
                sliderRigidBody.position = new Vector3(rbPos.x, rbPos.y, zPos);
            }
        }
    }
}
