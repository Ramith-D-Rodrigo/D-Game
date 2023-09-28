using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BreakableWall : MonoBehaviour
{
    public int hitPoints = 30;
    private bool isBroken = false;

    public float brickDensity = 0.1f;
    public float brickDrag = 0.01f;
    public float brickAngularDrag = 0.1f;
    //private PlayerInventory playerInventory;

    public PlayerUseObject playerUseObject;

    [SerializeField] private TextMeshProUGUI hitPointsText;
    [SerializeField] private float amountOfDisplayTimeInSeconds = 2.0f;

    private AudioSource audioSource;
    [SerializeField] private AudioClip[] hammerHitSounds;
    [SerializeField] private AudioClip[] wallBreakSounds;


    private bool isTextDisplaying;

    private void Start(){
        hitPoints = UnityEngine.Random.Range(10, 20);

        hitPointsText.transform.parent.gameObject.SetActive(false);
        hitPointsText.SetText(hitPoints.ToString());
        isTextDisplaying = false;

        audioSource = this.GetComponent<AudioSource>();
    }

    private void Update(){
        ShowHitPoints();
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
        audioSource.PlayOneShot(hammerHitSounds[UnityEngine.Random.Range(0, hammerHitSounds.Length)], 0.5f);

        if(hitPoints <= 0 && !isBroken){
            BreakTheWall();
        }
        else{
            hitPoints--;

            if(!isTextDisplaying){
                StartCoroutine(DisplayHitPointsText(amountOfDisplayTimeInSeconds));
            }
        }
    }

    private void ShowHitPoints(){
        hitPointsText.SetText(hitPoints.ToString());
    }

    private IEnumerator DisplayHitPointsText(float amountOfDisplayTimeInSeconds){
        isTextDisplaying = true;
        hitPointsText.transform.parent.gameObject.SetActive(true);

        yield return new WaitForSeconds(amountOfDisplayTimeInSeconds);

        hitPointsText.transform.parent.gameObject.SetActive(false);
        isTextDisplaying = false;
    }

    private void BreakTheWall()
    {
        audioSource.PlayOneShot(wallBreakSounds[UnityEngine.Random.Range(0, wallBreakSounds.Length)], 0.5f);
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

                child.GetComponent<SphereCollider>().enabled = true;
            }
            startingWallBrick += 5; //next row
        }
    }
}
