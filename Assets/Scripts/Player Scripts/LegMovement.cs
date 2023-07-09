using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LegMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float movementSpeed = 250f;
    public int upperLimit = 315;
    public int lowerLimit = 45;
    public float zRotation;
    public int currZValue;
    private int direction; //with respect to left leg when pressed W

    void Start()
    {
        zRotation = 0;
        currZValue = 0; 
        if(tag == "leftLeg"){
            direction = -1;
        } 
        else{
            direction = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W) | Input.GetKey(KeyCode.S)){ //key is pressed
            MoveLegs();
        }
        else{ //key is not pressed
            LegReset();
        }
    }

    void MoveLegs(){
        if(currZValue < upperLimit & currZValue > 180){ //limit reached (between 315 and 180)
            //reversing the direction
            direction = 1;
            transform.rotation.eulerAngles.Set(0, 0, upperLimit);
        }
        else if(currZValue < 180 & currZValue > lowerLimit){ //limit reached (between 0 and 45)
            //reversing the direction
            direction = -1;
            transform.rotation.eulerAngles.Set(0, 0, lowerLimit);
        }

        zRotation = Math.Abs(Input.GetAxisRaw("Vertical")) * Time.deltaTime * movementSpeed * direction; //get the movement

        if(Input.GetKey(KeyCode.LeftShift) & Input.GetAxis("Vertical") > 0){ //speed up when shift pressed
            zRotation *= 1.5f;
        }
        transform.Rotate(0, 0, zRotation);
        currZValue = Convert.ToInt32(transform.rotation.eulerAngles.z); //updated z value
    }

    void LegReset(){
        float temp = transform.rotation.eulerAngles.z;
        if(currZValue < 360 & currZValue > 180){ //legs infront
            LegFallBack(temp, Vector3.forward);
        }
        else if(currZValue > 0 & currZValue < 180){ //legs in back
            LegFallBack(temp, Vector3.back);
        }

        //make sure to check the precision
        if(currZValue == 0 | currZValue == 360 | temp < 360 & temp > 358 | temp > 0 & temp < 2){
            Start(); //back to initial values
        }
    }

    void LegFallBack(float maxZPoint, Vector3 vector){
        transform.rotation.eulerAngles.Set(0, 0, Convert.ToInt32(maxZPoint));
        transform.Rotate(vector); //bring to back
        currZValue = Convert.ToInt32(transform.rotation.eulerAngles.z);
    }
}
