using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMask : MonoBehaviour
{
    // Start is called before the first frame update
    private int maskIndex;  //to identify the mask prefab
    public int MaskIndex{
        get{
            return maskIndex;
        }
        set{
            maskIndex = value;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
