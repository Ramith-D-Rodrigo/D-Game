using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementRecorder : MonoBehaviour
{
    // Start is called before the first frame update


    //vector3 is position, quaternion is rotation
    //float is player health
    //bool is player dead status
    private Dictionary<GameObject, Stack<(Vector3, Quaternion, float, bool)>> bodyPartTransformations; 
    public Dictionary<GameObject, Stack<(Vector3, Quaternion, float, bool)>> BodyPartTransformations { get { return bodyPartTransformations; } }

    [SerializeField] private PlayerCollision playerCollision;
    [SerializeField] private GameObject[] playerBodyGameObjects;

    public float maxRecordingSecs = 6f;
    private bool isResetting;
    public bool IsResetting {get { return isResetting; }  set { isResetting = value; } }

    void Start()
    {
        InitializeDictionary();
        isResetting = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isResetting){
            AddBodyPartCurrentTransformation();
        }

    }

    public void RewindPlayerLastTransformation(){
        foreach(GameObject bodyPart in playerBodyGameObjects){
            if(bodyPartTransformations[bodyPart].Count > 0){
                (Vector3, Quaternion, float, bool) lastTransformation = bodyPartTransformations[bodyPart].Pop();
                bodyPart.transform.localPosition = lastTransformation.Item1;
                bodyPart.transform.localRotation = lastTransformation.Item2;
                playerCollision.PlayerHealth = lastTransformation.Item3;
                playerCollision.PlayerPrevState = lastTransformation.Item4; 
            }
            else{
                isResetting = false;
            }
        }
    }

    public (Vector3, Quaternion, float, bool) GetLastTransformationForBodyPart(GameObject bodyPart){
        return bodyPartTransformations[bodyPart].Pop();
    }

    private void InitializeDictionary(){
        bodyPartTransformations = new Dictionary<GameObject, Stack<(Vector3, Quaternion, float, bool)>>();

        foreach(GameObject bodyPart in playerBodyGameObjects){
            Stack<(Vector3, Quaternion, float, bool)> bodyPartTransformation = new();
            bodyPartTransformation.Push((bodyPart.transform.localPosition, bodyPart.transform.localRotation, playerCollision.PlayerHealth, playerCollision.IsPlayerDead));

            bodyPartTransformations.Add(bodyPart, bodyPartTransformation);
        }

    }

    public void ClearBodyPartPositionStacks(){
        foreach(GameObject bodyPart in playerBodyGameObjects){
            bodyPartTransformations[bodyPart].Clear();
        }
    }


    private void AddBodyPartCurrentTransformation(){
        foreach(GameObject bodyPart in playerBodyGameObjects){
            bodyPartTransformations[bodyPart].Push((bodyPart.transform.localPosition, bodyPart.transform.localRotation, playerCollision.PlayerHealth, playerCollision.IsPlayerDead));
        }
    }
}
