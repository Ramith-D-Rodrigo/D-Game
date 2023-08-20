using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SlidingUpWallObstacle : MonoBehaviour
{
    private int noOfDoorBoxes;
    private List<SlidingUpWallBox> doorBoxesComponents;
    public Animator wallAnimator;
    private bool isClosed;
    private bool isAnimating;
    private int maxDoorBoxes;

    [Tooltip("horizontal range of doorboxes: x is min, y is max")]
    public Vector2 zRange;

    [Tooltip("forward range: only has max")]
    public float xRange;
    public GameObject doorBoxPrefab;
    public GameObject doorBoxParent;
    // Start is called before the first frame update
    void Start()
    {
        maxDoorBoxes = 5;
        CreateDoorBoxes();

        isClosed = true; //wall is closed
    }

    private void CreateDoorBoxes()
    {
        noOfDoorBoxes = Random.Range(1, maxDoorBoxes); //amount of door boxes
        doorBoxesComponents = new List<SlidingUpWallBox>();

        Vector3 doorBoxParentPos = doorBoxParent.transform.position;

        Vector3 doorBoxSize = doorBoxPrefab.GetComponent<MeshRenderer>().bounds.size;   //renderer size for position checking

        int inc = 0;
        while(inc < noOfDoorBoxes)
        {
            Vector3 randomPosition = new(Random.Range(0, xRange), doorBoxParentPos.y, Random.Range(zRange.x, zRange.y));
            bool overlaps = false;
            foreach(SlidingUpWallBox comp in doorBoxesComponents){
                GameObject obj = comp.gameObject;
                if(this.Overlaps(randomPosition, obj.transform.localPosition, doorBoxSize)){ //has to find a new position
                    overlaps = true;
                    break;
                }
            }
            if(overlaps){
                continue;
            }
            else{
                inc++;
            }

            GameObject newObj = Instantiate(doorBoxPrefab, doorBoxParent.transform);

            newObj.transform.localPosition = randomPosition;
            doorBoxesComponents.Add(newObj.GetComponent<SlidingUpWallBox>());
        }
    }

    private bool Overlaps(Vector3 pos1, Vector3 pos2, Vector3 size){
        float zMin1 = pos1.z - size.z / 2;
        float zMax1 = pos1.z + size.z / 2;

        float zMin2 = pos2.z - size.z / 2;
        float zMax2 = pos2.z + size.z / 2;

        float xMin1 = pos1.x - size.x / 2;
        float xMax1 = pos1.x + size.x / 2;

        float xMin2 = pos2.x - size.x / 2;
        float xMax2 = pos2.x + size.x / 2;

        // Check for non-overlapping in the Z-axis
        if (zMax1 < zMin2 || zMax2 < zMin1) {
            return false;
        }

        // Check for non-overlapping in the X-axis
        if (xMax1 < xMin2 || xMax2 < xMin1) {
            return false;
        }

        // If there is no non-overlapping condition met, the rectangles overlap.
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        bool canOpen = true;
        foreach(SlidingUpWallBox component in doorBoxesComponents){
            if(!component.MassFullFilled){
                canOpen = false;
                break;
            }
        }

        if(canOpen && isClosed){
            SlideDownWall();
        }
        else if(!canOpen && !isClosed){
            SlideUpWall();
        }
    }

    private void SlideUpWall()
    {
        wallAnimator.SetTrigger("SlideUp");
        StartCoroutine(StopAnimation(true));
    }
    private void SlideDownWall()
    {
        wallAnimator.SetTrigger("SlideDown");
        StartCoroutine(StopAnimation(false));
    }

    private IEnumerator StopAnimation(bool upState) //upstate means going up (true)
    {
        AnimatorStateInfo animatorStateInfo;
        do{
            animatorStateInfo = wallAnimator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForEndOfFrame();
        }
        while(animatorStateInfo.normalizedTime % 1.0f < 0.9f);

        if(upState){ //wall has gone up
            isClosed = true;
        }
        else{ //wall has gone down
            isClosed = false;
        }
    }
}
