using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingWall : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f;
    public float RotationSpeed { get { return rotationSpeed; } set { rotationSpeed = value; } }

    private float secondSpeedFactor = 7f;

    private Rigidbody rb;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //rotate the wall around the y axis
        if(Mathf.RoundToInt(rb.rotation.eulerAngles.y) <= 91 && Mathf.RoundToInt(rb.rotation.eulerAngles.y) >= 89){ //if the wall is at the initial rotation
            //give random chance to start rotating
            int rand = UnityEngine.Random.Range(0, 100);

            if(rand % 50 != 0){
                secondSpeedFactor = 0f;
            }
            else{
                secondSpeedFactor = 7f;
            }
        }

        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, rotationSpeed * Time.fixedDeltaTime * secondSpeedFactor, 0));
    }
}
