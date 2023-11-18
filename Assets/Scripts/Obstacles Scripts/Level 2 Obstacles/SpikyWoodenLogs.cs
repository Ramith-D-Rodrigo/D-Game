using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikyWoodenLogs : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float maxMoveDistance;
    private int moveDirection = -1; //move back

    [SerializeField] private float moveSpeed;

    [SerializeField] private Rigidbody[] movableObjects;

    [SerializeField] private Transform maskInsertBox;

    private bool canMove;
    public bool CanMove{
        get{
            return canMove;
        }
        set{
            canMove = value;
        }
    }

    private Material finalMaskMaterial;
    public Material FinalMaskMaterial{
        get{
            return finalMaskMaterial;
        }
    }

    private GameObject instantiatedMask;
    public GameObject InstantiatedMask{
        get{
            return instantiatedMask;
        }
    }

    private int maskIndex;
    public int MaskIndex{
        get{
            return maskIndex;
        }
        set{
            maskIndex = value;
        }
    }

    void Start()
    {
        //MoveTheWoodenLogs();
        canMove = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!canMove){
            return;
        }

        //get the current position of the movable object in the x axis 
        float currXPos = movableObjects[0].position.x;

        //get the distance between the current position and the starting position
        float distance = this.transform.position.x - currXPos;


        if(distance < 0.0f){ //if the movable object has reached the end
            moveSpeed = 0.0f;   //stop moving
            canMove = false;
        }
        
        MoveTheWoodenLogs();
    }

    public void InsertMask(GameObject maskPrefab, int maskIndex, Material defaultMaterial){
        instantiatedMask = Instantiate(maskPrefab, maskInsertBox.position, maskInsertBox.rotation, maskInsertBox);
        finalMaskMaterial = instantiatedMask.GetComponent<Renderer>().material;
        //remove the material 
        instantiatedMask.GetComponent<Renderer>().material = null;

        //add the default material
        instantiatedMask.GetComponent<Renderer>().material = defaultMaterial;
        //set the mask index
        this.maskIndex = maskIndex;
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
