using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskInsertionHandler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private SpikyWoodenLogs spikyWoodenLogs;
    private HUD hud;
    private GameObject collidedPlayer;
    private EnemyMask enemyMask;  //if the collided player has an enemy mask, it will be stored here 

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

    private void OnTriggerExit(Collider other) {
        if(1 << other.gameObject.layer == playerLayer){
            hud.HideMessage();
            collidedPlayer = null;

            if(enemyMask){
                enemyMask.MaskInsertingBox = null;
                enemyMask = null;
            }
        }
    }


    private void ProcessPlayerCollision(Collider other){
        if (1 << other.gameObject.layer == playerLayer)
        {
            //get the top most parent of the collided object
            collidedPlayer = other.gameObject.transform.root.gameObject; //top most is the player complete game object

            PlayerHoldObject playerHoldObject = collidedPlayer.GetComponent<PlayerHoldObject>(); //holding object should be the mask placeholder (inside it has the mask)

            if (!playerHoldObject.CurrHoldingObject)
            {
                return;
            }

            if (playerHoldObject.CurrHoldingObject.tag == "EnemyMask")
            {
                Debug.Log("Player has a mask");

                enemyMask = playerHoldObject.CurrHoldingObject.GetComponent<EnemyMask>();
                if (enemyMask.MaskIndex == spikyWoodenLogs.MaskIndex)
                {
                    hud.DisplayGeneralMessage("Press " + Controls.UseObj.ToString() + " to Insert the Mask");
                    Debug.Log("Player has the correct mask");
                    enemyMask.MaskInsertingBox = this.gameObject;
                }
                else{
                    Debug.Log("Player has the wrong mask");
                }
            }
            else{
                Debug.Log("Player has no mask");
            }
            
        }
    }

    public void InsertMask(GameObject mask, int maskIndex){
        if(maskIndex != spikyWoodenLogs.MaskIndex){
            return;
        }

        //hide the message
        hud.HideMessage();

        //remove the mask from the player
        collidedPlayer.GetComponent<PlayerHoldObject>().CurrHoldingObject = null;
        enemyMask.MaskInsertingBox = null;
        enemyMask = null;
        collidedPlayer = null;

        //destroy the mask
        Destroy(mask);

        //play the animation
        Debug.Log("Inserting the mask");

        Renderer maskRenderer = spikyWoodenLogs.InstantiatedMask.GetComponent<Renderer>();

        StartCoroutine(ChangeColorAndMoveTheLogs(maskRenderer));
    }

    private IEnumerator ChangeColorAndMoveTheLogs(Renderer renderer)
    { //direction true -> increase blue, direction false -> decrease blue
        yield return ChangeColor(renderer);

       MoveTheLogs(renderer);
    }

    private void MoveTheLogs(Renderer renderer){
        //remove the material and add the final material
        renderer.material = null;
        renderer.material = spikyWoodenLogs.FinalMaskMaterial;

        //move the wooden logs
        spikyWoodenLogs.CanMove = true;
    }

    private IEnumerator ChangeColor(Renderer renderer)
    {
        yield return ChangeToBlue(renderer);

        yield return ChangeBackFromBlue(renderer);

        yield return ChangeToBlue(renderer);
    }

    private IEnumerator ChangeToBlue(Renderer renderer){
        for(int i = 0; i <= 200; i++){
            Color tempColor = renderer.material.color;
            Color newColor = new Color(tempColor.r, tempColor.g, i/255.0f, tempColor.a);
            renderer.material.color = newColor;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator ChangeBackFromBlue(Renderer renderer){
        for(int i = 200; i >= 0; i--){
            Color tempColor = renderer.material.color;
            Color newColor = new Color(tempColor.r, tempColor.g, i/255.0f, tempColor.a);
            renderer.material.color = newColor;
            yield return new WaitForEndOfFrame();
        }
    }
}
