using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class PlayerUseObject : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator playerArmAnimator;
    private bool isUsingObject;
    private GameObject usingObject;

    [Header("Cameras for Level 2")]
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject freeLookCamera;
    [SerializeField] private GameObject firstPersonCamera;
    void Start()
    {
        isUsingObject = false;
        playerArmAnimator = this.GetComponent<Animator>();
    }

    public void UseObject(GameObject toBeUsedObject){
        switch(toBeUsedObject.tag){
            case "Hammer":
                if(isUsingObject){
                    return;
                }
                isUsingObject = true;
                StartCoroutine(UseHammer(toBeUsedObject));
                break;
            
            case "WallBrick":
                if(isUsingObject){
                    return;
                }
                isUsingObject = true;
                StartUsingWallBrick(toBeUsedObject);
                break;
            
            case "Compass":
                ProcessUsingCompass(toBeUsedObject); //because compass is used and not used with one key
                break;
        }
    }

    IEnumerator UseHammer(GameObject hammer){
        usingObject = hammer;
        playerArmAnimator.SetTrigger("UsingHammer");

        yield return new WaitForSeconds(1f);

        playerArmAnimator.SetTrigger("UsingHammer");
        usingObject = null;
        isUsingObject = false;
    }

    public void StartUsingWallBrick(GameObject wallBrick){
        usingObject = wallBrick;
        playerArmAnimator.SetTrigger("UsingWallBrick"); //no invoke because he holds as he can
    }
    public void StopUsingWallBrick(){
        isUsingObject = false;
        usingObject = null;
        playerArmAnimator.SetTrigger("UsingWallBrick");
    }

    public bool GetIsUsingObject(){
        return isUsingObject;
    }



    //level 2 functions
    private void SwitchCamera(int cameraSelection){
        //cameraSelection = 0 -> main camera
        //cameraSelection = 1 -> first person camera

        if(cameraSelection == 0){
            firstPersonCamera.SetActive(false);
            Debug.Log("switching to main");

            mainCamera.SetActive(true);
            freeLookCamera.SetActive(true);
        }
        else{
            mainCamera.SetActive(false);
            freeLookCamera.SetActive(false);

            Debug.Log("switching to first person");

            firstPersonCamera.SetActive(true);
        }
    }

    private void ProcessUsingCompass(GameObject compass){
        if(usingObject == null){   //not using it yet, that means starting to use
            isUsingObject = true;
            StartUsingCompass(compass);
        }
        else{
            StopUsingCompass();
        }
    }

    private void StartUsingCompass(GameObject compass){
        usingObject = compass;
        playerArmAnimator.SetTrigger("UsingCompass");       
        SwitchCamera(1);

        compass.GetComponent<Compass>().IsWorking = true;
    }

    private void StopUsingCompass(){
        usingObject.GetComponent<Compass>().IsWorking = false;
        
        usingObject = null;
        isUsingObject = false;
        playerArmAnimator.SetTrigger("UsingCompass");
        SwitchCamera(0);

    }
}
