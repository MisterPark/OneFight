using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorWall : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.IsPlayer())
        {

        }
    }
}
