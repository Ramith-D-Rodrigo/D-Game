using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{

    [SerializeField] private int hitPoints = 6;
    public int HitPoints { get { return hitPoints; } set { hitPoints = value; } }

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


    // Start is called before the first frame update
    void Start()
    {

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
    }
}
