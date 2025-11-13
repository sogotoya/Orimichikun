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
        Debug.Log("HoukouäJén");
        owner.m_IsAnger = true;

    }

    public override void Stay()
    {

        if (!m_Flag && owner.m_RTSP.ReturnPosition(owner.gameObject))
        {
            m_Flag = true;
            owner.m_Animator.SetBool("Houkou", true);
            //ì{ÇËèÛë‘true
            owner.m_BM.m_BossAnger = true;
            Debug.Log("sasaksaksaksakska");
        }
        //HPÇ™0Ç…Ç»Ç¡ÇƒÇ¢ÇÈÇ©ÇÃîªíË
        if (owner.m_HP <= 0)
        {
            owner.ChangeState(AIState_ActionType.Die);
        }
    }

    public override void Exit()
    {
        Debug.Log("HoukouèIóπ");
        owner.m_Animator.SetBool("Houkou", false);
    }
}