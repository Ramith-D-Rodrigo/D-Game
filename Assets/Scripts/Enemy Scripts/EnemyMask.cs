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

    private GameObject maskInsertingBox;
    public GameObject MaskInsertingBox{
        get{
            return maskInsertingBox;
        }
        set{
            maskInsertingBox = value;
        }
    }
    void Start()
    {
        maskInsertingBox = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InsertMaskToBox(){
        MaskInsertionHandler maskInsertionHandler = maskInsertingBox.GetComponent<MaskInsertionHandler>();

        maskInsertionHandler.InsertMask(this.gameObject, maskIndex);
    }
}
