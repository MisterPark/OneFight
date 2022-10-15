using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftableObject : Entity
{
    private Unit unit;

    public void SetUnit(Unit unit)
    {
        this.unit = unit;
    }

    private void LateUpdate()
    {
        if (unit == null) return;

        transform.position = unit.LiftTarget;
    }
}
