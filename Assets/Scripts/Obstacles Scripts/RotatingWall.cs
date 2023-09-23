using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingWall : MonoBehaviour
{
    private float rotationSpeed = 10f; //varies due to the position in the scene
    public float RotationSpeed { get { return rotationSpeed; } set { rotationSpeed = value; } }

    private float secondSpeedFactor = 200f; //constant to all the walls

    private int shouldRotate = 1; //0 means no, 1 means yes

    private Rigidbody rb;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();

        //set max angular velocity
        rb.maxAngularVelocity = rotationSpeed / 10f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //rotate the wall around the y axis
        if(Mathf.RoundToInt(rb.rotation.eulerAngles.y) <= 91 && Mathf.RoundToInt(rb.rotation.eulerAngles.y) >= 89){ //if the wall is at the initial rotation
            //give random chance to start rotating
            int rand = UnityEngine.Random.Range(0, 100);

            if(rand % 50 != 0){
                shouldRotate = 0;
            }
            else{
                shouldRotate = 1;
            }
        }

        rb.angularVelocity = new Vector3(0, rb.maxAngularVelocity * shouldRotate, 0);

    }
}
