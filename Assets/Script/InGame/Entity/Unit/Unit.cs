using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Unit : Entity
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpPower;

    [SerializeField] private Vector3 hitTarget;
    [SerializeField] private GameObject hitBox;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationEvents animationEvents;
    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private MaterialProperty materialProperty;

    [SerializeField] private int moveSpeedHash;
    [SerializeField] private int velocityYHash;
    [SerializeField] private int isJumpHash;
    [SerializeField] private int comboHash;
    [SerializeField] private int isStiffHash;
    [SerializeField] private int stiffnessDurationHash;
    [SerializeField] private int downHash;

    // Move
    private Vector3 prevDirection = Vector3.right;
    private Vector3 currentDirection = Vector3.right;
    private float currentSpeed = 0f;
    private bool moveFlag = true;
    public bool IsMove { get; set; } = false;
    public bool IsJump { get; set; } = false;
    public bool IsAttack { get; set; } = false;
    public float JumpPower => jumpPower;

    public bool IsLeft { get { return currentDirection.x < 0; } }

    // Combo
    public bool AttackFlag { get; set; } = true;

    private int combo = 0;
    private bool comboFlag = true;
    private int maxCombo = 4;

    private bool hitBoxFlag = false;

    // Stiffness & Knockback
    private bool isStiff = false;
    private float stiffnessTick = 0f;
    private float stiffnessDuration = 0.5f;
    private float knockbackPower = 0.2f;
    private Vector3 knockbackDirection = Vector3.zero;
    // Down
    private bool downFlag = false;
    private bool isDown = false;

    public Team Team { get; set; }

    // Stat
    private float hp = 100f;
    private float damage = 1f;


    private void OnValidate()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (animator == null) animator = GetComponent<Animator>();
        if (rigid == null) rigid = GetComponent<Rigidbody2D>();
        if (animationEvents == null) animationEvents = GetComponent<AnimationEvents>();
        if(materialProperty == null) materialProperty = GetComponent<MaterialProperty>();

        moveSpeedHash = Animator.StringToHash("MoveSpeed");
        velocityYHash = Animator.StringToHash("VelocityY");
        isJumpHash = Animator.StringToHash("IsJump");
        comboHash = Animator.StringToHash("Combo");
        isStiffHash = Animator.StringToHash("IsStiff");
        stiffnessDurationHash = Animator.StringToHash("StiffnessDuration");
        downHash = Animator.StringToHash("IsDown");
    }

    private void Start()
    {
        animationEvents.Events[AnimationState.Attack01].OnEnter.AddListener(OnAttackStart);
        animationEvents.Events[AnimationState.Attack01].OnEnd.AddListener(OnAttackEnd);
        animationEvents.Events[AnimationState.Attack01].OnOverHalf.AddListener(OnAttackOverHalf);
        animationEvents.Events[AnimationState.Attack02].OnEnter.AddListener(OnAttackStart);
        animationEvents.Events[AnimationState.Attack02].OnEnd.AddListener(OnAttackEnd);
        animationEvents.Events[AnimationState.Attack02].OnOverHalf.AddListener(OnAttackOverHalf);
        animationEvents.Events[AnimationState.Attack03].OnEnter.AddListener(OnAttackStart);
        animationEvents.Events[AnimationState.Attack03].OnEnd.AddListener(OnAttackEnd);
        animationEvents.Events[AnimationState.Attack03].OnOverHalf.AddListener(OnAttackOverHalf);
        animationEvents.Events[AnimationState.Attack04].OnEnter.AddListener(OnAttackStart);
        animationEvents.Events[AnimationState.Attack04].OnEnd.AddListener(OnAttackEnd);
        animationEvents.Events[AnimationState.Attack04].OnOverHalf.AddListener(OnAttackOverHalf);
        animationEvents.Events[AnimationState.Down].OnEnter.AddListener(OnDownStart);
        animationEvents.Events[AnimationState.Down].OnExit.AddListener(OnDownExit);
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
        ProcessHit();
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
        if (isDown) return;
        if (isStiff) return;
        if (!moveFlag) return;
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
        spriteRenderer.flipX = IsLeft;
    }

    public void Jump()
    {
        if (isDown) return;
        if (isStiff) return;
        IsJump = true;
        gameObject.layer = LayerMask.NameToLayer("Jump");
        AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
    }

    public void Attack()
    {
        if (isDown) return;
        if (isStiff) return;
        if (IsJump) return;
        if (!AttackFlag) return;

        if (comboFlag)
        {
            comboFlag = false;
            if (combo < maxCombo)
            {
                combo++;
                moveFlag = false;
            }
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
        animator.SetFloat(stiffnessDurationHash, stiffnessDuration);
        if(downFlag)
        {
            downFlag = false;
            animator.SetTrigger(downHash);
        }
        if (isStiff)
        {
            animator.SetTrigger(isStiffHash);
        }
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

    private void ProcessHit()
    {
        if (isStiff)
        {
            stiffnessTick += Time.deltaTime;
            if (stiffnessTick > stiffnessDuration)
            {
                isStiff = false;
                moveFlag = true;
                AttackFlag = true;
                stiffnessTick = 0f;
                return;
            }
        }

        if(isStiff || isDown)
        {
            var power = isDown ? knockbackPower * 2f : knockbackPower;
            transform.position += power * knockbackDirection * Time.deltaTime;
        }

    }
    public void OnAttackStart()
    {
        hitBoxFlag = true;
        comboFlag = false;
    }

    public void OnAttackOverHalf()
    {
        comboFlag = true;
        if (hitBoxFlag)
        {
            hitBoxFlag = false;
            var curHitTarget = hitTarget;
            curHitTarget.x *= IsLeft ? -1f : 1f;
            var attackType = combo == 4 ? AttackType.Down : AttackType.Normal;
            CreateHitBox(transform.position + curHitTarget, attackType);
        }
    }

    public void OnAttackEnd()
    {
        combo = 0;
        comboFlag = true;
        moveFlag = true;
    }

    public void OnDownStart()
    {

    }

    public void OnDownExit()
    {
        Debug.Log("Down End");
        isDown = false;
        moveFlag = true;
        AttackFlag = true;
    }

    public void CreateHitBox(Vector3 target, AttackType attackType)
    {
        GameObject hitBoxObj = Instantiate(hitBox);
        hitBoxObj.transform.position = target;
        var box = hitBoxObj.GetComponent<HitBox>();
        if (box != null)
        {
            box.Owner = this;
            box.Team = Team;
            box.Damage = damage;
            box.AttackType = attackType;
        }
    }

    public void OnHit(float damage, Unit beater, AttackType attackType)
    {
        switch (attackType)
        {
            case AttackType.Normal:
                isStiff = true;
                stiffnessTick = 0f;
                break;
            case AttackType.Down:
                downFlag = true;
                isDown = true;
                break;
            default:
                break;
        }

        
        moveFlag = false;
        AttackFlag = false;
        this.knockbackDirection = (transform.position - beater.transform.position).normalized;
        materialProperty.OnHit = true;

        hp -= damage;
        if (hp <= 0f)
        {
            hp = 0f;
            Destroy(gameObject);
        }
    }
}
