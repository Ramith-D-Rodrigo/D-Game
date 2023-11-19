using System;
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

    private string[] pickableObjectTags = {"Hammer", "WallBrick", "Compass", "Sword", "EnemyMask"};
    private Hashtable pickableObjTagCollection;

    public GameObject PickableObj { get { return pickableObj; } set { pickableObj = value; }}

    [SerializeField] private HUD hud;
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
        if(pickableObjTagCollection.Contains(obj.tag) && !playerHoldObjectComponent.CurrHoldingObject){
            pickableObj = obj;

            hud.DisplayPickUpMessage(obj.tag);
        }
    }
    private void UnsetCollidedObj(GameObject obj){
        if(pickableObjTagCollection.Contains(obj.tag)){
            pickableObj = null;

            hud.HideMessage();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(pickableObj != null & Input.GetKeyDown(Controls.PickUpObj)){
            ProcessPickUp();
        }
    }

    public void ProcessPickUp(){
        Rigidbody rigidbody = pickableObj.GetComponent<Rigidbody>();
        //if doesnt have a rigidbody, can put in inventory
        if(rigidbody == null){
            ProcessPickableSettings();  //process the settings of the pickable object (changing colliders, enabling/disabling scripts)
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

    private void ProcessPickableSettings(){
        pickableObj.transform.SetParent(null);
        switch(pickableObj.tag){
            case "EnemyMask":
                EnemyMaskSettings(pickableObj);
                break;

            case "Sword":
                SwordSettings(pickableObj);
                break;

            case "Compass":
                CompassSettings(pickableObj);
                break;

            case "Hammer":
                HammerSettings(pickableObj);
                break;

            case "WallBrick":
                break;
        }
    }

    private void AddObjectToInventory(){
        playerInventoryComponent.InsertToInventory(pickableObj);
        pickableObj = null;
        hud.HideMessage();
        playerHoldObjectComponent.CurrHoldingObject = null;
    }

    private void PickUpObject(){
        if(!playerHoldObjectComponent.CurrHoldingObject){//player is not using some object
            //then pick up
            playerHoldObjectComponent.CurrHoldingObject = pickableObj;
            pickableObj = null;
            hud.HideMessage();
            playerHoldObjectComponent.HoldCurrentObject();
        }    
    }

    
    private void EnemyMaskSettings(GameObject gameObject)
    {
        //change to default layer
        gameObject.layer = 0;
        gameObject.GetComponent<SphereCollider>().enabled = false;
    }

    private void SwordSettings(GameObject gameObject)
    {
        gameObject.GetComponent<SphereCollider>().enabled = false;

        GameObject swordMesh = gameObject.transform.GetChild(0).gameObject; //1st child is the sword mesh
        swordMesh.GetComponent<BoxCollider>().enabled = true; //longer one
        swordMesh.transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;   //short horizontal one

        PlayerSword playerSword = swordMesh.GetComponent<PlayerSword>();
        playerSword.enabled = true;
        playerSword.SwitchOffLight();
    }

    private void CompassSettings(GameObject gameObject)
    {
        gameObject.GetComponent<SphereCollider>().enabled = false;
        Compass compass = gameObject.GetComponent<Compass>();
        compass.SwitchOffCanvas();
        compass.SwitchOffLight();
    }

    private void HammerSettings(GameObject gameObject){
        //remove the sphere collider and give the mesh collider
        gameObject.GetComponent<SphereCollider>().enabled = false;
        gameObject.GetComponent<MeshCollider>().enabled = true;
    }

}
