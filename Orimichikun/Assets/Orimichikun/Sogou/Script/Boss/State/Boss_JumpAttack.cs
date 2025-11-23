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
    bool m_CntFlag = false;

    public override void Enter()
    {
        //HP‚ª0‚É‚È‚Á‚Ä‚¢‚é‚©‚Ì”»’è
        if (owner.m_HP <= 0)
        {
            owner.ChangeState(AIState_ActionType.Die);
        }
        Debug.Log("JumpAttackŠJŽn");
        owner.m_Animator.SetTrigger("Jump");
        m_Flag=false;
        m_CntFlag = false;
        owner.m_Jump.Stop();
        owner.m_Jump.Play();
        owner.StartCoroutine(JumpA());

        owner.StartCoroutine(owner.m_JP.JumpAttackStart(owner.gameObject, result =>
        {
            if (result == 100 && !m_CntFlag)
            {
                owner.ChangeState(AIState_ActionType.Hari);
                m_CntFlag = true;
            }
        }));
    }

    public override void Stay()
    {
        //HP‚ª0‚É‚È‚Á‚Ä‚¢‚é‚©‚Ì”»’è
        if (owner.m_HP <= 0)
        {
            owner.ChangeState(AIState_ActionType.Die);
        }

        //HP‚ª”¼•ªØ‚Á‚½‚ç
        if (owner.m_MaxHP / 2 == owner.m_HP && !owner.m_IsAnger)
        {
            owner.ChangeState(AIState_ActionType.Houkou);
        }


    }

    public override void Exit()
    {
        Debug.Log("JumpAttackI—¹");
    }
    /// <summary>
    /// “®‚©‚³‚È‚¢ŽžŠÔ
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
