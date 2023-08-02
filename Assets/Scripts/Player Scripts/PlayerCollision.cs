using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCollision : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    private bool isCollidedWithObstacle;
    private bool isCollededWithWall;
    private Vector3 obstacleCollisionPointNormalized;
    private Vector3 wallCollisionPointNormalized;
    private bool isPlayerDead;
    public float playerHealth;
    public float healthReduceFactor = 5f;
    public float healthIncreaseFactor = 10f;
    private bool isOnPath; //check whether player is on the path
    private Vector3 currCenterOfMass;
    private Animator animator;

    public static float Map(float value, float inMin, float inMax, float outMin, float outMax){
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isCollidedWithObstacle = false;
        isCollededWithWall = false;
        isPlayerDead = false;
        playerHealth = 100f;
        isOnPath = true;
        currCenterOfMass = rb.centerOfMass;
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision other) {
        ContactPoint contact = other.GetContact(0);
        Vector3 surfaceDirection = contact.normal;

        //slider collision
        if(other.gameObject.tag == "slidingObstacle" && Math.Abs(surfaceDirection.z) == 1){ //only z direction surface
            isCollidedWithObstacle = true;
            obstacleCollisionPointNormalized = surfaceDirection;
        }

        if(other.gameObject.tag == "wall"){
            isCollededWithWall = true;
            wallCollisionPointNormalized = surfaceDirection;
        }
    }

    private void OnCollisionExit(Collision other) {
        if(other.gameObject.tag == "slidingObstacle"){
            isCollidedWithObstacle = false;
        }

        if(other.gameObject.tag == "wall"){
            isCollededWithWall = false;
        }

        if(other.gameObject.tag == "Path"){
            isOnPath = false;
        }
    }

    private void OnCollisionStay(Collision other) {
        //path
        if(other.gameObject.tag == "Path"){
            isOnPath = true;
        }
    }

    private void Update() {
        //normalized vector check for opposing wall and obstacle surfaces
        if(isCollededWithWall && isCollidedWithObstacle && !isPlayerDead && obstacleCollisionPointNormalized.z != wallCollisionPointNormalized.z){  
            KillPlayer();
        }

        if(!isOnPath && !isPlayerDead){  //not on path and player is still alive
            ReducePlayerHealth(); //reduce the health
        }
        else if(isOnPath && !isPlayerDead){ //on the path and player is still alive
            IncreasePlayerHealth();
        }

        //update the current center of mass
        if(!isPlayerDead && currCenterOfMass.y != rb.centerOfMass.y){ //center of mass has been changed (on y coordinate)
            currCenterOfMass = rb.centerOfMass;
            ReducePlayerHealth();
        }
        else if(!isPlayerDead && currCenterOfMass.z != rb.centerOfMass.z && !isOnPath){
            currCenterOfMass = rb.centerOfMass;
            ReducePlayerHealth();
        }
    }

    private void ReducePlayerHealth(){
        playerHealth -= Time.deltaTime * healthReduceFactor;

        //change player color to reflect the health
        ChangePlayerColor();

        if(playerHealth <= 0f){
            KillPlayer();
        }
    }

    private void IncreasePlayerHealth(){
        if(playerHealth >= 100){ //no need to increase further
            playerHealth = 100f;
            return;
        }
        playerHealth += Time.deltaTime * healthIncreaseFactor;

        //change color of player to show the health 
        ChangePlayerColor();
    }

    private void KillPlayer(){
        Destroy(rb);
        animator.SetBool("isRunning", false);
        animator.SetBool("isWalking", false);

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
            }
            else{
                //any other body part (head, body)
                childRB = child.gameObject.AddComponent<Rigidbody>();
            }

            childRB.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }
        isPlayerDead = true;
    }

    private void ChangePlayerColor(){
        for(int i = 0; i < transform.childCount; i++){
            Transform child = transform.GetChild(i);
            MeshRenderer mr = child.GetComponent<MeshRenderer>();

            if(child.gameObject.name == "Inventory"){ //ignore inventory
                continue;
            }

            if(child.gameObject.name == "Right Leg Rotation Point" || 
            child.gameObject.name == "Left Leg Rotation Point" || 
            child.gameObject.name == "Left Arm Rotation Point" || 
            child.gameObject.name == "Right Arm Rotation Point"){
                mr = child.GetChild(0).GetComponent<MeshRenderer>();
            }
            Color tempCol = mr.materials[1].color;
            mr.materials[1].color = new Color(tempCol.r, tempCol.g, tempCol.b, Map(100 - playerHealth, 0, 100, 0, 255)/255);
        }
    }
}
