using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class LookAtPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    //private bool isLookingAtPlayer;
    //public bool IsLookingAtPlayer {get { return isLookingAtPlayer;} set{ isLookingAtPlayer = value;} }
    [SerializeField] EnemyMover enemyMover;
    [SerializeField] private SphereCollider lookingAtPlayerCollider;
    private Transform player;
    public Transform Player {get { return player;} }


    void Start()
    {
        //isLookingAtPlayer = false;
        player = null;
    }

    private void FixedUpdate(){
        if(player){
            RotateTowardsPlayer();
        }
        else{
            StopRotatingTowardsPlayer();
        }
    }

    private void StopRotatingTowardsPlayer()
    {
        enemyMover.PlayerTransform = null;
    }

    private void RotateTowardsPlayer()
    {
        enemyMover.PlayerTransform = player;
    }

    // Update is called once per frame
    private void OnTriggerStay(Collider other) {
        if(other.gameObject.name == "Body" && other.transform.parent.tag == "Player"){
            ProcessCanActuallySee(other.transform.parent.gameObject);
        }
    }

    private void ProcessCanActuallySee(GameObject lookingObject){
        //shoot raycast from -45 to 45
        bool foundPlayer = false;

        float startingXAngle = -45;
        do{
            Vector3 shootingDirection = new Vector3(Mathf.Sin(startingXAngle * Mathf.Deg2Rad), 0.0f, Mathf.Cos(startingXAngle * Mathf.Deg2Rad));
            shootingDirection = shootingDirection * lookingAtPlayerCollider.radius;

            Vector3 worldDirection = transform.TransformDirection(shootingDirection);

            Ray ray = new Ray(transform.position, worldDirection.normalized);

            RaycastHit[] hits = Physics.RaycastAll(ray.origin, ray.direction, lookingAtPlayerCollider.radius);

            if(hits.Length > 0){
                if(hits[0].transform.gameObject.layer == lookingObject.layer){  //first hit is the player
                    Debug.DrawRay(transform.position, (hits[0].point - transform.position).normalized * hits[0].distance, Color.green);
                    foundPlayer = true;
                    break;
                }
                else{
                    Debug.DrawRay(transform.position, (hits[0].point - transform.position).normalized * hits[0].distance, Color.red);
                }
            }
            //didnt find the player, go to the next direction
            startingXAngle += 0.1f;

        }while(startingXAngle <= 45);


        if(foundPlayer){
            player = lookingObject.transform;
        }        
        else{
            player = null;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.name == "Body" && other.transform.parent.tag == "Player"){
            player = null;
        }
    }
}
