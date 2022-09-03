using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public Team Team { get; set; }
    public float Damage { get; set; }

    private float lifeTime = 0.1f;
    private float lifeTick = 0f;

    private void Update()
    {
        lifeTick += Time.deltaTime;
        if(lifeTick > lifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Unit target = collision.gameObject.GetComponent<Unit>();
        if (target != null)
        {
            if (target.Team != Team)
            {
                Debug.Log("Hit");
                target.Damage(Damage);
                Destroy(gameObject);
            }
        }
    }
}
