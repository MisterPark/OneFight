using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Entity
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpPower;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationEvents animationEvents;
    [SerializeField] private Rigidbody2D rigid;

    [SerializeField] private int moveSpeedHash;
    [SerializeField] private int velocityYHash;
    [SerializeField] private int isJumpHash;
    [SerializeField] private int comboHash;

    private Vector3 prevDirection = Vector3.right;
    private Vector3 currentDirection = Vector3.right;
    private float currentSpeed = 0f;
    public bool IsMove { get; set; } = false;
    public bool IsJump { get; set; } = false;
    public bool IsAttack { get; set; } = false;
    public float JumpPower => jumpPower;

    private int combo = 0;
    private int prevCombo = 0;
    private float comboDelay = 0.01f;
    private float comboTick = 0f;
    private bool comboFlag = true;

    private void OnValidate()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (animator == null) animator = GetComponent<Animator>();
        if (rigid == null) rigid = GetComponent<Rigidbody2D>();
        if(animationEvents == null) animationEvents = GetComponent<AnimationEvents>();

        moveSpeedHash = Animator.StringToHash("MoveSpeed");
        velocityYHash = Animator.StringToHash("VelocityY");
        isJumpHash = Animator.StringToHash("IsJump");
        comboHash = Animator.StringToHash("Combo");
    }

    private void Start()
    {
        animationEvents.Events[UnitState.Attack01].OnEnter.AddListener(OnAttack01Start);
        animationEvents.Events[UnitState.Attack01].OnExit.AddListener(OnAttack01End);
        animationEvents.Events[UnitState.Attack02].OnEnter.AddListener(OnAttack02Start);
        animationEvents.Events[UnitState.Attack02].OnExit.AddListener(OnAttack02End);
        animationEvents.Events[UnitState.Attack03].OnEnter.AddListener(OnAttack03Start);
        animationEvents.Events[UnitState.Attack03].OnExit.AddListener(OnAttack03End);
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

        ProcessAttack();
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
        if (combo != 0) return;
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

    public void Attack()
    {
        if(comboFlag)
        {
            combo++;
        }
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
        animator.SetInteger(comboHash, combo);
        animator.SetFloat(moveSpeedHash, currentSpeed);
        animator.SetFloat(velocityYHash, rigid.velocity.y);
        animator.SetBool(isJumpHash, IsJump);
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

    private void ProcessAttack()
    {
        if (combo > 3) combo = 0;
    }

    public void OnAttack01Start()
    {
    }

    public void OnAttack01End()
    {
        if(combo == 1)
        {
            combo = 0;
        }
    }

    public void OnAttack02Start()
    {
    }

    public void OnAttack02End()
    {
        if(combo == 2)
        {
            combo = 0;
        }
    }

    public void OnAttack03Start()
    {
    }

    public void OnAttack03End()
    {
        combo = 0;
    }
}
