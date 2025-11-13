using StateMachineAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Boss_JumpAttack : State<AITester_StateMachine>
{
    public Boss_JumpAttack(AITester_StateMachine owner) : base(owner) { }

    float m_Timer = 0f;


    bool m_Flag = false;

    public override void Enter()
    {
        Debug.Log("JumpAttackäJén");
        m_Flag=false;
    }

    public override void Stay()
    {
        //HPÇ™0Ç…Ç»Ç¡ÇƒÇ¢ÇÈÇ©ÇÃîªíË
        if (owner.m_HP <= 0)
        {
            owner.ChangeState(AIState_ActionType.Die);
        }

        //HPÇ™îºï™êÿÇ¡ÇΩÇÁ
        if (owner.m_MaxHP / 2 == owner.m_HP && !owner.m_IsAnger)
        {
            owner.ChangeState(AIState_ActionType.Houkou);
        }

        owner.StartCoroutine(JumpA());
        owner.StartCoroutine(owner.m_JP.JumpAttackStart(owner.gameObject, result =>
        {
            if (result==100)
                owner.ChangeState(AIState_ActionType.Hari);
        }));
    }

    public override void Exit()
    {
        Debug.Log("JumpAttackèIóπ");
    }
    /// <summary>
    /// ìÆÇ©Ç≥Ç»Ç¢éûä‘
    /// </summary>
    /// <returns></returns>
    IEnumerator JumpA()
    {
        if (!m_Flag)
        {
            yield return new WaitForSeconds(1.0f);
            m_Flag = true;
        }

    }
}
