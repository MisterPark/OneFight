using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] Team team;
    public Team Team { get { return team; } set { team = value; } }
    public Unit Owner { get; set; }
    public float Damage { get; set; }
    public AttackType AttackType { get; set; }

    private float lifeTime = 0.4f;
    private float lifeTick = 0f;

    private void OnEnable()
    {
        lifeTick = 0f;
    }

    private void Update()
    {
        lifeTick += Time.deltaTime;
        if (lifeTick > lifeTime)
        {
            ObjectPool.Instance.Free(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Unit target = collision.gameObject.GetComponent<Unit>();
        if (target != null)
        {
            if (target.Team != Team && target.IsDead == false)
            {
                target.OnHit(transform.position, Damage, Owner, AttackType);
                ObjectPool.Instance.Free(gameObject);
            }
        }
    }

    public static HitBox Create(Unit owner, Vector3 target, AttackType attackType)
    {
        GameObject hitBoxObj = ObjectPool.Instance.Allocate("HitBox");
        hitBoxObj.transform.position = target;
        var box = hitBoxObj.GetComponent<HitBox>();
        if (box != null)
        {
            box.Owner = owner;
            box.Team = owner.Team;
            box.Damage = owner.Damage;
            box.AttackType = attackType;
        }

        return box;
    }
}
