using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBrick : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] AudioClip[] floorHitSounds;
    [SerializeField] AudioClip[] otherBricksHitSounds;
    private AudioSource audioSource;

    private Transform parentWall;
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        parentWall = this.transform.parent;
    }


    private void OnTriggerEnter(Collider other) {

        if(other.gameObject.tag == "Ground" || other.gameObject.tag == "Path"){
            audioSource.PlayOneShot(floorHitSounds[Random.Range(0, floorHitSounds.Length)], 0.2f);
        }
        else if(other.gameObject.tag == "WallBrick"){
            //if the brick is within the wall, don't play the sound (compare the transform position of the brick and the wall)
            if(Mathf.Abs(parentWall.position.x - this.transform.position.x) < 2f){
                return;
            }

            audioSource.PlayOneShot(otherBricksHitSounds[Random.Range(0, otherBricksHitSounds.Length)], 0.2f);
        }
    }

    private void OnCollisionEnter(Collision other) {

        if(other.gameObject.tag == "Ground" || other.gameObject.tag == "Path"){
            audioSource.PlayOneShot(floorHitSounds[Random.Range(0, floorHitSounds.Length)], 0.2f);
        }
        else if(other.gameObject.tag == "WallBrick"){
            //if the brick is within the wall, don't play the sound (compare the transform position of the brick and the wall)
            if(Mathf.Abs(parentWall.position.x - this.transform.position.x) < 2f){
                return;
            }
            audioSource.PlayOneShot(otherBricksHitSounds[Random.Range(0, otherBricksHitSounds.Length)], 0.2f);
        }
    }
}
