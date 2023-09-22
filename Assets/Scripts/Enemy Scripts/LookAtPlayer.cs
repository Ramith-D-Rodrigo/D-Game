using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder;

public class LookAtPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    //private bool isLookingAtPlayer;
    //public bool IsLookingAtPlayer {get { return isLookingAtPlayer;} set{ isLookingAtPlayer = value;} }
    [SerializeField] EnemyMover enemyMover;
    [SerializeField] private SphereCollider lookingAtPlayerCollider;

    [SerializeField] private EnemyState enemyState;

    // Update is called once per frame
    private void OnTriggerStay(Collider other) {
        if(other.gameObject.name == "Body" && other.transform.parent.tag == "Player"){

            if(ProcessCanActuallySee(other.transform.parent.gameObject)){   //can actually see the player
                if(enemyState.CurrentState == EnemyState.EnemyStates.Idle){ //if enemy is idle
                    enemyState.CurrentState = EnemyState.EnemyStates.LookAtPlayer;  //change to look at player state
                }
                enemyMover.TargettingPlayer = other.transform.parent.gameObject;    //set the targetting player to look at
            }
            else{   //cannot see the player
                enemyState.CurrentState = EnemyState.EnemyStates.Idle;  //change to idle state
                enemyMover.TargettingPlayer = null; //set the targetting player to null
            }
        }
    }

    private bool ProcessCanActuallySee(GameObject lookingObject){
        //shoot raycast from -60 to 30 degrees
        bool foundPlayer = false;

        if(lookingObject.GetComponent<PlayerCollision>().IsPlayerDead){
            return false;
        }

        float startingXAngle = -60;
        do{
            Vector3 shootingDirection = new Vector3(Mathf.Sin(startingXAngle * Mathf.Deg2Rad), 0.0f, Mathf.Cos(startingXAngle * Mathf.Deg2Rad));
            shootingDirection = shootingDirection * lookingAtPlayerCollider.radius;

            Vector3 worldDirection = transform.TransformDirection(shootingDirection);

            Ray ray = new Ray(transform.position, worldDirection.normalized);

            //get the layer mask of the enemy to ignore
            int layerMask = 1 << this.gameObject.layer; //(bitwise operator)
            layerMask = ~layerMask;

            RaycastHit[] hits = Physics.RaycastAll(ray.origin, ray.direction, lookingAtPlayerCollider.radius, layerMask); //ignore own layer

            if(hits.Length > 0){

                //sort the hits by distance
                Array.Sort(hits, delegate(RaycastHit hit1, RaycastHit hit2){    //thank god this worked, i wasted more than 2 hours on this ffs
                    return hit1.distance.CompareTo(hit2.distance);
                });

                if(hits[0].transform.gameObject.layer == lookingObject.layer){  //first hit is the player
                    //Debug.DrawRay(transform.position, (hits[0].point - transform.position).normalized * hits[0].distance, Color.green);
                    Debug.DrawRay(ray.origin, ray.direction * hits[0].distance, Color.green);
                    foundPlayer = true;
                    break;
                }
                else{
                    //Debug.DrawRay(transform.position, (hits[0].point - transform.position).normalized * hits[0].distance, Color.red);
                    Debug.DrawRay(ray.origin, ray.direction * hits[0].distance, Color.red);
                }
            }
            else{
                Debug.DrawRay(ray.origin, ray.direction * lookingAtPlayerCollider.radius, Color.blue);
            }
            //didnt find the player, go to the next direction
            startingXAngle += 0.1f;

        }while(startingXAngle <= 30);


        return foundPlayer;
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.name == "Body" && other.transform.parent.tag == "Player"){
            //change to idle state
            enemyState.CurrentState = EnemyState.EnemyStates.Idle;
        }
    }
}
