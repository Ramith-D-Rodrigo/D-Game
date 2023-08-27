using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideObstacleSpeedSetter : MonoBehaviour
{
    // Start is called before the first frame update
    public float minSpeed = 30f;
    public float maxSpeed = 65f;

    List<float> xPositions = new();

    float xMin = Mathf.Infinity, xMax = -Mathf.Infinity;

    void Awake()
    {
        SlideObstacleMover[] childSlideObstacleMovers;
        SlidingDownObstacleMover[] childSlidingDownObstacleMovers; 



        //first get all the children obstacles
        if(this.gameObject.name == "Sliding Obstacles"){
            childSlideObstacleMovers = this.GetComponentsInChildren<SlideObstacleMover>();
            
            GetXPositions(childSlideObstacleMovers);

            Debug.Log(xMax + " " + xMin);

            SetSpeedTotheComponent(childSlideObstacleMovers);
        }
        else if(this.gameObject.name == "Sliding Down Obstacles"){
            childSlidingDownObstacleMovers = this.GetComponentsInChildren<SlidingDownObstacleMover>();

            GetXPositions(childSlidingDownObstacleMovers);

            Debug.Log(xMax + " " + xMin);

            SetSpeedTotheComponent(childSlidingDownObstacleMovers);
        }
        else{
            return;
        }
    }

    void GetXPositions(SlideObstacleMover[] moverComponentArr){
        for(int i = 0; i < moverComponentArr.Length; i++){
            float xPos = moverComponentArr[i].gameObject.transform.position.x;

            SetMinMaxOfXPositions(xPos);

            xPositions.Add(xPos);
        }
    }
    void GetXPositions(SlidingDownObstacleMover[] moverComponentArr){
        for(int i = 0; i < moverComponentArr.Length; i++){
            float xPos = moverComponentArr[i].gameObject.transform.position.x;
            Debug.Log(xPos);

            SetMinMaxOfXPositions(xPos);

            xPositions.Add(xPos);
        }
    }

    void SetSpeedTotheComponent(SlidingDownObstacleMover[] moverComponentArr){
        for(int i = 0; i < xPositions.Count; i++){
            float speed = Mapper.Map(xPositions[i], xMax, xMin, minSpeed, maxSpeed); //switch min and max because min x position is far away (towards the end)
            moverComponentArr[i].MoveSpeed = speed;
        }
    }

    void SetSpeedTotheComponent(SlideObstacleMover[] moverComponentArr){
        for(int i = 0; i < xPositions.Count; i++){
            float speed = Mapper.Map(xPositions[i], xMax, xMin, minSpeed, maxSpeed); //switch min and max because min x position is far away (towards the end)
            moverComponentArr[i].MoveSpeed = speed;
        }
    }

    void SetMinMaxOfXPositions(float xPos){
        if(xPos < xMin){
            Debug.Log(xPos + " is lesser than " + xMin);
            xMin = xPos;
        }
        
        if(xPos > xMax){
            xMax = xPos;
        }
    }
}
