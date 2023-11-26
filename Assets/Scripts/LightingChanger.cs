using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingChanger : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 destination;
    private Vector3 playerStartPosition;

    private Transform player;

    [SerializeField] private Light directionalLight;
    void Start()
    {
        LevelManager levelManager = GameObject.FindAnyObjectByType<LevelManager>();
        destination = levelManager.GetDestination();

        player = GameObject.FindObjectOfType<PlayerCollision>().transform;

        playerStartPosition = player.position;
    }

    // Update is called once per frame
    void Update()
    {
        directionalLight.intensity = Mapper.Map(player.position.x, playerStartPosition.x, destination.x, 0f, 1f);
        RenderSettings.ambientIntensity = Mapper.Map(player.position.x, playerStartPosition.x, destination.x, 0.3f, 1f);
    }
}
