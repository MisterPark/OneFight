using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player player = null;
    public static Player Instance => player;

    [SerializeField] private Unit unit;

    public Unit Unit => unit;

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
        if (Input.GetKey(KeyCode.DownArrow))
        {
            unit.Guard();
        }
        if (Input.GetKey(KeyCode.C))
        {
            unit.Lift();
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

        if(Input.GetKeyDown(KeyCode.Space))
        {
            //GameObject go = ObjectPool.Instance.Allocate("Textbox");
            //var textbox = go.GetComponent<Textbox>();
            //textbox.Unit = unit;
            //textbox.TotalOutput = "음하음하음하음하음하음하음하음하";
            
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            unit.Skill(0);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            unit.Skill(1);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            unit.Skill(2);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            unit.Skill(3);
        }


    }



}
