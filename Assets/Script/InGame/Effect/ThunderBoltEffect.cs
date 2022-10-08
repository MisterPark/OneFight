using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderBoltEffect : MonoBehaviour
{
    private float tick = 0f;
    private float time = 0.3f;
    private void OnEnable()
    {
        tick = 0f;
    }
    void Update()
    {
        tick += Time.deltaTime;
        if (tick > time)
        {
            ObjectPool.Instance.Free(gameObject);
        }
    }

    public static ThunderBoltEffect Create(Vector3 target, Vector3 direction)
    {
        GameObject hitEffectObj = ObjectPool.Instance.Allocate("ThunderBoltEffect");
        hitEffectObj.transform.position = target;
        var effect = hitEffectObj.GetComponent<ThunderBoltEffect>();
        var spriteRenderer = hitEffectObj.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = direction.x < 0;
        }

        return effect;
    }
}
