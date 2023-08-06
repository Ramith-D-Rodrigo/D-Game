using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppingObject : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    void Start()
    {
        if(this.gameObject.tag == "Hammer"){
            //add a rigidbody, remove mesh collider's is trigger
            rb = this.gameObject.AddComponent<Rigidbody>();
            this.GetComponent<MeshCollider>().isTrigger = false;
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        //check the rigidbody is sleeping or not
        if(rb.IsSleeping()){
            if(this.gameObject.tag == "Hammer"){
                Destroy(this.GetComponent<MeshCollider>(), 0);
                SphereCollider sc = this.gameObject.AddComponent<SphereCollider>();
                sc.isTrigger = true;
                sc.radius = 0.39f;
                sc.center.Set(0.0535097f, 4.78414e-09f, 0.18f);

                //then remove the rigidbody
                Destroy(this.rb, 0);
                //then remove the script itself
                Destroy(this, 0);
            }
        }
    }
}
