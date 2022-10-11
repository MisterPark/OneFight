using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Unit unit;
    [SerializeField] private float fov;
    [SerializeField] private float stopDistance;
    [SerializeField] private float attackDelay;

    private float attackTick = 0f;

    private void OnValidate()
    {
        if (unit == null) unit = GetComponent<Unit>();
    }

    private void Start()
    {
        unit.Team = Team.Enemy;
    }

    private void Update()
    {
        if (Player.Instance == null) return;

        Vector3 to;
        if (DetectPlayer(out to))
        {
            //unit.Guard();
            FollowTarget(Player.Instance.transform);
            AttackTarget(Player.Instance.transform);
        }
    }

    public bool DetectPlayer(out Vector3 to)
    {
        Vector3 playerPos = Player.Instance.transform.position;
        Vector3 myPos = transform.position;
        playerPos.y = 0f;
        myPos.y = 0f;
        to = playerPos - myPos;
        if (to.magnitude > fov)
        {
            return false;
        }
        return true;
    }

    public void FollowTarget(Transform target)
    {
        Vector3 playerPos = Player.Instance.transform.position;
        Vector3 myPos = transform.position;
        playerPos.y = 0f;
        myPos.y = 0f;
        Vector3 to = playerPos - myPos;
        if (to.magnitude > stopDistance)
        {
            unit.Move(to.normalized);
        }
    }

    public void AttackTarget(Transform target)
    {
        Vector3 playerPos = Player.Instance.transform.position;
        Vector3 myPos = transform.position;
        playerPos.y = 0f;
        myPos.y = 0f;
        Vector3 to = playerPos - myPos;
        if (to.magnitude <= stopDistance)
        {
            attackTick += Time.deltaTime;
            if (attackTick > attackDelay)
            {
                attackTick = 0f;
                unit.Attack();

            }
        }
        else
        {
            attackTick = 0f;
        }
    }
}
