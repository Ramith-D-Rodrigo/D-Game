using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class PlayerUseObject : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator playerArmAnimator;
    private bool isUsingObject;
    void Start()
    {
        isUsingObject = false;
        playerArmAnimator = this.GetComponent<Animator>();
    }

    public void UseObject(string parameter){
        isUsingObject = true;
        Invoke("Start" + parameter, 0);
    }

    public void StartUsingHammer(){
        playerArmAnimator.SetTrigger("UsingHammer");
        //animation runs for 1 second then stops
        Invoke("StopUsingHammer", 1);
    }

    public void StopUsingHammer(){
        isUsingObject = false;
        playerArmAnimator.SetTrigger("UsingHammer");
    }

    public void StartUsingWallBrick(){
        playerArmAnimator.SetTrigger("UsingWallBrick"); //no invoke because he holds as he can
    }
    public void StopUsingWallBrick(){
        isUsingObject = false;
        playerArmAnimator.SetTrigger("UsingWallBrick");
    }

    public bool GetIsUsingObject(){
        return isUsingObject;
    }
}
