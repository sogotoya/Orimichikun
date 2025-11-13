using StateMachineAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Boss_Spown : State<AITester_StateMachine>
{
    public Boss_Spown(AITester_StateMachine owner) : base(owner) { }

    float m_Timer=0f;

    public override void Enter()
    {
        Debug.Log("Spown開始");
        owner.m_Animator.SetTrigger("Houkou");
        owner.StartCoroutine(SpownRandom());

        m_Timer = 0f;
    }

    public override void Stay()
    {
        m_Timer += Time.deltaTime;
        //4秒たったらMoveに移行
        if(m_Timer==2)
        {
            owner.ChangeState(AIState_ActionType.JumpAttack);
        }
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
    }

    public override void Exit()
    {
        Debug.Log("Spown終了");
    }


    IEnumerator SpownRandom()
    {
        yield return new WaitForSeconds(1.5f);
        owner.m_SM.RandomSpown();
        yield return null;
    }
}
