using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventInvocator : StateMachineBehaviour
{
    [SerializeField] private AnimationState state;
    public AnimationState State => state;
    public UnityEvent OnEnter = new UnityEvent();
    public UnityEvent OnExit = new UnityEvent();
    public UnityEvent OnEnd = new UnityEvent();
    public UnityEvent OnOverHalf = new UnityEvent();

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        OnEnter.Invoke();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        if(stateInfo.normalizedTime > 0.7f)
        {
            OnOverHalf.Invoke();
        }
        if(stateInfo.normalizedTime > 0.95f)
        {
            OnEnd.Invoke();
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        OnExit.Invoke();
    }
}
