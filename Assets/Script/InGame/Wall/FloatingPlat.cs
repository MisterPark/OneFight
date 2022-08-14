using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlat : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Vector2 endPosition;
    Vector2 startPosition;
    bool toStart = false;

    public float Speed { get { return speed; } set { speed = value; } }
    public Vector3 Direction { get; private set; }

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        Vector3 sourcePosition;
        Vector3 destPosition;
        if(toStart)
        {
            sourcePosition = endPosition;
            destPosition = startPosition;
        }
        else
        {
            sourcePosition = startPosition;
            destPosition = endPosition;
        }

        Vector3 to = destPosition - sourcePosition;
        Direction = to.normalized;
        transform.position += Direction * speed * Time.deltaTime;
        float dist = (transform.position - new Vector3(destPosition.x, destPosition.y)).magnitude;
        if (dist < 0.01f)
        {
            toStart = !toStart;
            transform.position = destPosition;
        }
    }


}
