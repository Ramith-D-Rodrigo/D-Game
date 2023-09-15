using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySword : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Animator enemyArmAnimator;
    [SerializeField] private HitPlayer hitPlayer;
    [SerializeField] private LookAtPlayer lookAtPlayer;
    [SerializeField] private FollowPlayer followPlayer;
    [SerializeField] private float swordDamage = 25.0f; //4 hits to kill player (100/25 = 4)
    private bool isUsingSword;
    public bool IsUsingSword {get { return isUsingSword;} }
    private PlayerCollision playerCollision;

    private bool isHittingNow; //actual starting point of hit
    void Start()
    {
        isUsingSword = false;
        isHittingNow = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(hitPlayer.HittingPlayer){    //if enemy is within hit range
            playerCollision = hitPlayer.HittingPlayer.GetComponent<PlayerCollision>();  //get player collision script

            if(playerCollision.IsPlayerDead){   //if player is dead
                hitPlayer.HittingPlayer = null; //stop hitting player
                followPlayer.Player = null;
                lookAtPlayer.Player = null;
                playerCollision = null; //set player collision to null
            }
        } 
        else{   //if enemy is not within hit range
            playerCollision = null;
        }
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("hitting player with sword");
        if(other.gameObject.name == "Body" && other.transform.parent.tag == "Player" && isHittingNow){

            if(playerCollision){
                if(!playerCollision.IsPlayerDead){
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
        yield return new WaitForSeconds(1.0f);  //wait until finishing the animation
        isUsingSword = false;

    }
}
