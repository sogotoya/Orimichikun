using StateMachineAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Boss_Spown : State<AITester_StateMachine>
{
    public Boss_Spown(AITester_StateMachine owner) : base(owner) { }

    float m_Timer = 0f;

    public override void Enter()
    {
        Debug.Log("SpownŠJŽn");
        owner.m_Animator.SetTrigger("Houkou");
        owner.StartCoroutine(SoundStart());
        owner.StartCoroutine(SpownRandom());
        owner.StartCoroutine(CntChange());
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
        Debug.Log("SpownI—¹");
    }


    IEnumerator SpownRandom()
    {
        yield return new WaitForSeconds(1.5f);
        owner.m_SM.RandomSpown();
        yield return null;
    }

    IEnumerator CntChange()
    {
        yield return new WaitForSeconds(5.5f);
        owner.ChangeState(AIState_ActionType.JumpAttack);
        yield return null;
    }
    IEnumerator SoundStart()
    {
        yield return new WaitForSeconds(0.5f);
        owner.m_Spown.Stop();
        owner.m_Spown.Play();
    }
}
