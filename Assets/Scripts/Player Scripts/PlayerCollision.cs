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

    //LEVEL 1 ATTRIBUTES
    private bool isCollidedWithObstacle;
    private bool isCollededWithWall;
    private Vector3 obstacleCollisionPointNormalized;
    private Vector3 wallCollisionPointNormalized;
    private bool isOnPath; //check whether player is on the path


    //LEVEL 2 ATTRIBTUES
    private bool isOnTerrain;
    private float playerGroundLevel;
    public bool IsOnTerrain {get { return isOnTerrain;} }

    private bool isPlayerDead;
    public bool IsPlayerDead { get { return isPlayerDead; } }

    [SerializeField] private float playerHealth;
    public float PlayerHealth { get { return playerHealth; } set { playerHealth = value; } }
    public float healthReduceFactor = 5f;
    public float healthIncreaseFactor = 10f;

    private float timeTillResetMessage = 2.0f;
    private Vector3 currCenterOfMass;
    private Animator animator;
    private PlayerHoldObject holdObjectComponent;
    private PlayerUseObject useObjectComponent;
    private Animator[] allAnimators;
    [SerializeField] private SlideObstacleMover[] slideObstacleMovers;
    [SerializeField] private SlidingDownObstacleMover[] slidingDownObstacleMovers;
    private MovementRecorder movementRecorder;
    private bool playerPrevState;
    public bool PlayerPrevState { get { return playerPrevState; } set { playerPrevState = value; } }
    [SerializeField] private HUD hud;

    [SerializeField] private LevelManager levelManager;

    public GameObject[] playerBodyParts;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        playerHealth = 100f;

        switch(levelManager.CurrLevelIndex){
            case 1:
                InitializeLevelOneSettings();
                break;
            
            case 2:
                InitializeLevelTwoSettings();
                break;
        }

        animator = GetComponent<Animator>();
        holdObjectComponent = GetComponent<PlayerHoldObject>();
        useObjectComponent = GetComponentInChildren<PlayerUseObject>(); //this is in player arms

        allAnimators = FindObjectsOfType<Animator>();
        //get all slideObstacle movers

        movementRecorder = GetComponent<MovementRecorder>();

        ResetPlayerStats();

    }

    private void ResetPlayerStats(){
        isCollidedWithObstacle = false;
        isCollededWithWall = false;
        isPlayerDead = false;
        currCenterOfMass = rb.centerOfMass;
    }

    private void OnCollisionEnter(Collision other) {
        int currLevel = levelManager.CurrLevelIndex;

        switch(currLevel){
            case 1:
                ProcessLevelOneCollisionEnter(other);
                break;
            
            case 2:
                ProcessLevelTwoCollisionEnter(other);
                break;
        }

    }

    private void OnCollisionExit(Collision other) {

        int currLevel = levelManager.CurrLevelIndex;

        switch(currLevel){
            case 1:
                ProcessLevelOneCollisionExit(other);
                break;
            
            case 2:
                ProcessLevelTwoCollisionExit(other);
                break;
        }
    }

    private void OnCollisionStay(Collision other) {

        int currLevel = levelManager.CurrLevelIndex;

        switch(currLevel){
            case 1:
                ProcessLevelOneCollisionStay(other);
                break;
            
            case 2:
                ProcessLevelTwoCollisionStay(other);
                break;
        }
    }

    private void Update() {

        int currLevel = levelManager.CurrLevelIndex;

        switch(currLevel){
            case 1:
                ProcessLevelOneUpdate();
                break;
            
            case 2:
                ProcessLevelTwoUpdate();
                break;
        }

        //general scenario for all levels
        if(isPlayerDead && Input.GetKeyDown(Controls.ResetPlayer) && !movementRecorder.IsResetting){
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

    public void ReducePlayerHealth(float damage){   //reduce player health by certain amount (overloaded function)
        playerHealth -= damage;

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

        for(int i = 0; i < playerInventory.Length; i++){

            if(playerInventory[i] == null){
                continue;
            }

            playerInventory[i].GetComponent<DroppingObject>().enabled = true;
            playerInventory[i] = null; //not in inventory anymore
        }

        //drop the current holding object
        holdObjectComponent.DropObject();

        isPlayerDead = true;
        playerPrevState = isPlayerDead;
        StartCoroutine(DisplayResetMessage(timeTillResetMessage)); //display reset message after certain time
    }

    private IEnumerator DisplayResetMessage(float waitTime){
        yield return new WaitForSeconds(waitTime);
        if(!movementRecorder.IsResetting){
            hud.DisplayResetMessage();
        }
    }

    private void ChangePlayerColor(){
        for(int i = 0; i < playerBodyParts.Length; i++){
            MeshRenderer mr = playerBodyParts[i].GetComponent<MeshRenderer>();

            Color tempCol = mr.materials[1].color;
            mr.materials[1].color = new Color(tempCol.r, tempCol.g, tempCol.b, Mapper.Map(100 - playerHealth, 0, 100, 0, 255)/255);
        }
    }


    private IEnumerator ResetPlayer(){
        hud.HideMessage();
        hud.gameObject.SetActive(false);

        movementRecorder.IsResetting = true;
        //pause all the animators
        foreach(Animator animator in allAnimators){
            if(animator == null){ //already destroyed
                continue;
            }
            animator.enabled = false;
        }

        if(levelManager.CurrLevelIndex == 1){
            DisableSlidingObstacles();
        }

        //get all animators of the player
        Animator[] playerAnimators = this.gameObject.GetComponentsInChildren<Animator>();

        //disable them
        foreach(Animator animator in playerAnimators){
            animator.enabled = false;
        }

        float timeCounter = movementRecorder.maxRecordingSecs;


        //remove body part rigidbodies
        for(int i = 0; i < playerBodyParts.Length; i++){
            GameObject bodyPart = playerBodyParts[i];
            Rigidbody rb = bodyPart.GetComponent<Rigidbody>();
            Destroy(rb);
        }

        bool playerStateBeforeLastTransformationRewind = playerPrevState;
        bool canUseTimer = false;

        while(timeCounter > 0.0f && movementRecorder.IsResetting){
            movementRecorder.RewindPlayerLastTransformation(); //also changes health (player)
            ChangePlayerColor(); //so change the color of player
            bool playerStateAfterLastTransformationRewind = playerPrevState;

            //Debug.Log(playerStateBeforeLastTransformationRewind + " " + playerStateAfterLastTransformationRewind);

            if(playerStateBeforeLastTransformationRewind != playerStateAfterLastTransformationRewind){  //changed state from death to alive
                //now start the timer count
                canUseTimer = true;
                playerStateBeforeLastTransformationRewind = playerStateAfterLastTransformationRewind;
            }

            if(canUseTimer){
                timeCounter -= Time.fixedDeltaTime;
            }

            yield return new WaitForFixedUpdate();
        }

        //enable rigidbody of player
        rb.isKinematic = false;

        //enable player mover script
        this.GetComponent<PlayerMover>().enabled = true;

        //enable the animators
        foreach(Animator animator in playerAnimators){
            animator.enabled = true;
        }

        movementRecorder.IsResetting = false; //finished resetting

        //enable animator
        animator.enabled = true;

        if(levelManager.CurrLevelIndex == 1){
            EnableSlidingObstacles();
        }


        //re-enable all animators
        foreach(Animator animator in allAnimators){
            if(animator == null){   //already destroyed
                continue;
            }

            animator.enabled = true;
        }

        hud.gameObject.SetActive(true);

        ResetPlayerStats();
    }


    //----------------------------------------- LEVEL 1 FUNCTIONS ------------------------------------------------
    private void InitializeLevelOneSettings()
    {
        isOnPath = true;
    }

    private void ProcessLevelOneCollisionEnter(Collision other)
    {
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

        if(other.gameObject.tag == "slidingDownObstacle" && Math.Abs(surfaceDirection.y) == 1 && !isPlayerDead){ //only y direction surface
            //can kill player directly
            KillPlayer();
        }
    }

    private void ProcessLevelOneCollisionExit(Collision other)
    {
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

    private void ProcessLevelOneCollisionStay(Collision other)
    {
        //path
        if(other.gameObject.tag == "Path"){
            isOnPath = true;
        }
    }

    private void ProcessLevelOneUpdate()
    {
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
    }

    private void DisableSlidingObstacles(){
        foreach(SlideObstacleMover slideObstacleMover in slideObstacleMovers){
            slideObstacleMover.enabled = false;
        }
        foreach(SlidingDownObstacleMover slidingDownObstacle in slidingDownObstacleMovers){
            slidingDownObstacle.enabled = false;
        }
    }

    private void EnableSlidingObstacles(){
        foreach(SlidingDownObstacleMover slidingDownObstacle in slidingDownObstacleMovers){
            slidingDownObstacle.enabled = true;
        }

        foreach(SlideObstacleMover slideObstacleMover in slideObstacleMovers){
            slideObstacleMover.enabled = true;
        }
    }

    //----------------------------------------- LEVEL 2 FUNCTIONS ------------------------------------------------
    private void InitializeLevelTwoSettings()
    {
        isOnTerrain = true;
        playerGroundLevel = Mathf.Round(this.transform.position.y);
    }

    private void ProcessLevelTwoCollisionEnter(Collision other)
    {
    }

    private void ProcessLevelTwoCollisionExit(Collision other)
    {
        if(other.gameObject.tag == "Terrain"){
            isOnTerrain = false;
        }
    }

    private void ProcessLevelTwoCollisionStay(Collision other)
    {
        if(other.gameObject.tag == "Terrain"){
            isOnTerrain = true;
        }
    }
    private void ProcessLevelTwoUpdate()
    {
        float currBodyYPos = Mathf.Round(this.transform.position.y);

        if(!isPlayerDead && !isOnTerrain){ ///not dead and not touching the terrain
            ReducePlayerHealth();
        }
        
        if(!isPlayerDead && isOnTerrain && playerGroundLevel == currBodyYPos && currCenterOfMass.y == rb.centerOfMass.y){
            IncreasePlayerHealth();
        }
        else if(!isPlayerDead && isOnTerrain && currCenterOfMass.y != rb.centerOfMass.y){ //not dead and touching the terrain, but center of mass is changing on y axis
            Debug.Log("mass changed" + rb.centerOfMass);
            currCenterOfMass = rb.centerOfMass;
            ReducePlayerHealth();
        }
        else if(!isPlayerDead && isOnTerrain && playerGroundLevel != currBodyYPos){ //not dead and touching the terrain, but not on the ground level
            ReducePlayerHealth();
        }

    }

}
