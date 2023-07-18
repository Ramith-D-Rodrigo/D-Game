using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rigidbody;
    private bool isCollidedWithObstacle;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        isCollidedWithObstacle = false;
    }

    private void OnCollisionEnter(Collision other) {
        ContactPoint contact = other.contacts[0];
        Vector3 surfaceDirection = contact.normal;

        if(other.gameObject.tag == "slidingObstacle" && !isCollidedWithObstacle && Math.Abs(surfaceDirection.z) == 1){ //only z direction surface
            Destroy(rigidbody);
            //disable all the scripts of the player movement
            this.GetComponent<PlayerMover>().enabled = false;
        
            for(int i = 0; i < transform.childCount; i++){
                Transform child = transform.GetChild(i);
                Rigidbody childRB = null;
                //rotation points of arms and legs
                if(child.gameObject.name == "Right Leg Rotation Point" || 
                child.gameObject.name == "Left Leg Rotation Point" || 
                child.gameObject.name == "Left Arm Rotation Point" || 
                child.gameObject.name == "Right Arm Rotation Point"){

                    //get the cubes
                    childRB = child.GetChild(0).gameObject.AddComponent<Rigidbody>();
                    Debug.Log("attaching rb to " + child.GetChild(0).gameObject.name);
                    
                    if(child.gameObject.name == "Right Leg Rotation Point" || child.gameObject.name == "Left Leg Rotation Point"){ //legs
                        child.GetComponent<LegMovement>().enabled = false;
                    }
                    else{ //arms
                        child.GetComponent<ArmMovement>().enabled = false;
                    }

                }
                else{
                    //any other body part (head, body)
                    childRB = child.gameObject.AddComponent<Rigidbody>();
                    Debug.Log("attaching rb to " + child.gameObject.name);
                }

                childRB.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            }
            isCollidedWithObstacle = true; //make sure only collides once
        }
    }
}
