using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class PlayerPickable : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject pickableObj;
    private PlayerInventory playerInventoryComponent;
    private PlayerHoldObject playerHoldObjectComponent;

    private string[] pickableObjectTags = {"Hammer", "WallBrick"};
    private Hashtable pickableObjTagCollection;

    public GameObject PickableObj { get { return pickableObj; } set { pickableObj = value; }}
    void Start()
    {
        pickableObj = null;
        pickableObjTagCollection = new Hashtable();
        playerInventoryComponent = GetComponent<PlayerInventory>();
        playerHoldObjectComponent = GetComponent<PlayerHoldObject>();

        //adding at start will make the search easier
        foreach(string objTag in pickableObjectTags){
            pickableObjTagCollection.Add(objTag, true);
        }
    }

    private void OnTriggerEnter(Collider other) {
        SetCollidedObj(other.gameObject);
        
    }

    private void OnTriggerExit(Collider other) {
        UnsetCollidedObj(other.gameObject);
    }

/*     private void OnCollisionEnter(Collision other) {
        SetCollidedObj(other.gameObject);
    }

    private void OnCollisionExit(Collision other) {
        UnsetCollidedObj(other.gameObject);
    } */

    private void SetCollidedObj(GameObject obj){
        if(pickableObjTagCollection.Contains(obj.tag)){
            pickableObj = obj;
        }
    }
    private void UnsetCollidedObj(GameObject obj){
        if(pickableObjTagCollection.Contains(obj.tag)){
            pickableObj = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(pickableObj != null & Input.GetKeyDown(KeyCode.F)){
            ProcessPickUp();
        }
    }

    public void ProcessPickUp(){
        Rigidbody rigidbody = pickableObj.GetComponent<Rigidbody>();
        //if doesnt have a rigidbody, can put in inventory
        if(rigidbody == null){
            if(playerInventoryComponent.CanAddMore()){
                AddObjectToInventory();
            }
            else{
                PickUpObject();
            }  
        }
        else{
            if(rigidbody.mass >= this.GetComponent<Rigidbody>().mass / 2){ //if the weight is above the half of player's weight
                //then cannot put in inventory, just pickup
                PickUpObject();

            }
            else{
                if(playerInventoryComponent.CanAddMore()){
                    AddObjectToInventory();
                }  
            }
        }
    }

    private void AddObjectToInventory(){
        playerInventoryComponent.InsertToInventory(pickableObj);
        pickableObj = null;
        playerHoldObjectComponent.CurrHoldingObject = null;
    }

    private void PickUpObject(){
        if(!playerHoldObjectComponent.CurrHoldingObject){//player is not using some object
            //then pick up
            playerHoldObjectComponent.CurrHoldingObject = pickableObj;
            pickableObj = null;
            playerHoldObjectComponent.HoldCurrentObject();
        }    
    }

}
