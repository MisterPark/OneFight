using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
    public static bool IsPlayer(this GameObject gameObject)
    {
        if (gameObject == null) return false;

        if (gameObject == Player.Instance.gameObject) return true;

        return false;
    }

    public static int GetLayerMask(this GameObject gameObject)
    {
        return 1 << gameObject.layer;
    }

    public static AnimationState ToUnitState(this string name)
    {
        for (int i = (int)AnimationState.Idle; i < (int)AnimationState.End; i++)
        {
            AnimationState state = (AnimationState)i;
            if (string.IsNullOrEmpty(name))
            {
                return AnimationState.None;
            }

            if(name.Contains(state.ToString()))
            {
                return state;
            }
        }
        return AnimationState.None;
    }
}
