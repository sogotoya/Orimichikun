using StateMachineAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Roll : State<AITester_StateMachine>
{
    public Boss_Roll(AITester_StateMachine owner) : base(owner) { }

    public override void Enter()
    {
        Debug.Log("回転スタート");

        //Moveに移行
        //owner.ChangeState(AIState_ActionType.Move);
    }

    public override void Stay()
    {
        //HPが0になっているかの判定
        if (owner.m_HP <= 0)
        {
            owner.ChangeState(AIState_ActionType.Die);
        }

        //初期状態なら
        if (!owner.m_IsAnger)
        {
            owner.m_Animator.SetTrigger("Roll_1");
            owner.m_FR.NormalRoll(owner.gameObject);
        }
        else//怒り状態なら
        {
            owner.m_Animator.SetTrigger("Roll_2");
            owner.m_SR.AngrylRoll(owner.gameObject);
        }
    }

    public override void Exit()
    {
        Debug.Log("回転終了");
    }
}
