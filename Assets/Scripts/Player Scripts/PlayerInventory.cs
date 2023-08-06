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
    private int inventoryIndex;
    private int inventoryObjRotation = -45;
    private int currHoldingObjIndex;
    public PlayerUseObject playerUseObjectLeft; //for left arm
    public PlayerUseObject playerUseObjectRight; //for right arm

    private void Start(){
        inventory = new GameObject[3]; //only three in the inventory
        inventoryIndex = 0;
        currHoldingObjIndex = -1;
        playerUseObjectLeft = this.transform.parent.GetChild(5).GetComponent<PlayerUseObject>();
        playerUseObjectRight = this.transform.parent.GetChild(6).GetComponent<PlayerUseObject>();
    }
    public GameObject[] GetInventory(){
        return inventory;
    }

    private void Update(){

        //not using any object
        if(!playerUseObjectLeft.GetIsUsingObject() && !playerUseObjectRight.GetIsUsingObject()){
            if(Input.GetKeyDown(KeyCode.Alpha1)){   //first object in the inventory
                if(currHoldingObjIndex != 0){
                    currHoldingObjIndex = 0;
                    HoldCurrentObject();
                }
                else{ //same object, so put back to inventory
                    PutObjBackInInventory(inventory[currHoldingObjIndex], 1);
                    currHoldingObjIndex = -1;
                }
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2)){  //second object in the inventory
                currHoldingObjIndex = 1;
            }
            else if(Input.GetKeyDown(KeyCode.Alpha3)){
                currHoldingObjIndex = 2;
            }

            if(Input.GetKeyDown(KeyCode.E)){    //use the object
                if(currHoldingObjIndex != -1){
                    UseCurrentObject(inventory[currHoldingObjIndex].tag);
                }
            }
        }

        if(currHoldingObjIndex != -1 && Input.GetKeyDown(KeyCode.F)){
            //drop
            DropObject();
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

    public bool CanAddMore(){
        if(GetAvailableSlotInInventory() == inventory.Length){
            return false;
        }
        return true;
    }

    private void HoldCurrentObject(){
        if(currHoldingObjIndex != -1){

            GameObject leftArm = this.transform.parent.GetChild(5).GetChild(0).gameObject;
            GameObject rightArm = this.transform.parent.GetChild(6).GetChild(0).gameObject;

            switch(inventory[currHoldingObjIndex].gameObject.tag){
                case "Hammer":
                    //rotation (180, 0, 180)
                    // position (-0.5, -0.45, 0)
                    inventory[currHoldingObjIndex].transform.localRotation = UnityEngine.Quaternion.Euler(180, 0, 180);
                    //add it to the left hand

                    //5th child is arm rotation point
                    inventory[currHoldingObjIndex].transform.SetParent(leftArm.transform);
                    inventory[currHoldingObjIndex].transform.localPosition = new UnityEngine.Vector3(-0.5f, -0.45f, 0);
                    break;

                case "WallBrick":
                    // leftArmRotationPoint = leftArm.transform.parent.GetComponent<Transform>();
                    //Transform rightArmRotationPoint = rightArm.transform.parent.GetComponent<Transform>();
                    //leftArmRotationPoint.localRotation = UnityEngine.Quaternion.Euler(leftArmRotationPoint.rotation.x, leftArmRotationPoint.rotation.y, -90f);
                    //rightArmRotationPoint.localRotation = UnityEngine.Quaternion.Euler(rightArmRotationPoint.rotation.x, rightArmRotationPoint.rotation.y, -90f);
                    inventory[currHoldingObjIndex].transform.SetParent(leftArm.transform);
                    inventory[currHoldingObjIndex].transform.localPosition = new UnityEngine.Vector3(-0.5f, -0.45f, 0);
                    UseCurrentObject(inventory[currHoldingObjIndex].tag);
                    break;
            }      
        }
    }


    private void PutObjBackInInventory(GameObject gameObject, int inventoryIndex){
        if(gameObject != null){
            //set sphere collider
            if(gameObject.tag == "Hammer"){
                //0.17 , 0 , 0 center 
                //remove the sphere collider and give the mesh collider
                Destroy(gameObject.GetComponent<SphereCollider>());

                MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
                meshCollider.convex = true;
                meshCollider.isTrigger = true;
            }

            gameObject.transform.parent = this.transform;
            gameObject.transform.localPosition = new UnityEngine.Vector3(0, 0, 0);
            gameObject.transform.localRotation = UnityEngine.Quaternion.Euler(180, 90, inventoryObjRotation * inventoryIndex);
        }
    }

    private void UseCurrentObject(string objectType){
        playerUseObjectLeft.UseObject("Using" + objectType);
        playerUseObjectRight.UseObject("Using" + objectType);
    }

    public PlayerUseObject GetPlayerUseObjectLeft(){
        return playerUseObjectLeft;
    }

    public PlayerUseObject GetPlayerUseObjectRight(){
        return playerUseObjectRight;
    }

//TODO:: current error is that holding that particular object will throw error if it is not in the inventory
    public void SetCurrHoldingObject(GameObject obj){
        inventory[currHoldingObjIndex] = obj;
        HoldCurrentObject();
    } 

    public GameObject GetCurrHoldingObject(){
        return inventory[currHoldingObjIndex];
    }

    private void DropObject(){
        inventory[currHoldingObjIndex].transform.SetParent(null, true);

        //attach dropping object script
        inventory[currHoldingObjIndex].gameObject.AddComponent<DroppingObject>();

        inventory[currHoldingObjIndex] = null;
        currHoldingObjIndex = -1;

    }
}
