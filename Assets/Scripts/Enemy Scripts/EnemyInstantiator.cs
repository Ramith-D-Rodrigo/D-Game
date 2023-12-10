using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInstantiator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject[] maskPrefabs;

    //an hash map to check how much masks instantiated for a particular mask prefab
    private List<GameObject> enemiesWithMask;

    void Awake()
    {
        //find the level manager
        LevelManager levelManager = GameObject.FindObjectOfType<LevelManager>();

        maskPrefabs = levelManager.EnemyMaskPrefabs;

        InsertMasksToEnemies();
    }


    private void InsertMasksToEnemies()
    {
        int initialChildCount = this.transform.childCount;
        Transform[] children = getDirectChildren();

        //shuffle the children for randomization
        Shuffle(children);

        int maskIndex = 0;

        for(int i = 0; i < children.Length; i++){
            GameObject enemyChild = children[i].gameObject;

            //find the mask placeholder in the enemy
            Transform maskPlaceHolder = enemyChild.GetComponent<EnemyCollision>().EnemyMaskPlaceholder.transform;

            GameObject mask = Instantiate(maskPrefabs[maskIndex], maskPlaceHolder.position, maskPlaceHolder.rotation, maskPlaceHolder);

            EnemyMask enemyMask = maskPlaceHolder.gameObject.AddComponent<EnemyMask>(); //add the enemy mask script to the mask placeholder (to identify the mask prefab)
            DroppingObject droppingObj = maskPlaceHolder.gameObject.AddComponent<DroppingObject>(); //add the dropping object script to the mask placeholder (to drop the mask when player drops it)
            droppingObj.enabled = false;   //disable the dropping object script

            enemyMask.MaskIndex = maskIndex;    //to identify the mask prefab

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
}
