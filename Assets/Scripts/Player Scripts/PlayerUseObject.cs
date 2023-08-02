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
        playerArmAnimator.SetTrigger(parameter);
        //animation runs for 1 second then stops
        Invoke("Stop" + parameter, 1);
    }

    public void StopUsingHammer(){
        isUsingObject = false;
        playerArmAnimator.SetTrigger("UsingHammer");
    }

    public bool getIsUsingObject(){
        return isUsingObject;
    }


}
