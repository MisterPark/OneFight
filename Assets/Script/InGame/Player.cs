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

    private void OnValidate()
    {
        if (unit == null) unit = GetComponent<Unit>();
    }

    void Update()
    {
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
            unit.IsJump = true;
            gameObject.layer = LayerMask.NameToLayer("Jump");
            unit.AddForce(Vector2.up * unit.JumpPower, ForceMode2D.Impulse);
        }

        
    }



}
