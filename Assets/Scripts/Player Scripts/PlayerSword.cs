using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Light pointLight;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchOnLight(){
        pointLight.enabled = true;
    }

    public void SwitchOffLight(){
        pointLight.enabled = false;
    }
}
