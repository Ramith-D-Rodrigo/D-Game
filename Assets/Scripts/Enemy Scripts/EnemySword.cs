using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySword : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Animator enemyArmAnimator;
    [SerializeField] private HitPlayer hitPlayer;
    [SerializeField] private float swordDamage = 20.0f; //5 hits to kill player (100/20 = 5)
    private bool isUsingSword;
    public bool IsUsingSword {get { return isUsingSword;} }
    private PlayerCollision playerCollision;

    [SerializeField] private EnemyState enemyState;

    private bool isHittingNow; //actual starting point of hit

    private Sword sword;
    void Start()
    {
        isUsingSword = false;
        isHittingNow = false;

        sword = GetComponent<Sword>();
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyState.CurrentState == EnemyState.EnemyStates.HitPlayer){    //if enemy is in hitting state
            playerCollision = hitPlayer.HittingPlayer.GetComponent<PlayerCollision>();  //get player collision script

            if(playerCollision.IsPlayerDead){   //if player is dead
                hitPlayer.HittingPlayer = null; //stop hitting player

                enemyState.CurrentState = EnemyState.EnemyStates.Idle;

                playerCollision = null; //set player collision to null
            }
        } 
        else{   //if enemy is not in hitting state
            playerCollision = null;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.name == "Body" && other.transform.parent.tag == "Player" && isHittingNow){
            Debug.Log("hitting player with sword");

            if(playerCollision){
                if(!playerCollision.IsPlayerDead){
                    sword.PlaySwordHitSound();
                    playerCollision.ReducePlayerHealth(swordDamage);
                }
            }
        }
    }


    public IEnumerator UseSword(){
        isUsingSword = true;
        enemyArmAnimator.SetTrigger("toggleHit");   //start using the sword
        isHittingNow = true;    //also can hit

        yield return new WaitForSeconds(1.0f);  //wait until finishing one hit animation
        isHittingNow = false;   //not hitting anymore

        enemyArmAnimator.SetTrigger("toggleHit");//stop using the sword
        yield return new WaitForSeconds(0.2f);  //wait until finishing the animation
        isUsingSword = false;

    }
}
