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
    }
    // Update is called once per frame
    void Update()
    {

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
}
