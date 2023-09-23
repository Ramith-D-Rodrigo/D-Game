using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScriptHandler : MonoBehaviour
{
    public void StopAllPlayerScripts(){
        //disable player movement script
        PlayerMover pm = GetComponent<PlayerMover>();
        pm.StopMovement();
        pm.enabled = false;
        
        GetComponent<PlayerCollision>().enabled = false;

        GetComponent<PlayerPickable>().enabled = false;
        
        GetComponent<PlayerInventory>().enabled = false;
        //disable player hold object script
        GetComponent<PlayerHoldObject>().enabled = false;

        //disable player use object script
        GetComponentInChildren<PlayerUseObject>().enabled = false;

        GetComponent<MovementRecorder>().enabled = false;
    }
}
