using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
    public static bool IsPlayer(this GameObject gameObject)
    {
        if (gameObject == null) return false;

        if (gameObject == Player.Self) return true;

        return false;
    }

    public static int GetLayerMask(this GameObject gameObject)
    {
        return 1 << gameObject.layer;
    }

    public static UnitState ToUnitState(this string name)
    {
        for (int i = (int)UnitState.Idle; i < (int)UnitState.End; i++)
        {
            UnitState state = (UnitState)i;
            if (string.IsNullOrEmpty(name))
            {
                return UnitState.None;
            }

            if(name.Contains(state.ToString()))
            {
                return state;
            }
        }
        return UnitState.None;
    }
}
