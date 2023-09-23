using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject destination;
    [SerializeField] GameObject pointerPivot;

    [SerializeField] Light[] pointLights;

    [SerializeField] Canvas locationPinCanvas;

    private bool isWorking; //to check whether the compass should work or not
    public bool IsWorking {get {return isWorking;} set {isWorking = value;} }
    void Start()
    {
        isWorking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isWorking){
            pointerPivot.transform.LookAt(destination.transform);
        }
    }


    public void SwitchOnLight(){
        foreach(Light light in pointLights){
            light.enabled = true;
        }
    }

    public void SwitchOffLight(){
        foreach(Light light in pointLights){
            light.enabled = false;
        }
    }

    public void SwitchOnCanvas(){
        locationPinCanvas.enabled = true;
    }

    public void SwitchOffCanvas(){
        locationPinCanvas.enabled = false;
    }
}
