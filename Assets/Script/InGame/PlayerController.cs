using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _jumpPower;

    SpriteRenderer _spriteRenderer;
    Animator _animator;
    Rigidbody2D _rigidbody;

    private Vector3 _prevDirection = Vector3.right;
    private Vector3 _currentDirection = Vector3.right;
    private float _currentSpeed = 0f;
    private bool _isMove = false;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();

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

        if(Input.GetKeyDown(KeyCode.UpArrow) && _rigidbody.velocity.y < 0.01f && _rigidbody.velocity.y > -0.01f)
        {
            _rigidbody.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
        }

        if(_isMove == false)
        {
            Decelerate();
        }

        ProcessAnimation();
    }

    private void Move(Vector3 direction)
    {
        _isMove = true;
        _currentSpeed += Time.deltaTime;
        _currentSpeed = Mathf.Clamp(_currentSpeed, 0f, _maxSpeed);
        _prevDirection = _currentDirection;
        _currentDirection = direction;
        if(_currentDirection != _prevDirection)
        {
            Decelerate();
        }
        transform.position += _currentDirection * _currentSpeed * Time.deltaTime;
        
        _spriteRenderer.flipX = _currentDirection == Vector3.left ? true : false;
    }

    private void Decelerate()
    {
        _currentSpeed = _currentSpeed * 0.5f;
        _currentSpeed = Mathf.Clamp(_currentSpeed, 0f, _maxSpeed);
    }

    private void ProcessAnimation()
    {
        _animator.SetFloat("MoveSpeed", _currentSpeed);
        _animator.SetFloat("VelocityY", _rigidbody.velocity.y);
    }
}
