using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    // Start is called before the first frame update

    //enums for enemy states
    public enum EnemyStates{
        Idle,
        LookAtPlayer,
        FollowPlayer,
        HitPlayer,
        Dead
    }

    private EnemyStates currentState;
    public EnemyStates CurrentState {get {return currentState;} set {currentState = value;}}

    [SerializeField] private TextMeshProUGUI playerDetectionMessage;

    void Start()
    {
        currentState = EnemyStates.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Current State: " + currentState);
        ProcessPlayerDetectionMessage();
    }

    private void ProcessPlayerDetectionMessage(){
        switch(currentState){
            case EnemyStates.Idle:
                playerDetectionMessage.gameObject.SetActive(false);
                break;
            case EnemyStates.LookAtPlayer:
                playerDetectionMessage.SetText("????");
                playerDetectionMessage.color = Color.yellow;
                playerDetectionMessage.gameObject.SetActive(true);
                break;
            case EnemyStates.FollowPlayer:
                playerDetectionMessage.SetText("!!!!");
                playerDetectionMessage.color = Color.red;
                playerDetectionMessage.gameObject.SetActive(true);
                break;
            case EnemyStates.Dead:
                playerDetectionMessage.gameObject.SetActive(false);
                break;
        }
    }
}
