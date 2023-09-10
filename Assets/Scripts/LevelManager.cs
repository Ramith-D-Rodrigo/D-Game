using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    private int currLevelIndex;
    public int CurrLevelIndex { get { return currLevelIndex; } }

    [Header("General Attributes")]
    [SerializeField] private GameObject player;
    [SerializeField] private Light directionalLight;
    [SerializeField] private float nextLevelLoadWaitSeconds = 3.0f;
    private Dictionary<int, Vector3> levelReachCheck;

    [SerializeField] private Vector3 levelOneDestination = new(-1030, 0, 0); //only care about x position

    [SerializeField] private Vector3 levelTwoDestination; //care about x and z positions

    [SerializeField] private Vector3 levelThreeDestination;

    void Awake()
    {
        currLevelIndex = SceneManager.GetActiveScene().buildIndex;

        switch(currLevelIndex){
            case 1:
                ProcessLevelOneSettings();
                break;

            case 2:
                ProcessLevelTwoSettings();
                break;

            case 3:
                ProcessLevelThreeSettings();
                break;
        }

        IntializeLevelReach();
    }
    // Update is called once per frame
    void Update()
    {
        switch(currLevelIndex){
            case 1:
                ProcessLevelOneDestination();
                break;

            case 2:
                ProcessLevelTwoDestination();
                break;

            case 3:
                ProcessLevelThreeDestination();
                break;
        }
    }

    private void ProcessLevelThreeDestination()
    {

    }

    private void ProcessLevelTwoDestination()
    {
        if(player.transform.position.x <= levelReachCheck[2].x && player.transform.position.z <= levelReachCheck[2].z){   //reached the level 2 destination
            PlayerScriptHandler playerScriptHandler = player.GetComponent<PlayerScriptHandler>();

            playerScriptHandler.StopAllPlayerScripts();

            //start the coroutine
            StartCoroutine(LoadNextLevel());
        }
    }

    private void ProcessLevelOneDestination()
    {
        if(player.transform.position.x <= levelReachCheck[1].x){   //reached the level 1 reach point
            PlayerScriptHandler playerScriptHandler = player.GetComponent<PlayerScriptHandler>();

            playerScriptHandler.StopAllPlayerScripts();

            //start the coroutine
            StartCoroutine(LoadNextLevel());
        }
    }

    private void ProcessLevelThreeSettings()
    {

    }

    private void ProcessLevelTwoSettings()
    {
        Rigidbody playerRB = player.GetComponent<Rigidbody>();

        playerRB.constraints = RigidbodyConstraints.None;

        //but freeze rotation on all
        playerRB.constraints = RigidbodyConstraints.FreezeRotation;
    }


    private void ProcessLevelOneSettings()
    {
        //player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
    }


    private IEnumerator LoadNextLevel(){
        yield return new WaitForSeconds(nextLevelLoadWaitSeconds);
        SceneManager.LoadScene(currLevelIndex + 1);
    }

    private void IntializeLevelReach(){
        levelReachCheck = new Dictionary<int, Vector3>
        {
            { 1, levelOneDestination },   
            { 2, levelTwoDestination },
            { 3, levelThreeDestination}
        };
    }
}
