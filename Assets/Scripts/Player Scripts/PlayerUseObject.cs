using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class PlayerUseObject : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator playerArmAnimator;
    private bool isUsingObject;
    void Start()
    {
        isUsingObject = false;
        playerArmAnimator = this.GetComponent<Animator>();
    }

    public void UseObject(string parameter){
        if(isUsingObject){
            return;
        }

        isUsingObject = true;
        switch(parameter){
            case "Hammer":
                StartCoroutine(UseHammer());
                break;
            
            case "WallBrick":
                StartUsingWallBrick();
                break;
        }
    }


    IEnumerator UseHammer(){
        playerArmAnimator.SetTrigger("UsingHammer");
        yield return new WaitForSeconds(1f);
        playerArmAnimator.SetTrigger("UsingHammer");
        isUsingObject = false;
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
