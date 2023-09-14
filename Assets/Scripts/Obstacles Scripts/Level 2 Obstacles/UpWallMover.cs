using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpWallMover : MonoBehaviour
{
    // Start is called before the first frame update
    private float moveSpeed = 25f;
    private float stoppingYPos = 11f;
    private bool isClosed;
    private bool isClosing;
    private Rigidbody rb;
    void Start()
    {
        isClosed = false;
        isClosing = false;
        rb = this.GetComponent<Rigidbody>();
    }

    void Update(){
        if(isClosing){
            CloseThePath();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.name == "Body" && !isClosed){
            isClosing = true;
        }
    }

    private void CloseThePath(){
        //yield return new WaitForSeconds(1f);
        rb.transform.position = Vector3.MoveTowards(rb.position,new Vector3(rb.position.x, stoppingYPos, rb.position.z), Time.deltaTime * moveSpeed);
        //rb.MovePosition();
        if(rb.transform.position.y >= stoppingYPos){
            isClosed = true;
            isClosing = false;
        }
    }
}
