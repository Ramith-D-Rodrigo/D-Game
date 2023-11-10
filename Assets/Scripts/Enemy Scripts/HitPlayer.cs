using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private LookAtPlayer lookAtPlayer;

    [SerializeField] private FollowPlayer followPlayer;

    [SerializeField] private EnemyCollision enemyCollision;
    private GameObject hittingPlayer;
    public GameObject HittingPlayer {get { return hittingPlayer;} set {hittingPlayer = value;} }

    [SerializeField] private EnemySword enemySword;

    [SerializeField] private EnemyState enemyState;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyState.CurrentState == EnemyState.EnemyStates.HitPlayer){  //if enemy is within hit range and not following player
            //give random chance to hit player
            if(!enemySword.IsUsingSword){
                int randomChance = UnityEngine.Random.Range(0, 1000);
                if(randomChance % enemyCollision.Difficulty == 0){
                    StartCoroutine(enemySword.UseSword());
                }
            }
        } 

    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.name == "Body" && other.transform.parent.tag == "Player"){
            ProcessCanActuallyHit(other.transform.parent.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.name == "Body" && other.transform.parent.tag == "Player"){
            enemyState.CurrentState = EnemyState.EnemyStates.FollowPlayer;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.name == "Body" && other.transform.parent.tag == "Player"){
            ProcessCanActuallyHit(other.transform.parent.gameObject);
        }
    }

    private void ProcessCanActuallyHit(GameObject gameObject)
    {
        if(enemyState.CurrentState == EnemyState.EnemyStates.FollowPlayer){  //if enemy is looking at player
            hittingPlayer = gameObject; //obviously enemy is in range to hit player
            enemyState.CurrentState = EnemyState.EnemyStates.HitPlayer;            
        }
    }
}
