using StateMachineAI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Boss_Houkou : State<AITester_StateMachine>
{
    public Boss_Houkou(AITester_StateMachine owner) : base(owner) { }

    float m_Timer = 0f;

    bool m_Flag = false;

    public override void Enter()
    {
        Debug.Log("HoukouŠJŽn");


    }

    public override void Stay()
    {
        if (owner.m_RTSP.ReturnPosition(owner.gameObject))
        {

            if (!m_Flag)
            {
                m_Flag = true;
                owner.m_Animator.SetBool("Houkou", true);
                //“{‚ètrue
                owner.m_BM.m_BossAnger = true;
            }
        }

        //HP‚ª0‚É‚È‚Á‚Ä‚¢‚é‚©‚Ì”»’è
        if (owner.m_HP <= 0)
        {
            owner.ChangeState(AIState_ActionType.Die);
        }
    }

    public override void Exit()
    {
        Debug.Log("HoukouI—¹");
        owner.m_Animator.SetBool("Houkou", false);
    }
}