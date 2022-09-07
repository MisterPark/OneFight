using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] Team team;
    public Team Team { get { return team; } set { team = value; } }
    public float Damage { get; set; }

    private float lifeTime = 0.4f;
    private float lifeTick = 0f;

    private void Update()
    {
        lifeTick += Time.deltaTime;
        if (lifeTick > lifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Unit target = collision.gameObject.GetComponent<Unit>();
        if (target != null)
        {
            if (target.Team != Team)
            {
                var knockbackDirection = (collision.transform.position - transform.position).normalized;
                Debug.Log("Hit");

                target.OnHit(Damage, knockbackDirection);
                Destroy(gameObject);
            }
        }
    }
}
