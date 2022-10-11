using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill Data", menuName = "Scriptable Object/Skill Data", order = int.MaxValue)]

public class SkillData : ScriptableObject
{
    public Sprite icon;
    public string skillName;
    public SkillType type;
    public int damage;
    public int damageIncreasePerLevel;
    public float cooltime;
}

[System.Serializable]
public enum SkillType
{
    Passive,
    Active,
}

[System.Serializable]
public enum SkillKind
{
    None,
    ThunderBolt,
    GazellePunch,
    RollingThunder,
    Box,
    End
}

public enum SkillNumber
{
    Skill01,
    Skill02,
    Skill03,
    Skill04,
    End
}

[System.Serializable]
public class SkillElement
{
    public SkillKind kind;
    public SkillData data;
}