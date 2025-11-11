using StateMachineAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Boss_Die : State<AITester_StateMachine>
{
    public Boss_Die(AITester_StateMachine owner) : base(owner) { }

    float m_Timer = 0f;
    public override void Enter()
    {
        Debug.Log("DieäJén");
        owner.m_Animator.SetTrigger("Die");
    }

    public override void Stay()
    {

    }

    public override void Exit()
    {
        Debug.Log("DieèIóπ");
    }
}
