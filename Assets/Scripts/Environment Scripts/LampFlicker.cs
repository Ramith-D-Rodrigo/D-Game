using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampFlicker : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator lampFlickAnimation;
    void Start()
    {
        lampFlickAnimation = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Random.Range(0, 1000) % 327 == 0){
            lampFlickAnimation.SetTrigger("Flickering");
        }
        else{
            //lampFlickAnimation.Stop();
        }
    }
}
