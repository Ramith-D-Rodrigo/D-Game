using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    public int hitPoints = 30;
    private bool isBroken = false;

    public float brickDensity = 0.1f;
    public float brickDrag = 0.1f;
    public float brickAngularDrag = 0.1f;
    //private PlayerInventory playerInventory;

    public PlayerUseObject playerUseObject;

    private void Start(){
        hitPoints = UnityEngine.Random.Range(10, 20);
    }
    private void OnTriggerEnter(Collider other) { //other should be a gameobject inside the inventory

        if(other.gameObject.tag == "Hammer"){
            //parent = left arm -> left arm rotation point0
            try
            {
                if(playerUseObject.GetIsUsingObject()){
                    HammerHit();
                }
            }
            catch (NullReferenceException){
                return;
            }

        }
    }

    private void HammerHit()
    {
        if(hitPoints <= 0 && !isBroken){
            BreakTheWall();
        }
        else{
            hitPoints--;
        }
    }

    private void BreakTheWall()
    {
        //remove this object's rigidbody
        Destroy(this.GetComponent<Rigidbody>());
        isBroken = true;
        int startingWallBrick = UnityEngine.Random.Range(16, 18); //top left starting brick to open the path
        
        int removedChildCount = 0;
        int currChildCount = this.transform.childCount;

        while(startingWallBrick < currChildCount){
            for(int i = 0; i < 3; i++){ //column
                Transform child = this.transform.GetChild(startingWallBrick - 1 + i - removedChildCount);
                child.SetParent(null);
                removedChildCount++; //to ammend the parent switch
                Rigidbody childRB = child.gameObject.AddComponent<Rigidbody>();
                childRB.SetDensity(brickDensity);
                childRB.mass = childRB.mass;
                childRB.drag = brickDrag;
                childRB.angularDrag = brickAngularDrag;

                //enable the sphere collider
                child.GetComponent<SphereCollider>().enabled = true;
            }
            startingWallBrick += 5; //next row
        }
    }
}
