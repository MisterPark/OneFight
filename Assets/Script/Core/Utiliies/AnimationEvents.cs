using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class AnimationEvents : MonoBehaviour
{
    private Animator animator;
    private Dictionary<UnitState, AnimationEventInvocator> animationEvents = new Dictionary<UnitState, AnimationEventInvocator>();

    public Dictionary<UnitState, AnimationEventInvocator> Events => animationEvents;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        var behaviours = animator.GetBehaviours<AnimationEventInvocator>();
        foreach (var behaviour in behaviours)
        {
            animationEvents.Add(behaviour.State, behaviour);
        }
    }
}
