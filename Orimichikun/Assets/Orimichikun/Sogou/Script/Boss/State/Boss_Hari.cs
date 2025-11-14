using StateMachineAI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Boss_Hari : State<AITester_StateMachine>
{
    public Boss_Hari(AITester_StateMachine owner) : base(owner) { }

    float m_Timer = 0f;
    public override void Enter()
    {
        Debug.Log("針飛ばすスタート");
        owner.m_Animator.SetTrigger("Hari");

        owner.StartCoroutine(StartShot());
        m_Timer = 0f;
    }

    public override void Stay()
    {
        m_Timer += Time.deltaTime;

        //指定時間経過した移行
        if(m_Timer>=1.5)
        {
            owner.ChangeState(AIState_ActionType.Roll);
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
        Debug.Log("針飛ばす終了");
    }
    IEnumerator StartShot()
    {
        yield return new WaitForSeconds(0.3f);
        owner.m_SS.ShotStart(owner.m_IsAnger, owner.gameObject);
        yield return null;
    }
}
