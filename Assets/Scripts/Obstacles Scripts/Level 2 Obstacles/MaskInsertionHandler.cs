using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskInsertionHandler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private LayerMask playerLayer;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    private void OnTriggerEnter(Collider other) {
        
        if(1 << other.gameObject.layer == playerLayer){
            Debug.Log("player collided with the sphere collider");
            //other.gameObject.GetComponent<PlayerController>().Die();
        }
    }
}
