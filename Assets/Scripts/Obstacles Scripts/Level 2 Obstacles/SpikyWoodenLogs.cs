using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikyWoodenLogs : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float maxMoveDistance;

    [Tooltip("+1 or -1")]
    [SerializeField] private int moveDirection;

    [SerializeField] private float moveSpeed;

    [SerializeField] private Rigidbody[] movableObjects;

    [SerializeField] private GameObject[] maskPrefabs;

    [SerializeField] private Transform maskInsertBox;

    void Start()
    {
        //MoveTheWoodenLogs();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
/*         //reverse the direction if the movable object has reached the max distance

        //get the current position of the movable object in the x axis 
        float currXPos = movableObjects[0].position.x;

        //get the distance between the current position and the starting position
        float distance = this.transform.position.x - currXPos;


        if(distance >= maxMoveDistance || distance < 0.0f){ //if the movable object has reached the max distance
            moveDirection *= -1;    //reverse the direction
        }
        
        MoveTheWoodenLogs(); */
    }

    public void InsertMask(GameObject maskPrefab){
        GameObject mask = Instantiate(maskPrefab, maskInsertBox.position, maskInsertBox.rotation, maskInsertBox);
    }

    private void MoveTheWoodenLogs(){
        foreach(Rigidbody movableObject in movableObjects){
            //use move position to move the object
            Vector3 target = movableObject.position + movableObject.transform.up * Time.deltaTime * moveSpeed * moveDirection;

            //Debug.Log("target: " + target + " movable object position: " + movableObject.position);

            movableObject.MovePosition(target);
        }
    }
}
