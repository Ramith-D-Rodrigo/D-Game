using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Light[] pointLights;

    [SerializeField] private PlayerUseObject playerUseObject;

    private string[] enemyBodyParts = {"EnemyHead", "EnemyBody", "EnemyArm", "EnemyLeg"};
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Collision with" + other.gameObject.tag);
        if(enemyBodyParts.Contains(other.gameObject.tag) && other.transform.parent.tag == "Enemy" && playerUseObject.GetIsUsingObject()){
              other.transform.parent.GetComponent<EnemyCollision>().HitPoints--;
        }
    }

    public void SwitchOnLight(){
        pointLights[0].enabled = true;
        pointLights[1].enabled = true;
    }

    public void SwitchOffLight(){
        pointLights[0].enabled = false;
        pointLights[1].enabled = false;
    }
}
