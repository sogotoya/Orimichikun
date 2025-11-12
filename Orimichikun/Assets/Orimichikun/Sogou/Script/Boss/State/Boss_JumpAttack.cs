using StateMachineAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Boss_JumpAttack : State<AITester_StateMachine>
{
    public Boss_JumpAttack(AITester_StateMachine owner) : base(owner) { }

    float m_Timer = 0f;
    public override void Enter()
    {
        Debug.Log("JumpAttackŠJŽn");
    }

    public override void Stay()
    {
        //HP‚ª0‚É‚È‚Á‚Ä‚¢‚é‚©‚Ì”»’è
        if (owner.m_HP <= 0)
        {
            owner.ChangeState(AIState_ActionType.Die);
        }
    }

    public override void Exit()
    {
        Debug.Log("JumpAttackI—¹");
    }
}
