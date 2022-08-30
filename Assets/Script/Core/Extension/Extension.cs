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
}
