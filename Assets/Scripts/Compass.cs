using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject destination;
    [SerializeField] GameObject pointerPivot;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pointerPivot.transform.LookAt(destination.transform);
    }
}
