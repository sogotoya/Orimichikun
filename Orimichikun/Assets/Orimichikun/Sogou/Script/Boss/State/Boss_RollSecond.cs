using StateMachineAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_RollSecond : State<AITester_StateMachine>
{
    public Boss_RollSecond(AITester_StateMachine owner) : base(owner) { }

    public override void Enter()
    {
        Debug.Log("怒りモードの回転スタート");
        owner.m_Animator.SetTrigger("Roll_2");
    }

    public override void Stay()
    {
    }

    public override void Exit()
    {
    }
}
