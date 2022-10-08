using System.Collections.Generic;
using UnityEngine;

public class OnHitProcedure
{
    public Vector3 HitPoint;
    public float Damage { get; set; }
    public Unit Unit { get; set; }
    public AttackType AttackType { get; set; }
}
public partial class Unit : Entity
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpPower;

    [SerializeField] private Vector3 hitTarget;

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
    [SerializeField] private int guardHash;
    [SerializeField] private int isDeadHash;
    [SerializeField] private int skill01Hash;

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
    // Guard
    private bool isGuard = false;
    public bool IsGuard { get { return isGuard; } }

    // Combo
    public bool AttackFlag { get; set; } = true;

    private int combo = 0;
    private int prevCombo = 0;
    private float comboDelay = 0.3f;
    private float maxComboDelay = 0.5f;
    private float comboTick = 0f;
    private bool comboFlag = true;
    private int maxCombo = 4;

    private bool hitBoxFlag = false;

    // Stiffness & Knockback
    private bool isStiff = false;
    private float stiffnessTick = 0f;
    private float stiffnessDuration = 0.5f;
    private float knockbackPower = 0.2f;
    private Vector3 knockbackDirection = Vector3.zero;
    private float knockbackTick = 0f;
    private float knockbackDuration = 0.5f;
    private bool knockbackFlag = false;
    // Down
    private bool downFlag = false;
    private bool isDown = false;

    public Team Team { get; set; }

    // OnHit
    private List<OnHitProcedure> onHitProcedures = new List<OnHitProcedure>();

    // Stat
    private float hp = 10f;
    private float damage = 1f;
    public float Damage => damage;

    // Death
    private bool isDead = false;
    public bool IsDead => isDead;

    // Skill
    Skill[] skills = new Skill[4];
    public bool isSkill01 = false;


    private void OnValidate()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (animator == null) animator = GetComponent<Animator>();
        if (rigid == null) rigid = GetComponent<Rigidbody2D>();
        if (animationEvents == null) animationEvents = GetComponent<AnimationEvents>();
        if (materialProperty == null) materialProperty = GetComponent<MaterialProperty>();

        moveSpeedHash = Animator.StringToHash("MoveSpeed");
        velocityYHash = Animator.StringToHash("VelocityY");
        isJumpHash = Animator.StringToHash("IsJump");
        comboHash = Animator.StringToHash("Combo");
        isStiffHash = Animator.StringToHash("IsStiff");
        stiffnessDurationHash = Animator.StringToHash("StiffnessDuration");
        downHash = Animator.StringToHash("IsDown");
        guardHash = Animator.StringToHash("IsGuard");
        isDeadHash = Animator.StringToHash("IsDead");
        skill01Hash = Animator.StringToHash("Skill01");
    }

    private void Start()
    {
        animationEvents.Events[AnimationState.Attack01].OnEnter.AddListener(OnAttackStart);
        animationEvents.Events[AnimationState.Attack01].OnEnd.AddListener(OnAttackEnd);
        animationEvents.Events[AnimationState.Attack01].OnOverHalf.AddListener(OnAttackOverHalf);
        animationEvents.Events[AnimationState.Attack01].OnExit.AddListener(OnAttackExit);
        animationEvents.Events[AnimationState.Attack02].OnEnter.AddListener(OnAttackStart);
        animationEvents.Events[AnimationState.Attack02].OnEnd.AddListener(OnAttackEnd);
        animationEvents.Events[AnimationState.Attack02].OnOverHalf.AddListener(OnAttackOverHalf);
        animationEvents.Events[AnimationState.Attack02].OnExit.AddListener(OnAttackExit);
        animationEvents.Events[AnimationState.Attack03].OnEnter.AddListener(OnAttackStart);
        animationEvents.Events[AnimationState.Attack03].OnEnd.AddListener(OnAttackEnd);
        animationEvents.Events[AnimationState.Attack03].OnOverHalf.AddListener(OnAttackOverHalf);
        animationEvents.Events[AnimationState.Attack03].OnExit.AddListener(OnAttackExit);
        animationEvents.Events[AnimationState.Attack04].OnEnter.AddListener(OnAttackStart);
        animationEvents.Events[AnimationState.Attack04].OnEnd.AddListener(OnAttackEnd);
        animationEvents.Events[AnimationState.Attack04].OnOverHalf.AddListener(OnAttackOverHalf);
        animationEvents.Events[AnimationState.Attack04].OnExit.AddListener(OnAttackExit);
        animationEvents.Events[AnimationState.Down].OnEnter.AddListener(OnDownStart);
        animationEvents.Events[AnimationState.Down].OnExit.AddListener(OnDownExit);
        animationEvents.Events[AnimationState.Skill01].OnEnter.AddListener(OnSkillStart);
        animationEvents.Events[AnimationState.Skill01].OnEnd.AddListener(OnSkillEnd);
        animationEvents.Events[AnimationState.Skill01].OnOverHalf.AddListener(OnSkillOverHalf);

        
    }

    private void FixedUpdate()
    {
        IsMove = false;
        isGuard = false;
    }

    private void LateUpdate()
    {
        if (IsMove == false)
        {
            Decelerate();
        }

        ProcessSkill();
        ProcessAttack();
        ProcessJump();
        ProcessHit();
        ProcessKnockback();
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
        if (isDead) return;
        if (isGuard) return;
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
        if (isDead) return;
        if (isDown) return;
        if (isStiff) return;
        IsJump = true;
        gameObject.layer = LayerMask.NameToLayer("Jump");
        AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
    }

    public void Attack()
    {
        if (isDead) return;
        if (isGuard) return;
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

    public void Guard()
    {
        isGuard = true;
    }

    public void Skill01()
    {
        if (isDead) return;
        if (isGuard) return;
        if (isDown) return;
        if (isStiff) return;
        if (IsJump) return;
        if (!AttackFlag) return;

        isSkill01 = true;
    }

    public void Skill02()
    {

    }

    public void Skill03()
    {

    }

    public void Skill04()
    {

    }

    public void Knockback(Vector3 direction, float power)
    {
        knockbackFlag = true;
        knockbackTick = 0f;
        knockbackDirection = direction;
        knockbackPower = power;
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
        animator.SetBool(isDeadHash, isDead);
        animator.SetFloat(stiffnessDurationHash, stiffnessDuration);
        if (downFlag)
        {
            downFlag = false;
            animator.SetTrigger(downHash);
        }
        if (isStiff)
        {
            animator.SetTrigger(isStiffHash);
        }
        animator.SetBool(guardHash, isGuard);
        animator.SetBool(skill01Hash, isSkill01);
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
        if (combo > 0)
        {
            if (prevCombo != combo)
            {
                prevCombo = combo;
                comboTick = 0;
                hitBoxFlag = false;
                return;
            }

            comboTick += Time.deltaTime;
            if (comboTick > maxComboDelay)
            {
                combo = 0;
                comboTick = 0;
                hitBoxFlag = false;
                comboFlag = true;
                moveFlag = true;
            }
            else if (comboTick > comboDelay)
            {
                comboFlag = true;
                moveFlag = true;
                if (hitBoxFlag == false)
                {
                    hitBoxFlag = true;
                    var curHitTarget = hitTarget;
                    curHitTarget.x *= IsLeft ? -1f : 1f;
                    var attackType = combo == maxCombo ? AttackType.Down : AttackType.Normal;
                    HitBox.Create(this, transform.position + curHitTarget, attackType);
                }
            }

        }
        else
        {
            comboFlag = true;
        }


    }
    private void ProcessHit()
    {
        var hits = onHitProcedures.Count;
        for (int i = 0; i < hits; i++)
        {
            var hit = onHitProcedures[i];
            var toBeater = hit.Unit.transform.position - transform.position;
            var toVictim = transform.position - hit.Unit.transform.position;
            if (isGuard)
            {
                hit.Unit.Knockback(toBeater.normalized, 0.3f);
            }
            else
            {
                switch (hit.AttackType)
                {
                    case AttackType.Normal:
                        isStiff = true;
                        stiffnessTick = 0f;
                        break;
                    case AttackType.Down:
                        downFlag = true;
                        isDown = true;
                        currentDirection = toBeater.normalized;
                        currentDirection = new Vector3(currentDirection.x, 0f, 0f).normalized;
                        spriteRenderer.flipX = IsLeft;
                        break;
                    default:
                        break;
                }

                moveFlag = false;
                AttackFlag = false;

                var power = isDown ? 2f : 0.3f;
                this.knockbackDirection = toVictim.normalized;
                Knockback(knockbackDirection, power);

                if(gameObject.IsPlayer())
                {
                    Cam.Instance.Shake();
                }

                materialProperty.OnHit = true;

                hp -= hit.Damage;
                if (hp <= 0f)
                {
                    hp = 0f;
                    isDead = true;
                }
            }

            StrikeEffect.Create(hit.HitPoint, toVictim.normalized);
        }
        onHitProcedures.Clear();

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

    }
    private void ProcessKnockback()
    {
        if (knockbackFlag)
        {
            knockbackTick += Time.deltaTime;
            if (knockbackTick > knockbackDuration)
            {
                knockbackFlag = false;
                knockbackTick = 0f;
            }
            else
            {
                transform.position += knockbackDirection * knockbackPower * Time.deltaTime;
            }
        }
    }

    private void ProcessSkill()
    {
    }

    public void OnAttackStart()
    {
        var clips = animator.GetCurrentAnimatorClipInfo(0);
        if (clips != null && clips.Length > 0)
        {
            maxComboDelay = clips[0].clip.length;
            comboDelay = maxComboDelay * 0.6f;
        }
    }

    public void OnAttackOverHalf()
    {
    }

    public void OnAttackEnd()
    {
    }

    public void OnSkillStart()
    {
        ThunderBoltEffect.Create(transform.position, IsLeft ? Vector3.left : Vector3.right);
    }

    public void OnSkillOverHalf()
    {
        var curHitTarget = hitTarget;
        curHitTarget.x *= IsLeft ? -1f : 1f;
        HitBox.Create(this, transform.position + curHitTarget, AttackType.Down);
    }

    public void OnSkillEnd()
    {
        isSkill01 = false;
    }

    public void OnAttackExit()
    {
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

    public void OnHit(Vector3 hitPoint, float damage, Unit beater, AttackType attackType)
    {
        var procedure = new OnHitProcedure();
        procedure.HitPoint = hitPoint;
        procedure.Damage = damage;
        procedure.AttackType = attackType;
        procedure.Unit = beater;
        onHitProcedures.Add(procedure);
    }
}
