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
    public GameObject UsingObject {get {return usingObject;}}

    [Header("Cameras for Level 2")]
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject freeLookCamera;
    [SerializeField] private GameObject firstPersonCamera;

    private int prevUsedSwordAnimation;
    void Start()
    {
        isUsingObject = false;
        playerArmAnimator = this.GetComponent<Animator>();

        prevUsedSwordAnimation = -1;
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

            case "Sword":
                if(isUsingObject){
                    return;
                }
                int animationChoice;

                do{
                    animationChoice = Random.Range(0, 3);
                }
                while(animationChoice == prevUsedSwordAnimation);

                prevUsedSwordAnimation = animationChoice;

                isUsingObject = true;
                switch(animationChoice){
                    case 0:
                        StartCoroutine(UseSword(toBeUsedObject));
                        break;
                    case 1:
                        StartCoroutine(UseSword2(toBeUsedObject));
                        break;
                    case 2:
                        StartCoroutine(UseSword3(toBeUsedObject));
                        break;
                }

                break;
            
            case "EnemyMask":
                EnemyMask enemyMask = toBeUsedObject.GetComponent<EnemyMask>();
                if(!enemyMask.MaskInsertingBox){
                    return;
                }

                enemyMask.InsertMaskToBox();
                break;

        }
    }

    IEnumerator UseHammer(GameObject hammer){
        usingObject = hammer;
        playerArmAnimator.SetTrigger("UsingHammer");

        yield return new WaitForSeconds(1f);

        playerArmAnimator.SetTrigger("UsingHammer");
        StopUsingObject();
    }

    public void StartUsingWallBrick(GameObject wallBrick){
        usingObject = wallBrick;
        playerArmAnimator.SetTrigger("UsingWallBrick"); //no invoke because he holds as he can
    }
    public void StopUsingWallBrick(){
        StopUsingObject();
        playerArmAnimator.SetTrigger("UsingWallBrick");
    }

    public bool GetIsUsingObject(){
        return isUsingObject;
    }

    public void StopUsingObject(){
        isUsingObject = false;
        usingObject = null;
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

    public void StopUsingCompass(){
        usingObject.GetComponent<Compass>().IsWorking = false;
        
        StopUsingObject();
        playerArmAnimator.SetTrigger("UsingCompass");
        SwitchCamera(0);

    }

    IEnumerator UseSword(GameObject sword){
        usingObject = sword;
        playerArmAnimator.SetTrigger("UsingSword");

        yield return new WaitForSeconds(1f/60 * 15);

        playerArmAnimator.SetTrigger("UsingSword");
        StopUsingObject();

    }


    IEnumerator UseSword2(GameObject sword){
        usingObject = sword;
        playerArmAnimator.SetTrigger("UsingSword2");

        yield return new WaitForSeconds(1f/60 * 15);

        playerArmAnimator.SetTrigger("UsingSword2");
        StopUsingObject();

    }

    IEnumerator UseSword3(GameObject sword){
        usingObject = sword;
        playerArmAnimator.SetTrigger("UsingSword3");

        yield return new WaitForSeconds(1f/60 * 15);

        playerArmAnimator.SetTrigger("UsingSword3");
        StopUsingObject();

    }
}
