using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player player = null;
    public static Player Self => player;

    [SerializeField] private Unit unit;

    private void Awake()
    {
        player = this;
    }

    private void Start()
    {
        unit.Team = Team.Player;
    }

    private void OnValidate()
    {
        if (unit == null) unit = GetComponent<Unit>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            unit.Guard();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            unit.Attack();
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            unit.Move(Vector3.right);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            unit.Move(Vector3.left);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && unit.IsJump == false)
        {
            unit.Jump();
        }


    }



}
