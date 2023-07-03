using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ArmMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public float movementSpeed = 250f;
    public float xRotation;
    public float zRotation;
    public float xDirection;
    public float currZValue;
    public int upperLimit = 170;
    public int midPoint = 90;
    public int lowerLimit = 45;

    void Start()
    {
        xRotation = 0;
        zRotation = 0;
        if(tag == "leftArm"){
            xDirection = -1;
        }
        else{
            xDirection = 1;
        }
        currZValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if((Input.GetKey(KeyCode.UpArrow) | Input.GetKey(KeyCode.DownArrow))){

            if(currZValue < upperLimit & currZValue > midPoint & Input.GetKey(KeyCode.UpArrow)){
                return;
            }

            if(currZValue > lowerLimit & currZValue < midPoint & Input.GetKey(KeyCode.DownArrow)){
                return;
            }
            zRotation = Input.GetAxisRaw("VerticalArrow") * Time.deltaTime * movementSpeed;

            xRotation = Input.GetAxisRaw("HorizontalArrow") * Time.deltaTime * movementSpeed * xDirection;
            transform.Rotate(xRotation, 0, zRotation, Space.Self);
            currZValue = Convert.ToInt32(transform.rotation.eulerAngles.z);
        }
    }
}
