using StateMachineAI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Boss_Houkou : State<AITester_StateMachine>
{
    public Boss_Houkou(AITester_StateMachine owner) : base(owner) { }

    float m_Timer=0f;
    public override void Enter()
    {
        Debug.Log("Houkou開始");
        owner.m_Animator.SetTrigger("Houkou");
        owner.m_BCC.CollarChangeStart();
        //Moveに移行
        owner.ChangeState(AIState_ActionType.Move);
    }

    public override void Stay()
    {
        m_Timer += Time.deltaTime;
        //3秒たったら移行
        if(m_Timer==3)
        {
            owner.ChangeState(AIState_ActionType.Move);
        }

        //HPが0になっているかの判定
        if (owner.m_HP <= 0)
        {
            owner.ChangeState(AIState_ActionType.Die);
        }
    }

    public override void Exit()
    {
        Debug.Log("Houkou終了");
    }
}