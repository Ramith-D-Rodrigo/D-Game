using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    //private bool isFollowingPlayer;
    //public bool IsFollowingPlayer {get { return isFollowingPlayer;} set{ isFollowingPlayer = value;} }

    [SerializeField] private EnemyMover enemyMover;
    [SerializeField] private LookAtPlayer lookAtPlayer;
    [SerializeField] private HitPlayer hitPlayer;

    private Transform player;
    public Transform Player {get { return player;} }

    void Start()
    {
        player = null;
    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.name == "Body" && other.transform.parent.tag == "Player"){
            ProcessCanActuallyFollowPlayer(other.transform.parent.gameObject);
        }
    }

    private void ProcessCanActuallyFollowPlayer(GameObject followingObject)
    {
        if(lookAtPlayer.Player == followingObject.transform){ //if enemy is looking at player
            //now we check whether the enemy is within hit range
            if(hitPlayer.HittingPlayer == followingObject){
                //if enemy is within hit range, then stop following
                player = null;
                Debug.Log("Within hit range");
            }
            else{
                //if enemy is not within hit range, then follow
                player = followingObject.transform;
            }
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