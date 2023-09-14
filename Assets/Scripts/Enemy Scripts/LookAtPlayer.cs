using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class LookAtPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isLookingAtPlayer;
    public bool IsLookingAtPlayer {get { return isLookingAtPlayer;} set{ isLookingAtPlayer = value;} }
    [SerializeField] EnemyMover enemyMover;
    [SerializeField] private SphereCollider lookingAtPlayerCollider;
    private Transform player;


    void Start()
    {
        isLookingAtPlayer = false;
        player = null;
    }

    private void FixedUpdate(){
        if(isLookingAtPlayer){
            RotateTowardsPlayer();
        }
    }

    private void RotateTowardsPlayer()
    {
        enemyMover.RotateEnemy(player);
    }

    // Update is called once per frame
    private void OnTriggerStay(Collider other) {
        if(other.gameObject.name == "Body" && other.gameObject.transform.parent.tag == "Player"){
            ProcessCanActuallySee(other.gameObject);
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

            Debug.DrawRay(transform.position, worldDirection , Color.red);
            RaycastHit[] hits = Physics.RaycastAll(transform.position, worldDirection, lookingAtPlayerCollider.radius);

            if(hits.Length > 0){
                if(hits[0].transform.gameObject.layer == lookingObject.layer){  //first hit is the player
                    foundPlayer = true;
                    break;
                }
            }
            //didnt find the playey, go to the next direction
            startingXAngle += 0.1f;

        }while(startingXAngle <= 45);


        if(foundPlayer){
            isLookingAtPlayer = true;
            player = lookingObject.transform;
        }        
        else{
            isLookingAtPlayer = false;
            player = null;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.name == "Body" && other.gameObject.transform.parent.tag == "Player"){
            isLookingAtPlayer = false;
            player = null;
        }
    }
}
