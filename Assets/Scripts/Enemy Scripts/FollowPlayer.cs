using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private EnemyState enemyState;

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.name == "Body" && other.transform.parent.tag == "Player"){
            ProcessCanActuallyFollowPlayer(other.transform.parent.gameObject);

        }
    }

    private void ProcessCanActuallyFollowPlayer(GameObject followingObject)
    {
        if(enemyState.CurrentState == EnemyState.EnemyStates.LookAtPlayer){ //if enemy is looking at player
            enemyState.CurrentState = EnemyState.EnemyStates.FollowPlayer;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.name == "Body" && other.transform.parent.tag == "Player"){
            enemyState.CurrentState = EnemyState.EnemyStates.LookAtPlayer;
        }
    }

    
}
