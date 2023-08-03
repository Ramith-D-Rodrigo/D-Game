using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingUpWallBox : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator parentWallAnimator;
    public float neededMassToOpen = 6f;
    private float currMass;
    private bool isClosed;
    void Start()
    {
        currMass = 0f;
        isClosed = true;
        parentWallAnimator = this.transform.parent.GetChild(0).GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision other){
        //get the rigidBody
        Rigidbody rigidBody = other.gameObject.GetComponent<Rigidbody>();
        if(!rigidBody){
            return;
        }
        currMass += rigidBody.mass;
        ProcessCurrMass();
    }

    private void OnCollisionExit(Collision other){
        Rigidbody rigidBody = other.gameObject.GetComponent<Rigidbody>();
        if(!rigidBody){
            return;
        }
        currMass -= rigidBody.mass;
        ProcessCurrMass();
    }

    private void ProcessCurrMass(){
        Debug.Log("processing");
        if(currMass >= neededMassToOpen && isClosed){
            SlideDownWall();
        }
        else if(currMass < neededMassToOpen && !isClosed){
            SlideUpWall();
        }
    }

    private void SlideUpWall()
    {
        parentWallAnimator.SetTrigger("SlideUp");
        isClosed = true;
    }

    private void SlideDownWall()
    {
        parentWallAnimator.SetTrigger("SlideDown");
        isClosed = false;
    }
}
