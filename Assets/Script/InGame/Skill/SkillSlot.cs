using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    [SerializeField] private SkillNumber number;

    [SerializeField] private Image cooltimeImage;

    private void LateUpdate()
    {
        ProcessCooltime();
    }

    private void ProcessCooltime()
    {
        if (Player.Instance == null) return;
        if (Player.Instance.Unit == null) return;
        if (Player.Instance.Unit.Skills == null) return;
        if (Player.Instance.Unit.Skills[(int)number] == null) return;

        var skill = Player.Instance.Unit.Skills[(int)number];
        cooltimeImage.fillAmount = skill.LeftCooltimeRatio;
    }
}
