using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _maxSpeed;


    SpriteRenderer _spriteRenderer;
    Animator _animator;

    private Vector3 _currentDirection = Vector3.right;
    private float _currentSpeed = 0f;
    private bool _isMove = false;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        _isMove = false;

        if(Input.GetKey(KeyCode.RightArrow))
        {
            Move(Vector3.right);

        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            Move(Vector3.left);
        }

        if(_isMove == false)
        {
            _currentSpeed = _currentSpeed * 0.5f;
            _currentSpeed = Mathf.Clamp(_currentSpeed, 0f, _maxSpeed);
        }

        _animator.SetFloat("MoveSpeed", _currentSpeed);
    }

    private void Move(Vector3 direction)
    {
        _isMove = true;
        _currentSpeed += Time.deltaTime;
        _currentSpeed = Mathf.Clamp(_currentSpeed, 0f, _maxSpeed);
        _currentDirection = direction;
        transform.position += _currentDirection * _currentSpeed * Time.deltaTime;
        
        _spriteRenderer.flipX = _currentDirection == Vector3.left ? true : false;
    }
}
