using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppingObject : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    void OnEnable()
    {
        this.transform.SetParent(null, true);
        
        if(this.gameObject.tag == "Hammer"){
            //add a rigidbody, remove mesh collider's is trigger
            rb = this.gameObject.AddComponent<Rigidbody>();
            this.GetComponent<MeshCollider>().isTrigger = false;
        }
        else if(this.gameObject.tag == "WallBrick"){
            rb = this.GetComponent<Rigidbody>();
            rb.isKinematic = false;
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        //check the rigidbody is sleeping or not
        if(rb.IsSleeping()){
            if(this.gameObject.tag == "Hammer"){

                //re enable sphere collider
                GetComponent<SphereCollider>().enabled = true;

                //disable mesh collider
                MeshCollider mc = GetComponent<MeshCollider>();
                mc.isTrigger = true;
                mc.enabled = false;

                //then remove the rigidbody
                Destroy(this.rb, 0);
            }
            else if(this.gameObject.tag == "WallBrick"){

                //re enable sphere collider
                GetComponent<SphereCollider>().enabled = true;

            }

            //disable the script
            this.enabled = false;
        }
    }
}