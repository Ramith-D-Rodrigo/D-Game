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
    public GameObject HittingPlayer {get { return hittingPlayer;} }
    void Start()
    {
        hittingPlayer = null;
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        if(lookAtPlayer.Player == gameObject.transform){
            if(followPlayer.Player == gameObject.transform){
                hittingPlayer = gameObject;
            }
            else{
                hittingPlayer = null;
            }
            
        }
    }
}
