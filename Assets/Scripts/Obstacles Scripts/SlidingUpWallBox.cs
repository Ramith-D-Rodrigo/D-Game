using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingUpWallBox : MonoBehaviour
{
    // Start is called before the first frame update
    public float neededMassToOpen = 6f;
    private float currMass;
    private bool massFullFilled;

    public bool MassFullFilled { get { return massFullFilled; } }
    void Start()
    {
        currMass = 0f;
        massFullFilled = false;
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
        if(currMass >= neededMassToOpen){
            massFullFilled = true;
        }
        else if(currMass < neededMassToOpen){
            massFullFilled = false;
        }
    }
}
