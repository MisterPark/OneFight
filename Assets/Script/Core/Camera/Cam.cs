using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if(Player.Self != null)
        {
            var playerPosition = Player.Self.transform.position;
            transform.position = new Vector3(playerPosition.x, playerPosition.y +0.5f, transform.position.z);
        }
    }
}
