using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLevel : MonoBehaviour
{
    // Start is called before the first frame update
    private Dictionary<int, Vector3> levelReachCheck;

    [SerializeField] private float nextLevelLoadWaitSeconds = 3.0f;

    void Start()
    {
        IntializeLevelReach();
    }

    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.transform.position.x <= levelReachCheck[1].x){   //reached the level 1 reach point
            StopAllPlayerScripts();
        }
    }

    private void IntializeLevelReach(){
        levelReachCheck = new Dictionary<int, Vector3>
        {
            { 1, new Vector3(-1030, 0, 0) }   //only care about x position
        };

        //TODO: other two level reachpoints
    }

    private void StopAllPlayerScripts(){
        //disable player movement script
        GetComponent<PlayerMover>().enabled = false;
        
        GetComponent<PlayerCollision>().enabled = false;

        GetComponent<PlayerPickable>().enabled = false;
        
        GetComponent<PlayerInventory>().enabled = false;
        //disable player hold object script
        GetComponent<PlayerHoldObject>().enabled = false;

        //disable player use object script
        GetComponentInChildren<PlayerUseObject>().enabled = false;

        GetComponent<MovementRecorder>().enabled = false;

        StartCoroutine(LoadNextLevel());
    }

    private IEnumerator LoadNextLevel(){
        yield return new WaitForSeconds(nextLevelLoadWaitSeconds);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
