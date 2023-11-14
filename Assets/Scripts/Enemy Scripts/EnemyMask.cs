using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMask : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform maskPlaceHolder;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InsertMask(GameObject maskPrefab){
        GameObject mask = Instantiate(maskPrefab, maskPlaceHolder.position, maskPlaceHolder.rotation, maskPlaceHolder);
    }
}
