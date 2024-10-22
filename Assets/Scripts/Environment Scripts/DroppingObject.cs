using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppingObject : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;

    private void Awake(){
        if(this.tag == "EnemyMask"){
            this.enabled = false;
        }
    }
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
        else if(this.gameObject.tag == "Compass"){
            rb = this.gameObject.AddComponent<Rigidbody>();
            this.transform.GetChild(0).GetComponent<MeshCollider>().enabled = true;
        }
        else if(this.gameObject.tag == "Sword"){
            rb = this.gameObject.AddComponent<Rigidbody>();
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            
            GameObject swordMesh = this.transform.GetChild(0).gameObject; //1st child is the sword mesh
            PlayerSword playerSword = swordMesh.GetComponent<PlayerSword>();
            playerSword.enabled = false;

            BoxCollider longer = swordMesh.GetComponent<BoxCollider>(); //longer one
            BoxCollider shorter = swordMesh.transform.GetChild(0).GetComponent<BoxCollider>();   //short horizontal one

            longer.isTrigger = false;
            shorter.isTrigger = false;
        }
        else if(this.gameObject.tag == "EnemyMask"){
            rb = this.gameObject.AddComponent<Rigidbody>();
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            this.gameObject.GetComponent<SphereCollider>().enabled = false;
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
            else if(this.gameObject.tag == "Compass"){
                //re enable mesh collider
                this.transform.GetChild(0).GetComponent<MeshCollider>().enabled = false;
                GetComponent<SphereCollider>().enabled = true;

                Compass compass = GetComponent<Compass>();
                compass.SwitchOnLight();
                compass.SwitchOnCanvas();

                //then remove the rigidbody
                Destroy(this.rb, 0);
            }
            else if(this.gameObject.tag == "Sword"){
                //re enable sphere collider
                GameObject swordMesh = this.transform.GetChild(0).gameObject; //1st child is the sword mesh
                BoxCollider longer = swordMesh.GetComponent<BoxCollider>(); //longer one
                BoxCollider shorter = swordMesh.transform.GetChild(0).GetComponent<BoxCollider>();   //short horizontal one
                longer.isTrigger = true;
                shorter.isTrigger = true;

                longer.enabled = false;
                shorter.enabled = false;

                GetComponent<SphereCollider>().enabled = true;
                swordMesh.GetComponent<PlayerSword>().SwitchOnLight();

                //then remove the rigidbody
                Destroy(this.rb, 0);
            }
            else if(this.gameObject.tag == "EnemyMask"){
                //re enable sphere collider
                GetComponent<SphereCollider>().enabled = true;

                //then remove the rigidbody
                Destroy(this.rb, 0);
            }

            //disable the script
            this.enabled = false;
        }
    }
}
