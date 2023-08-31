using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHoldObject : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject currHoldingObject;
    private PlayerInventory playerInventory;
    private PlayerPickable playerPickable;

    public PlayerUseObject playerUseObject;

    public GameObject leftArm;
    public GameObject rightArm;

    public GameObject CurrHoldingObject { get { return currHoldingObject; } set { currHoldingObject = value; } }

    void Start(){
        currHoldingObject = null;
        playerInventory = GetComponent<PlayerInventory>();
        playerPickable = GetComponent<PlayerPickable>();
    }

    // Update is called once per frame
    void Update(){
        if(Input.GetKeyDown(Controls.UseObj)){    //use the object
            if(currHoldingObject != null){
                UseCurrentObject(currHoldingObject.tag);
            }
        }

        
        if(CurrHoldingObject != null && Input.GetKeyDown(Controls.DropObj)){
            //drop
            DropObject();
        }

        if(Input.GetKeyDown(Controls.PickUpObj) && currHoldingObject != null){    //putting back to inventory check
            playerPickable.PickableObj = currHoldingObject;
            playerPickable.ProcessPickUp();
        }
    }

    private void UseCurrentObject(string objectType){
        playerUseObject.UseObject(objectType);
    }


    public void HoldCurrentObject(){
        if(currHoldingObject != null){
            switch(currHoldingObject.gameObject.tag){
                case "Hammer":
                    //rotation (180, 0, 180)
                    // position (-0.5, -0.45, 0)
                    currHoldingObject.transform.localRotation = UnityEngine.Quaternion.Euler(180, 0, 180);
                    //add it to the left hand

                    currHoldingObject.transform.SetParent(leftArm.transform);
                    currHoldingObject.transform.localPosition = new UnityEngine.Vector3(-0.5f, -0.45f, 0);
                    break;

                case "WallBrick":
                    UseCurrentObject(currHoldingObject.gameObject.tag);
                    HoldWallBrick();
                    break;
            }      
        }
    }

    void HoldWallBrick(){
        currHoldingObject.transform.SetParent(this.transform);
        currHoldingObject.transform.localRotation = UnityEngine.Quaternion.Euler(0, 0, 0);
        currHoldingObject.transform.localPosition = new UnityEngine.Vector3(-7f, 6f, 0);

        //set the rigidbody to iskinematic
        currHoldingObject.GetComponent<Rigidbody>().isKinematic = true;

        currHoldingObject.GetComponent<SphereCollider>().enabled = false;
    }

    private void DropObject(){
        //enable dropping object script
        currHoldingObject.gameObject.GetComponent<DroppingObject>().enabled = true;

        //animations
        if(currHoldingObject.tag == "WallBrick"){
            playerUseObject.StopUsingWallBrick();
        }

        currHoldingObject = null;
    }
}
