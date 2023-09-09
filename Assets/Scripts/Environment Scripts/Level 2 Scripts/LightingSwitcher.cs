using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    
    private float lightIntensity = 0.3f;
    [SerializeField] Light directionalLight;

    private float collidedXPos;


    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.name == "Body"){
            collidedXPos = other.transform.position.x;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.name == "Body"){
            if(collidedXPos != other.transform.position.x){ //the other side
                SwitchLighting();
            }
        }
    }

    private void SwitchLighting(){
        if(lightIntensity == 1f){
            lightIntensity = 0.3f;
            RenderSettings.ambientIntensity = lightIntensity;
            directionalLight.intensity = 0f;

        }
        else{
            lightIntensity = 1f;
            RenderSettings.ambientIntensity = lightIntensity;
            directionalLight.intensity = 1f;

        }
    }
}
