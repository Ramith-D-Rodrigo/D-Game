using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private GameObject[] inventory;
    private int inventoryIndex;
    private int inventoryObjRotation = -45;
    private GameObject currUsingObj;


    private void Start(){
        inventory = new GameObject[3]; //only three in the inventory
        inventoryIndex = 0;
        currUsingObj = null;
    }
    public GameObject[] GetInventory(){
        return inventory;
    }

    public int GetInventoryIndex(){
        return inventoryIndex;
    }

    private void Update(){

        if(Input.GetKeyDown(KeyCode.Alpha1)){   //first object in the inventory
            if(currUsingObj != inventory[0]){
                currUsingObj = inventory[0];
                HoldCurrentObject();
            }
            else{ //same object, so put back to inventory
                PutObjBackInInventory(currUsingObj, 1);
                currUsingObj = null;
            }
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2)){  //second object in the inventory
            currUsingObj = inventory[1];
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3)){
            currUsingObj = inventory[2];
        }
    }

    public void InsertToInventory(GameObject obj){
        inventory[inventoryIndex++] = obj;
        PutObjBackInInventory(obj, inventoryIndex);
    }

    public bool CanAddMore(){
        if(inventoryIndex < inventory.Length){
            return true;
        }
        return false;
    }

    private void HoldCurrentObject(){
        if(currUsingObj != null){
            //0.17 , 0 , 0 center 
            //radius 0.06
            //rotation (180, 0, 180)
            // position (-0.5, -0.45, 0)
            if(currUsingObj.gameObject.tag == "Hammer"){
                currUsingObj.transform.localRotation = UnityEngine.Quaternion.Euler(180, 0, 180);
                SphereCollider sc = currUsingObj.GetComponent<SphereCollider>();
                sc.center = new UnityEngine.Vector3(0.17f, 0, 0);
                sc.radius = 0.06f;
            }

            //add it to the left hand

            //5th child is arm rotation point
            currUsingObj.transform.SetParent(this.transform.parent.GetChild(5).GetChild(0));
            currUsingObj.transform.localPosition = new UnityEngine.Vector3(-0.5f, -0.45f, 0);
        }
    }


    private void PutObjBackInInventory(GameObject gameObject, int inventoryIndex){
        if(gameObject != null){
            gameObject.transform.parent = this.transform;
            gameObject.transform.localPosition = new UnityEngine.Vector3(0, 0, 0);
            gameObject.transform.localRotation = UnityEngine.Quaternion.Euler(180, 90, inventoryObjRotation * inventoryIndex);
        }
    }
}
