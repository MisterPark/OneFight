using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Unit unit;
    [SerializeField] private float fov;
    [SerializeField] private float stopDistance;

    private void OnValidate()
    {
        if (unit == null) unit = GetComponent<Unit>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (Player.Self == null) return;

        Vector3 to;
        if (DetectPlayer(out to))
        {
            FollowTarget(Player.Self.transform);
        }
    }

    public bool DetectPlayer(out Vector3 to)
    {
        Vector3 playerPos = Player.Self.transform.position;
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
        Vector3 playerPos = Player.Self.transform.position;
        Vector3 myPos = transform.position;
        playerPos.y = 0f;
        myPos.y = 0f;
        Vector3 to = playerPos - myPos;
        if (to.magnitude > stopDistance)
        {
            unit.Move(to.normalized);
        }
    }
}
