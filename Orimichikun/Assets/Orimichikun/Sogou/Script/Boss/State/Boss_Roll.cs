using StateMachineAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Roll : State<AITester_StateMachine>
{
    public Boss_Roll(AITester_StateMachine owner) : base(owner) { }

    bool m_Flag = false;
    bool m_Punpun = false;
    public override void Enter()
    {
        //HP‚ª0‚É‚È‚Á‚Ä‚¢‚é‚©‚Ì”»’è
        if (owner.m_HP <= 0)
        {
            owner.ChangeState(AIState_ActionType.Die);
        }
        Debug.Log("‰ñ“]ƒXƒ^[ƒg");
        m_Flag = false;
        owner.StartCoroutine(SoundStart());
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

        //‰Šúó‘Ô‚È‚ç
        if (!owner.m_IsAnger)
        {
            owner.StartCoroutine(Roll());
            if (owner.m_FR.NormalRoll(owner.gameObject, 2) == 100)
            {
                owner.ChangeState(AIState_ActionType.Spown);
            }
        }
        else//“{‚èó‘Ô‚È‚ç
        {
            Debug.Log("‚±‚È‚Õ‚ñ‚Õ‚ñ");
            if(!m_Punpun)owner.StartCoroutine (PunpunWait());

            owner.m_FR.m_MoveSpeed = 5;
            if (owner.m_FR.NormalRoll(owner.gameObject, 2) == 100)
            {
                owner.ChangeState(AIState_ActionType.Spown);
            }
            //if (owner.m_SR.AngrylRoll(owner.gameObject, 2) == 100)
            //{
            //    owner.ChangeState(AIState_ActionType.Spown);
            //}
        }

    }

    public override void Exit()
    {
        Debug.Log("‰ñ“]I—¹");
    }

    /// <summary>
    /// “®‚©‚³‚È‚¢ŽžŠÔ
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
    IEnumerator PunpunWait()
    {

        yield return new WaitForSeconds(3f);
        m_Punpun = true;
    }

    IEnumerator SoundStart()
    {
        yield return new WaitForSeconds(1f);
        owner.m_Move.Stop();
        owner.m_Move.Play();
    }
}
