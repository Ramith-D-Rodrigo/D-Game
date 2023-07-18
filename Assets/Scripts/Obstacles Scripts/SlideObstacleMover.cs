using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideObstacleMover : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed = 25f;
    public float maxZPos = 14;
    private float zPos;
    private float moveDirection = 1;
    private bool isLeft = true;
    void Start()
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
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed * moveDirection);

        if(isLeft){ //left obstacle 
            if(transform.position.z >= maxZPos){    //reached the range boundary
                moveDirection = -1;
                transform.position.Set(transform.position.x, transform.position.y, maxZPos - 1);
            }
            else if(transform.position.z <= zPos){
                moveDirection = 1;
                transform.position.Set(transform.position.x, transform.position.y, zPos + 1);
            }
        }
        else if(!isLeft){   //right obstacle
            if(transform.position.z <= maxZPos){    //reached the range boundary
                moveDirection = 1;
                transform.position.Set(transform.position.x, transform.position.y, maxZPos + 1);
            }
            else if(transform.position.z >= zPos){
                moveDirection = -1;
                transform.position.Set(transform.position.x, transform.position.y, zPos - 1);
            }
        }
    }
}
