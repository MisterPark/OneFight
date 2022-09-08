using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialProperty : MonoBehaviour
{
    [SerializeField] private Color _hitColor;
    [SerializeField] private bool _onHit;

    public Color HitColor { get { return _hitColor; } set { _hitColor = value; } }
    public bool OnHit { get { return _onHit; } set { _onHit = value; } }

    private MaterialPropertyBlock _mpb;
    private Renderer _renderer;

    private float twinkleTick = 0f;
    private float twinkleDuration = 0.5f;

    private float transTick = 0f;
    private float transDelay = 0.1f;

    private void Start()
    {
        _mpb = new MaterialPropertyBlock();
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (OnHit)
        {
            twinkleTick += Time.deltaTime;
            if (twinkleTick > twinkleDuration)
            {
                twinkleTick = 0f;
                OnHit = false;
            }
            else
            {
                transTick += Time.deltaTime;
                if (transTick > transDelay)
                {
                    transTick = 0f;
                    OnHit = !OnHit;
                }
            }
        }
        _renderer.GetPropertyBlock(_mpb);
        _mpb.SetColor("_HitColor", _hitColor);
        _mpb.SetFloat("_Ratio", OnHit ? 1 : 0);
        _renderer.SetPropertyBlock(_mpb);
    }

}
