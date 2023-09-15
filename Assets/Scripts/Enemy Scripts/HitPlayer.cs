using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private LookAtPlayer lookAtPlayer;

    [SerializeField] private FollowPlayer followPlayer;
    private GameObject hittingPlayer;
    public GameObject HittingPlayer {get { return hittingPlayer;} set {hittingPlayer = value;} }

    [SerializeField] private EnemySword enemySword;

    void Start()
    {
        hittingPlayer = null;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hittingPlayer && !followPlayer.Player){  //if enemy is within hit range and not following player
            //give random chance to hit player
            if(!enemySword.IsUsingSword){
                int randomChance = UnityEngine.Random.Range(0, 100);
                if(randomChance % 8 == 0){
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
            hittingPlayer = null;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.name == "Body" && other.transform.parent.tag == "Player"){
            ProcessCanActuallyHit(other.transform.parent.gameObject);
        }
    }

    private void ProcessCanActuallyHit(GameObject gameObject)
    {
        if(lookAtPlayer.Player == gameObject){  //if enemy is looking at player
            hittingPlayer = gameObject; //obviously enemy is in range to hit player            
        }
        else{   //if enemy is not looking at player
            hittingPlayer = null;
            Debug.Log("Not looking at player, so cannot hit");
        }
    }
}
