using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingManager : MonoBehaviour
{
    [SerializeField] private Light directionalLight;
    [SerializeField] private float sunSpeed;
    [SerializeField, Range(0, 24)] private float timeOfDay;

    [SerializeField] private Gradient skyColor;
    [SerializeField] private Gradient equatorColor;
    [SerializeField] private Gradient directionalColor;   //the sun
    private void OnValidate() {
        UpdateLighting(timeOfDay / 24f);
    }

    void Start()
    {
        timeOfDay = 2f; //start at 2am
    }

    // Update is called once per frame
    void Update()
    {
        timeOfDay += Time.deltaTime * sunSpeed;
        timeOfDay %= 24; //modulus to keep it between 0 and 24
        UpdateLighting(timeOfDay / 24f);
    }

    private void UpdateLighting(float timePercent){
        RenderSettings.ambientEquatorColor = equatorColor.Evaluate(timePercent);
        RenderSettings.ambientSkyColor = skyColor.Evaluate(timePercent);

        if(directionalLight != null){
            directionalLight.color = directionalColor.Evaluate(timePercent);
            directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, -170f, 0)); //rotate the light around the x axis
        }
    }
}
