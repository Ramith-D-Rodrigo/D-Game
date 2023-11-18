using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{

    [SerializeField] private int hitPoints;
    [Tooltip("Lower the difficulty value means the enemy is hitting player more often")]
    [SerializeField] private int difficulty;
    private int maxHitPoints = 6;
    private int minHitPoints = 0;
    public int HitPoints { get { return hitPoints; } set { hitPoints = value; } }
    public int Difficulty { get { return difficulty; } set { difficulty = value; } }

    private bool isDead = false;
    public bool IsDead { get { return isDead; } set { isDead = value; } }

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private GameObject[] enemyBodyParts;

    [SerializeField] private Animator enemyArmAnimator;

    //all enemy scripts
    [SerializeField] private EnemyMover enemyMover;
    [SerializeField] private EnemySword enemySword;
    [SerializeField] private FollowPlayer followPlayer;
    [SerializeField] private LookAtPlayer lookAtPlayer;
    [SerializeField] private HitPlayer hitPlayer;
    [SerializeField] private EnemyState enemyState;
    [SerializeField] private GameObject enemyMaskPlaceholder;
    public GameObject EnemyMaskPlaceholder { get { return enemyMaskPlaceholder; } }


    // Start is called before the first frame update
    void Start()
    {
        hitPoints = maxHitPoints;
    }

    // Update is called once per frame
    void Update()
    {
        if(hitPoints <= 0 && !isDead){
            isDead = true;
            KillEnemy();
        }

    }

    private void KillEnemy()
    {
        rb.isKinematic = true;
        enemyAnimator.SetBool("isRunning", false);
        enemyAnimator.SetBool("isWalking", false);

        enemyAnimator.enabled = false;

        //disable all the scripts of the enemy movement
        enemyMover.enabled = false;
        Destroy(enemyMover);

        for(int i = 0; i < enemyBodyParts.Length; i++){
            GameObject bodyPart = enemyBodyParts[i];
            Rigidbody rb = bodyPart.AddComponent<Rigidbody>();
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }

        //disable the enemy arm animator
        enemyArmAnimator.enabled = false;
        Destroy(enemyArmAnimator);

        //disable the remaining enemy scripts

        //add rigidbody to the enemy sword and disable the enemy sword script
        enemySword.transform.parent.gameObject.AddComponent<Rigidbody>();
        BoxCollider longer = enemySword.GetComponent<BoxCollider>(); //longer one
        BoxCollider shorter = enemySword.transform.GetChild(0).GetComponent<BoxCollider>();   //short horizontal one

        longer.isTrigger = false;
        shorter.isTrigger = false;

        enemySword.enabled = false;
        Destroy(enemySword);


        //disable the follow player script
        followPlayer.enabled = false;
        Destroy(followPlayer);

        //disable the look at player script
        lookAtPlayer.enabled = false;
        Destroy(lookAtPlayer);

        //disable the hit player script
        hitPlayer.enabled = false;
        Destroy(hitPlayer);

        //enable the sphere collider on maskPlaceholder
        enemyMaskPlaceholder.GetComponent<SphereCollider>().enabled = true;

        enemyState.CurrentState = EnemyState.EnemyStates.Dead;
    }

    private void ChangeEnemyColor(){
        //change the color of the enemy upon hit

        for(int i = 0; i < enemyBodyParts.Length; i++){
            GameObject bodyPart = enemyBodyParts[i];
            Renderer rend = bodyPart.GetComponent<Renderer>();
            
            Color currColor = rend.materials[1].color;
            rend.materials[1].color = new Color(currColor.r, currColor.g, currColor.b, Mapper.Map(maxHitPoints - hitPoints, minHitPoints, maxHitPoints, 0, 255) / 255);
        }
    }

    public void HitEnemy(){
        hitPoints--;
        ChangeEnemyColor();
    }
}
