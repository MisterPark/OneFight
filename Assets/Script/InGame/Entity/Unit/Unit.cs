using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Entity
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpPower;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigid;

    private Vector3 prevDirection = Vector3.right;
    private Vector3 currentDirection = Vector3.right;
    private float currentSpeed = 0f;
    public bool IsMove { get; set; } = false;
    public bool IsJump { get; set; } = false;

    public float JumpPower => jumpPower;


    private void OnValidate()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (animator == null) animator = GetComponent<Animator>();
        if (rigid == null) rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        IsMove = false;
    }

    private void LateUpdate()
    {
        if (IsMove == false)
        {
            Decelerate();
        }

        ProcessJump();
        ProcessAnimation();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ProcessTouchGround(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        ProcessFloatingPlat(collision);
    }

    public void Move(Vector3 direction)
    {
        IsMove = true;
        currentSpeed = maxSpeed;
        //currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);
        prevDirection = currentDirection;
        currentDirection = new Vector3(direction.x, 0f, 0f).normalized;
        if (currentDirection != prevDirection)
        {
            Decelerate();
        }
        transform.position += currentDirection * currentSpeed * Time.deltaTime;
        spriteRenderer.flipX = currentDirection.x < 0 ? true : false;
    }

    public void AddForce(Vector3 force)
    {
        rigid.AddForce(force);
    }

    public void AddForce(Vector3 force, ForceMode2D forceMode)
    {
        rigid.AddForce(force, forceMode);
    }

    private void Decelerate()
    {
        currentSpeed = currentSpeed * 0.5f;
        currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);
    }

    private void ProcessAnimation()
    {
        animator.SetFloat("MoveSpeed", currentSpeed);
        animator.SetFloat("VelocityY", rigid.velocity.y);
        animator.SetBool("IsJump", IsJump);
    }

    private void ProcessJump()
    {
        if (rigid.velocity.y < 0f)
        {
            gameObject.layer = LayerMask.NameToLayer("Unit");
        }
    }

    private void ProcessTouchGround(Collision2D collision)
    {
        int targetMask = collision.gameObject.GetLayerMask();
        int groundMask = LayerMask.GetMask("Ground", "FloatingPlat");

        if ((targetMask & groundMask) != 0)
        {
            IsJump = false;
        }
    }

    private void ProcessFloatingPlat(Collision2D collision)
    {
        int targetMask = collision.gameObject.GetLayerMask();
        int platMask = LayerMask.GetMask("FloatingPlat");
        if ((targetMask & platMask) != 0)
        {
            var plat = collision.gameObject.GetComponent<FloatingPlat>();
            if (plat != null)
            {
                transform.position += plat.Direction * plat.Speed * Time.fixedDeltaTime;
            }
        }
    }
}
