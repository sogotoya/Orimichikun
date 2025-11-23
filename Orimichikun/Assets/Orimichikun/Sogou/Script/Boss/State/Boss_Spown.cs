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
        Debug.Log("Spown開始");
        owner.m_Animator.SetTrigger("Houkou");
        owner.m_SW.m_IsSpownWarning = true;//危険マークの表示ON
        owner.StartCoroutine(SoundStart());
        owner.StartCoroutine(SpownRandom());
        owner.StartCoroutine(CntChange());
    }

    public override void Stay()
    {

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

    /// <summary>
    /// スポーン生成
    /// </summary>
    /// <returns></returns>
    IEnumerator SpownRandom()
    {
        yield return new WaitForSeconds(1.5f);
        owner.m_SM.RandomSpown();
        yield return null;
        owner.m_SW.m_IsSpownWarning = false;//危険マークの表示OFF
    }
    /// <summary>
    /// 時間経過でステート変化
    /// </summary>
    /// <returns></returns>
    IEnumerator CntChange()
    {
        yield return new WaitForSeconds(5.5f);
        owner.ChangeState(AIState_ActionType.JumpAttack);
        yield return null;
    }
    /// <summary>
    /// サウンドの開始
    /// </summary>
    /// <returns></returns>
    IEnumerator SoundStart()
    {
        yield return new WaitForSeconds(0.5f);
        owner.m_Spown.Stop();
        owner.m_Spown.Play();
    }
}
