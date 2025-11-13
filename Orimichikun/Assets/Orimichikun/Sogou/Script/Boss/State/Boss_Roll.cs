using StateMachineAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Roll : State<AITester_StateMachine>
{
    public Boss_Roll(AITester_StateMachine owner) : base(owner) { }

    bool m_Flag=false;
    public override void Enter()
    {
        Debug.Log("回転スタート");
        m_Flag = false;
        if (!owner.m_IsAnger)
        {
            owner.m_Animator.SetTrigger("Roll_1");
        }
        else
        {
            owner.m_Animator.SetTrigger("Roll_2");
        }
    }

    public override void Stay()
    {
        //HPが0になっているかの判定
        if (owner.m_HP <= 0)
        {
            owner.ChangeState(AIState_ActionType.Die);
        }

        //HPが半分切ったら
        if (owner.m_MaxHP / 2 == owner.m_HP && !owner.m_IsAnger)
        {
            owner.ChangeState(AIState_ActionType.Houkou);
        }

        //初期状態なら
        if (!owner.m_IsAnger)
        {
            owner.StartCoroutine(Roll());
            if (owner.m_FR.NormalRoll(owner.gameObject,2)==100)
            {
                owner.ChangeState(AIState_ActionType.Hari);
            }
        }
        else//怒り状態なら
        {
            owner.StartCoroutine(Roll());
            if (owner.m_SR.AngrylRoll(owner.gameObject,2)==100)
            {
                owner.ChangeState(AIState_ActionType.Hari);
            }
        }


    }

    public override void Exit()
    {
        Debug.Log("回転終了");
    }

    /// <summary>
    /// 動かさない時間
    /// </summary>
    /// <returns></returns>
    IEnumerator Roll()
    {
        if (!m_Flag)
        {
            yield return new WaitForSeconds(1.0f);
            m_Flag = true;
        }

    }
}
