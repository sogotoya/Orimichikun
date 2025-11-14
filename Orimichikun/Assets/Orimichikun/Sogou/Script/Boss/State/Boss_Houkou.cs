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
        //HP‚ª0‚É‚È‚Á‚Ä‚¢‚é‚©‚Ì”»’è
        if (owner.m_HP <= 0)
        {
            owner.ChangeState(AIState_ActionType.Die);
        }
        Debug.Log("HoukouŠJŽn");
        owner.m_IsAnger = true;
        owner.m_Animator.SetBool("Houkou", true);
        //“{‚èó‘Ôtrue
        owner.m_BM.m_BossAnger = true;
        owner.StartCoroutine(PunpunRoll());
    }

    public override void Stay()
    {

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
    /// <summary>
    /// “®‚©‚³‚È‚¢ŽžŠÔ
    /// </summary>
    /// <returns></returns>
    IEnumerator PunpunRoll()
    {
        yield return new WaitForSeconds(3.0f);
        owner.ChangeState(AIState_ActionType.Roll);
        Debug.Log("’âŽ~’†");

    }
}