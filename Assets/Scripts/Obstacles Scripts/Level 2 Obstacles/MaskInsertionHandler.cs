using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskInsertionHandler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private SpikyWoodenLogs spikyWoodenLogs;
    private HUD hud;

    void Start()
    {
        //find the hud component
        hud = GameObject.FindObjectOfType<HUD>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    private void OnTriggerEnter(Collider other)
    {
        ProcessPlayerCollision(other);
    }

    private void OnTriggerStay(Collider other) {
        ProcessPlayerCollision(other);
    }


    private void ProcessPlayerCollision(Collider other){
        if (1 << other.gameObject.layer == playerLayer)
        {
            //get the top most parent of the collided object
            GameObject collidedObj = other.gameObject.transform.root.gameObject; //top most is the player complete game object

            PlayerHoldObject playerHoldObject = collidedObj.GetComponent<PlayerHoldObject>(); //holding object should be the mask placeholder (inside it has the mask)

            if (!playerHoldObject.CurrHoldingObject)
            {
                return;
            }

            if (playerHoldObject.CurrHoldingObject.tag == "EnemyMask")
            {
                Debug.Log("Player has a mask");

                EnemyMask enemyMask = playerHoldObject.CurrHoldingObject.GetComponent<EnemyMask>();
                if (enemyMask.MaskIndex == spikyWoodenLogs.MaskIndex)
                {
                    Debug.Log("Player has the correct mask");
                    hud.DisplayGeneralMessage("Press " + Controls.UseObj.ToString() + " to Insert the Mask");
                }
            }
        }
    }
}
