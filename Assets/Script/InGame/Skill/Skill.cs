using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Skill
{
    private bool isActive = false;
    private float cooltimeTIck = 0f;
    public Sprite Icon { get; set; }
    public string Name { get; set; }
    public SkillType Type { get; set; }
    public bool IsActive => isActive;
    public float Cooltime { get; set; }
    public bool IsCooltime
    {
        get
        {
            return cooltimeTIck < Cooltime;
        }
    }
    public int Damage { get; set; }

    public void Activate()
    {
        if (IsCooltime) return;
        isActive = true;
    }

    public virtual void Update()
    {
        cooltimeTIck += Time.deltaTime;
    }
}
