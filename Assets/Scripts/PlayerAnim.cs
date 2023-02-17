using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    Animator animator;

    private static readonly int BreakStart = Animator.StringToHash("BreakStart");
    private static readonly int Breaking = Animator.StringToHash("Breaking");
    private static readonly int BreakEnd = Animator.StringToHash("BreakEnd");
    private static readonly int Idling = Animator.StringToHash("Idle");

    AnimState state = AnimState.IDLE;

    void Start()
    {
        animator = GetComponent<Animator>();
    }


    public void Break()
    {
        if(state == AnimState.BREAKING) return;
        animator.CrossFade(BreakStart, 0, 0);
        state = AnimState.BREAKING;
    }

    public void EndBreak()
    {
        if(state == AnimState.IDLE) return;
        animator.CrossFade(BreakEnd, 0, 0);
        state = AnimState.IDLE;
    }

    public void Idle()
    {
        if(state == AnimState.IDLE) return;
        animator.CrossFade(Idling, 0, 0);
        state = AnimState.IDLE;
    }
}
