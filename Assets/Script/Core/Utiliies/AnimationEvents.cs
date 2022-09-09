using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class AnimationEvents : MonoBehaviour
{
    private Animator animator;
    private Dictionary<AnimationState, AnimationEventInvocator> animationEvents = new Dictionary<AnimationState, AnimationEventInvocator>();

    public Dictionary<AnimationState, AnimationEventInvocator> Events => animationEvents;


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
