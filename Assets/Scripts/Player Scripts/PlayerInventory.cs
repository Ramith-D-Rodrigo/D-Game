using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PlayerInventory : MonoBehaviour
{
    private GameObject[] inventory;
    private readonly int inventoryObjRotation = -45;
    private PlayerHoldObject playerHoldObject;

    public PlayerUseObject playerUseObject;
    public GameObject inventoryGameObject;

    public GameObject[] Inventory { get { return inventory; } }

    private void Start(){
        inventory = new GameObject[3]; //only three in the inventory
        playerHoldObject = GetComponent<PlayerHoldObject>();
    }
    public GameObject[] GetInventory(){
        return inventory;
    }

    private void Update(){

        //not using any object
        if(!playerUseObject.GetIsUsingObject()){
            if(Input.GetKeyDown(KeyCode.Alpha1)){   //first object in the inventory
                if(playerHoldObject.CurrHoldingObject == null){
                    playerHoldObject.CurrHoldingObject = inventory[0];
                    inventory[0] = null;    //not in the inventory anymore
                    playerHoldObject.HoldCurrentObject();
                }
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2)){  //second object in the inventory
                if(playerHoldObject.CurrHoldingObject == null){
                    playerHoldObject.CurrHoldingObject = inventory[1];
                    inventory[1] = null;    //not in the inventory anymore
                    playerHoldObject.HoldCurrentObject();
                }
            }
            else if(Input.GetKeyDown(KeyCode.Alpha3)){  //third object in the inventory
                if(playerHoldObject.CurrHoldingObject == null){
                    playerHoldObject.CurrHoldingObject = inventory[2];
                    inventory[2] = null;    //not in the inventory anymore
                    playerHoldObject.HoldCurrentObject();
                }
            }

/*             if(Input.GetKeyDown(KeyCode.E)){    //use the object
                if(currHoldingObj != null){
                    UseCurrentObject(currHoldingObj.tag);
                }
            } */
        }
    }

    public void InsertToInventory(GameObject obj){
        int index = GetAvailableSlotInInventory();        
        inventory[index] = obj;
        PutObjBackInInventory(obj, index + 1);
    }

    public int GetAvailableSlotInInventory(){
        int i;
        for(i = 0; i < inventory.Length; i++){
            if(inventory[i] == null){
                return i;
            }
        }
        return i;
    }

    public bool HasObjectOfTag(string tag){
        foreach(GameObject obj in inventory){
            if(obj != null && obj.tag == tag){
                return true;
            }
        }
        return false;
    }

    public bool CanAddMore(){
        if(GetAvailableSlotInInventory() == inventory.Length){
            return false;
        }
        return true;
    }

    private void PutObjBackInInventory(GameObject gameObject, int inventoryIndex){
        if(gameObject != null){
            if(gameObject.tag == "wallBrick"){
                return;
            }
            
            //place in the inventory
            gameObject.transform.parent = inventoryGameObject.transform;
            gameObject.transform.localPosition = new UnityEngine.Vector3(0, 0, 0);

            if(gameObject.tag == "Hammer")
            {
                //0.17 , 0 , 0 center 
                gameObject.transform.localRotation = UnityEngine.Quaternion.Euler(180, 90, inventoryObjRotation * inventoryIndex);
            }
            else if(gameObject.tag == "Compass")
            {
                gameObject.transform.localRotation = UnityEngine.Quaternion.Euler(0, 0, -90);
            }
            else if(gameObject.tag == "Sword")
            {
                gameObject.transform.localPosition = new UnityEngine.Vector3(0.03f, 0.41f, 0);
                gameObject.transform.localRotation = UnityEngine.Quaternion.Euler(0, 90, 180);

            }
            else if(gameObject.tag == "EnemyMask")
            {
                gameObject.transform.localPosition = new UnityEngine.Vector3(0, 0, 0);
                gameObject.transform.localRotation = UnityEngine.Quaternion.Euler(0, 90, 0);
            }

        }
    }
}
