using StateMachineAI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Boss_Hari : State<AITester_StateMachine>
{
    public Boss_Hari(AITester_StateMachine owner) : base(owner) { }

    public override void Enter()
    {
        Debug.Log("針飛ばすスタート");
        owner.m_Animator.SetTrigger("Hari");
        owner.m_SS.ShotStart(owner.m_IsAnger,owner.gameObject);
    }

    public override void Stay()
    {
        //HPが0になっているかの判定
        if (owner.m_HP <= 0)
        {
            owner.ChangeState(AIState_ActionType.Die);
        }

        //HPが半分切ったら
        if (owner.m_MaxHP / 2 == owner.m_HP)
        {
            owner.ChangeState(AIState_ActionType.Houkou);
        }
    }

    public override void Exit()
    {
        Debug.Log("針飛ばす終了");
    }

}
