using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill
{
    [SerializeField] private SkillData data;
    public static int SkillCount = 4;
    private float cooldownTick = 0f;
    public Sprite Icon { get; set; }
    public string Name { get; set; }
    public SkillType Type { get; set; }
    public float Cooldown { get; set; }
    public bool IsCooldown
    {
        get
        {
            return cooldownTick < Cooldown;
        }
    }

    public float LeftCooltimeRatio
    {
        get
        {
            var left = Cooldown - cooldownTick;
            var ratio = left / Cooldown;
            return ratio < 0 ? 0 : ratio;
        }
    }

    public int Damage { get; set; }

    private bool active = false;
    public bool Active => active;

    public void Initialize()
    {
        SetData(data);
    }

    public void Activate()
    {
        active = true;
        cooldownTick = 0f;
    }

    public void Deactivate()
    {
        active = false;
    }

    public void Update()
    {
        cooldownTick += Time.deltaTime;
    }

    public void SetData(SkillData data)
    {
        if (data == null) return;
        Icon = data.icon;
        Name = data.name;
        Type = data.type;
        Cooldown = data.cooltime;
        Damage = data.damage;
    }

    public void SetActive(bool active)
    {
        this.active = active;
    }
}
