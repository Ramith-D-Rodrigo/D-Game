using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikyWoodenLogsInstantiator : MonoBehaviour
{
    private GameObject[] maskPrefabs;
    private void Awake(){
        //get the level manager
        LevelManager levelManager = GameObject.FindObjectOfType<LevelManager>();

        maskPrefabs = levelManager.EnemyMaskPrefabs;

        //get direct children of this game object
        Transform[] children = getDirectChildren();

        //shuffle the children for randomization
        Shuffle(children);

        InsertMasksToSpikyWoodenLogs(children);
    }

    private void InsertMasksToSpikyWoodenLogs(Transform[] children)
    {
        int maskIndex = 0;

        for(int i = 0; i < children.Length; i++){
            SpikyWoodenLogs spikyWoodenLogs = children[i].gameObject.GetComponent<SpikyWoodenLogs>();
            spikyWoodenLogs.InsertMask(maskPrefabs[maskIndex]);
            maskIndex = (maskIndex + 1) % maskPrefabs.Length;
        }
    }

    private void Shuffle(Transform[] children)
    {
        for(int i = 0; i < children.Length; i++){
            int randIndex = UnityEngine.Random.Range(0, children.Length);

            Transform temp = children[randIndex];
            children[randIndex] = children[i];
            children[i] = temp;
        }
    }

    private Transform[] getDirectChildren()
    {
        Transform[] children = new Transform[this.transform.childCount];

        for(int i = 0; i < this.transform.childCount; i++){
            children[i] = this.transform.GetChild(i);
        }

        return children;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
