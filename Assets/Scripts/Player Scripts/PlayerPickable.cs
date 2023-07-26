using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickable : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject pickableObj;
    private PlayerInventory playerInventoryComponent;
    void Start()
    {
        pickableObj = null;
        playerInventoryComponent = this.transform.GetChild(0).GetComponent<PlayerInventory>(); //first child is the inventory object
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Hammer"){
            pickableObj = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Hammer"){
            pickableObj = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(pickableObj != null & Input.GetKeyDown(KeyCode.F) & playerInventoryComponent.CanAddMore()){
            PickUpObject();
        }
    }

    private void PickUpObject(){
        playerInventoryComponent.InsertToInventory(pickableObj);
    }

}
