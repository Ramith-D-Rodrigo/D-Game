using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AudioClip[] swordHitSounds;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySwordHitSound(){
        audioSource.PlayOneShot(swordHitSounds[Random.Range(0, swordHitSounds.Length)]);
    }
}
