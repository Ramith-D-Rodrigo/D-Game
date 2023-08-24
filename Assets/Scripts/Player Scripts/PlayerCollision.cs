using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCollision : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    private bool isCollidedWithObstacle;
    private bool isCollededWithWall;
    private Vector3 obstacleCollisionPointNormalized;
    private Vector3 wallCollisionPointNormalized;

    private bool isPlayerDead;
    public bool IsPlayerDead { get { return isPlayerDead; } }

    public float playerHealth;
    public float healthReduceFactor = 5f;
    public float healthIncreaseFactor = 10f;
    private bool isOnPath; //check whether player is on the path
    private Vector3 currCenterOfMass;
    private Animator animator;
    private PlayerHoldObject holdObjectComponent;
    private Animator[] allAnimators;
    [SerializeField] private SlideObstacleMover[] slideObstacleMovers;
    private SlidingDownObstacleMover[] slidingDownObstacleMovers;


    public GameObject[] playerBodyParts;

    public static float Map(float value, float inMin, float inMax, float outMin, float outMax){
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        isOnPath = true;

        animator = GetComponent<Animator>();
        holdObjectComponent = GetComponent<PlayerHoldObject>();

        allAnimators = FindObjectsOfType<Animator>();
        //get all slideObstacle movers

        //TODO: add the sliding down obstacles via editor
        slidingDownObstacleMovers = FindObjectsOfType<SlidingDownObstacleMover>();

        ResetPlayerStats();
    }

    private void ResetPlayerStats(){
        isCollidedWithObstacle = false;
        isCollededWithWall = false;
        isPlayerDead = false;
        playerHealth = 100f;
        currCenterOfMass = rb.centerOfMass;
    }

    private void OnCollisionEnter(Collision other) {
        ContactPoint contact = other.GetContact(0);
        Vector3 surfaceDirection = contact.normal;

        //slider collision
        if(other.gameObject.tag == "slidingObstacle" && Math.Abs(surfaceDirection.z) == 1){ //only z direction surface
            isCollidedWithObstacle = true;
            obstacleCollisionPointNormalized = surfaceDirection;
        }

        if(other.gameObject.tag == "wall"){
            isCollededWithWall = true;
            wallCollisionPointNormalized = surfaceDirection;
        }

        if(other.gameObject.tag == "slidingDownObstacle" && Math.Abs(surfaceDirection.y) == 1){ //only y direction surface
            //can kill player directly
            KillPlayer();
        }
    }

    private void OnCollisionExit(Collision other) {
        if(other.gameObject.tag == "slidingObstacle"){
            isCollidedWithObstacle = false;
        }

        if(other.gameObject.tag == "wall"){
            isCollededWithWall = false;
        }

        if(other.gameObject.tag == "Path"){
            isOnPath = false;
        }
    }

    private void OnCollisionStay(Collision other) {
        //path
        if(other.gameObject.tag == "Path"){
            isOnPath = true;
        }
    }

    private void Update() {
        //normalized vector check for opposing wall and obstacle surfaces
        if(isCollededWithWall && isCollidedWithObstacle && !isPlayerDead && obstacleCollisionPointNormalized.z != wallCollisionPointNormalized.z){  
            KillPlayer();
        }

        if(!isOnPath && !isPlayerDead){  //not on path and player is still alive
            ReducePlayerHealth(); //reduce the health
        }
        else if(isOnPath && !isPlayerDead){ //on the path and player is still alive
            IncreasePlayerHealth();
        }

        //update the current center of mass
        if(!isPlayerDead && currCenterOfMass.y != rb.centerOfMass.y){ //center of mass has been changed (on y coordinate)
            currCenterOfMass = rb.centerOfMass;
            ReducePlayerHealth();
        }
        else if(!isPlayerDead && currCenterOfMass.z != rb.centerOfMass.z && !isOnPath){
            currCenterOfMass = rb.centerOfMass;
            ReducePlayerHealth();
        }

        if(isPlayerDead && Input.GetKeyDown(KeyCode.Escape)){
            StartCoroutine(ResetPlayer());
        }
    }

    private void ReducePlayerHealth(){
        playerHealth -= Time.deltaTime * healthReduceFactor;

        //change player color to reflect the health
        ChangePlayerColor();

        if(playerHealth <= 0f){
            KillPlayer();
        }
    }

    private void IncreasePlayerHealth(){
        if(playerHealth >= 100){ //no need to increase further
            playerHealth = 100f;
            return;
        }
        playerHealth += Time.deltaTime * healthIncreaseFactor;

        //change color of player to show the health 
        ChangePlayerColor();
    }

    private void KillPlayer(){
        rb.isKinematic = true;
        animator.SetBool("isRunning", false);
        animator.SetBool("isWalking", false);

        animator.enabled = false;

        //disable all the scripts of the player movement
        this.GetComponent<PlayerMover>().enabled = false;
    
        for(int i = 0; i < playerBodyParts.Length; i++){
            GameObject bodyPart = playerBodyParts[i];
            Rigidbody rb = bodyPart.AddComponent<Rigidbody>();
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }

        //go through all the inventory items and enable the dropping object script
        GameObject[] playerInventory = GetComponent<PlayerInventory>().Inventory;

        foreach(GameObject inventoryItem in playerInventory){

            if(inventoryItem == null){
                continue;
            }
            
            inventoryItem.GetComponent<DroppingObject>().enabled = true;
        }

        //drop the current holding object
        GameObject currHoldingObject = holdObjectComponent.CurrHoldingObject;
        if(currHoldingObject != null){
            currHoldingObject.GetComponent<DroppingObject>().enabled = true;
        }

        isPlayerDead = true;
    }

    private void ChangePlayerColor(){
        for(int i = 0; i < playerBodyParts.Length; i++){
            MeshRenderer mr = playerBodyParts[i].GetComponent<MeshRenderer>();

            Color tempCol = mr.materials[1].color;
            mr.materials[1].color = new Color(tempCol.r, tempCol.g, tempCol.b, Map(100 - playerHealth, 0, 100, 0, 255)/255);
        }
    }


    private IEnumerator ResetPlayer(){
        //pause all the animators
        foreach(Animator animator in allAnimators){
            animator.enabled = false;
        }
        foreach(SlideObstacleMover slideObstacleMover in slideObstacleMovers){
            slideObstacleMover.enabled = false;
        }
        foreach(SlidingDownObstacleMover slidingDownObstacle in slidingDownObstacleMovers){
            slidingDownObstacle.enabled = false;
        }

        //get all animators of the player
        Animator[] playerAnimators = this.gameObject.GetComponentsInChildren<Animator>();

        //disable them
        foreach(Animator animator in playerAnimators){
            animator.enabled = false;
        }

        MovementRecorder movementRecorder = GetComponent<MovementRecorder>();

        float timeCounter = movementRecorder.maxRecordingSecs;
        movementRecorder.IsResetting = true;

        //remove body part rigidbodies
        for(int i = 0; i < playerBodyParts.Length; i++){
            GameObject bodyPart = playerBodyParts[i];
            Rigidbody rb = bodyPart.GetComponent<Rigidbody>();
            Destroy(rb);
        }

        while(timeCounter > 0.0f && movementRecorder.IsResetting){
            movementRecorder.RewindPlayerLastTransformation();
            timeCounter -= Time.fixedDeltaTime;
            yield return null;
        }

        //enable rigidbody of player
        rb.isKinematic = false;

        //set isplayerdead
        isPlayerDead = false;

        //enable player mover script
        this.GetComponent<PlayerMover>().enabled = true;

        //enable the animators
        foreach(Animator animator in playerAnimators){
            animator.enabled = true;
        }

        //enable animator
        animator.enabled = true;

        ResetPlayerStats();

        foreach(SlidingDownObstacleMover slidingDownObstacle in slidingDownObstacleMovers){
            slidingDownObstacle.enabled = true;
        }

        foreach(SlideObstacleMover slideObstacleMover in slideObstacleMovers){
            slideObstacleMover.enabled = true;
        }
        //re-enable all animators
        foreach(Animator animator in allAnimators){
            animator.enabled = true;
        }
    }
}
