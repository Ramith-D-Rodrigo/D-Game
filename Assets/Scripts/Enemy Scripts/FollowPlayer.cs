using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isFollowingPlayer;
    public bool IsFollowingPlayer {get { return isFollowingPlayer;} set{ isFollowingPlayer = value;} }

    [SerializeField] private EnemyMover enemyMover;
    [SerializeField] private LookAtPlayer lookAtPlayer;

    private Transform player;
    void Start()
    {
        isFollowingPlayer = false;
        player = null;
    }

    private void FixedUpdate(){
        if(isFollowingPlayer){
            MoveTowardsPlayer();
        }
        else{
            StopMovingTowardsPlayer();
        }
    }

    private void StopMovingTowardsPlayer()
    {
        enemyMover.StopEnemyMovement();
    }

    private void MoveTowardsPlayer(){ 
        enemyMover.MoveForwardBackward(player);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.name == "Body" && other.gameObject.transform.parent.tag == "Player"){
            ProcessCanActuallyFollowPlayer(other.gameObject);
        }
    }

    private void ProcessCanActuallyFollowPlayer(GameObject followingObject)
    {
        if(lookAtPlayer.IsLookingAtPlayer){
            isFollowingPlayer = true;
            player = followingObject.transform;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.name == "Body" && other.gameObject.transform.parent.tag == "Player"){
            isFollowingPlayer = false;
            player = null;
        }
    }

    
}
