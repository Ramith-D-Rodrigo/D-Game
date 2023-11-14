using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    private int currLevelIndex;
    public int CurrLevelIndex { get { return currLevelIndex; } }

    [Header("General Attributes")]
    [SerializeField] private GameObject player;

    [SerializeField] private float nextLevelLoadWaitSeconds = 3.0f;
    [SerializeField] private float currLevelDisplayTextSeconds = 4.0f;
    private Dictionary<int, Vector3> levelReachCheck;

    [Header("Level Destinations")]
    [SerializeField] private Vector3 levelOneDestination = new(-1030, 0, 0); //only care about x position
    [SerializeField] private Vector3 levelTwoDestination; //care about x and z positions
    [SerializeField] private Vector3 levelThreeDestination;

    [Header("Level 2 Attributes")]
    [SerializeField] private Light directionalLight;

    [SerializeField] private GameObject[] enemyMaskPrefabs;
    public GameObject[] EnemyMaskPrefabs { get { return enemyMaskPrefabs; } }

    [Header("Level UI Panels")]
    [SerializeField] private TextMeshProUGUI levelNumberText;
    [SerializeField] private GameObject levelClearPanel;

    void Awake()
    {
        currLevelIndex = SceneManager.GetActiveScene().buildIndex;
        levelClearPanel.SetActive(false);
        levelNumberText.transform.parent.gameObject.SetActive(false);

        //hide mouse cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

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

        StartCoroutine(DisplayLevel());
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

        //health reduce factor is 3 and increase factor is 15
        PlayerCollision playerCollision = player.GetComponent<PlayerCollision>();
        playerCollision.HealthReduceFactor = 3;
        playerCollision.HealthIncreaseFactor = 15;
    }


    private void ProcessLevelOneSettings()
    {
        //player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;

        //health reduce factor is 5 and increase factor is 10
        PlayerCollision playerCollision = player.GetComponent<PlayerCollision>();

        playerCollision.HealthReduceFactor = 5;
        playerCollision.HealthIncreaseFactor = 10;
    }


    private IEnumerator LoadNextLevel(){
        ActivateLevelClearMessage();
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

    private void ActivateLevelClearMessage(){
        levelClearPanel.SetActive(true);
    }

    private IEnumerator DisplayLevel(){
        levelNumberText.SetText(currLevelIndex.ToString());

        levelNumberText.transform.parent.gameObject.SetActive(true);

        yield return new WaitForSeconds(currLevelDisplayTextSeconds);

        levelNumberText.transform.parent.gameObject.SetActive(false);
    }

    public void RestartLevel(){
        //if time scale is 0, set it to 1
        if(Time.timeScale == 0.0f){
            Time.timeScale = 1.0f;
        }
        SceneManager.LoadScene(currLevelIndex);
    }
}
