using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInstantiator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject[] maskPrefabs;

    //an hash map to check how much masks instantiated for a particular mask prefab
    private Hashtable maskCount;
    private List<GameObject> enemiesWithMask;

    void Awake()
    {
        //find the level manager
        LevelManager levelManager = GameObject.FindObjectOfType<LevelManager>();

        maskPrefabs = levelManager.EnemyMaskPrefabs;

        InitializeTable();

        InsertMasksToEnemies();
    }


    private void InsertMasksToEnemies()
    {
        int initialChildCount = this.transform.childCount;

        enemiesWithMask = new List<GameObject>();

        for(int i = 0; i < maskPrefabs.Length; i++){
            int randChildIndex;

            if(enemiesWithMask.Count == initialChildCount){ //if all the enemies have a mask
                break;
            }

            while((int) maskCount[i] <= Mathf.CeilToInt((float)initialChildCount / (float)maskPrefabs.Length)){
                if(enemiesWithMask.Count == initialChildCount){ //if all the enemies have a mask
                    break;
                }
                //the while loop checks if all the masks are equally distributed among the enemies (some masks may have higher count than others due to randomization)
                randChildIndex = Random.Range(0, this.transform.childCount);

                GameObject enemyChild = this.transform.GetChild(randChildIndex).gameObject;

                //find the mask placeholder in the enemy
                Transform maskPlaceHolder = enemyChild.GetComponent<EnemyCollision>().EnemyMaskPlaceholder.transform;

                GameObject mask = Instantiate(maskPrefabs[i], maskPlaceHolder.position, maskPlaceHolder.rotation, maskPlaceHolder);

                EnemyMask enemyMask = maskPlaceHolder.gameObject.AddComponent<EnemyMask>(); //add the enemy mask script to the mask placeholder (to identify the mask prefab)
                DroppingObject droppingObj = maskPlaceHolder.gameObject.AddComponent<DroppingObject>(); //add the dropping object script to the mask placeholder (to drop the mask when player drops it)
                droppingObj.enabled = false;   //disable the dropping object script

                enemyMask.MaskIndex = i;    //to identify the mask prefab

                maskCount[i] = (int)maskCount[i] + 1;

                enemiesWithMask.Add(enemyChild);

                //remove the child from parent
                enemyChild.transform.SetParent(null);
            }
        }

        //re add the enemies to the parent
        foreach(GameObject enemy in enemiesWithMask){
            enemy.transform.SetParent(this.transform);
        }
    }
    private void InitializeTable(){
        maskCount = new Hashtable();

        for(int i = 0; i < maskPrefabs.Length; i++){
            maskCount.Add(i, 0);
        }
    }
}
