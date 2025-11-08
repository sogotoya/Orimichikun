using StateMachineAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_RollFast : State<AITester_StateMachine>
{
    public Boss_RollFast(AITester_StateMachine owner) : base(owner) { }

    public override void Enter()
    {
        Debug.Log("ノーマルモードの回転スタート");
        owner.m_Animator.SetTrigger("Roll_1");
    }

    public override void Stay()
    {
    }

    public override void Exit()
    {
    }
}
