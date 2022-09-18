using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeEffect : MonoBehaviour
{
    [SerializeField] private int strikeHash;
    private Animator animator;

    private int strikeType = 0;

    private float tick = 0f;
    private float time = 0.15f;

    private void OnValidate()
    {
        strikeHash = Animator.StringToHash("Strike");
    }

    private void OnEnable()
    {
        tick = 0f;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        strikeType = Random.Range(0, 2);
    }

    void Update()
    {
        tick += Time.deltaTime;
        if(tick > time)
        {
            ObjectPool.Instance.Free(gameObject);
        }
    }

    private void LateUpdate()
    {
        ProcessAnimation();
    }

    private void ProcessAnimation()
    {
        animator.SetInteger(strikeHash, strikeType);
    }
}
