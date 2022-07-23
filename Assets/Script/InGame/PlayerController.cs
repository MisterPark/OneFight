using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;
    SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * _speed * Time.deltaTime;
            _spriteRenderer.flipX = false;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * _speed * Time.deltaTime;
            _spriteRenderer.flipX = true;
        }
    }
}
