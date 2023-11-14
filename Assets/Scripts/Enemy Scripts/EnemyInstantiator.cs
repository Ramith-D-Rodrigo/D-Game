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
                EnemyMask enemyMask = enemyChild.GetComponent<EnemyMask>();

                enemyMask.InsertMask(maskPrefabs[i]);

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
