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
        //HPが0になっているかの判定
        if (owner.m_HP <= 0)
        {
            owner.ChangeState(AIState_ActionType.Die);
        }
        Debug.Log("Houkou開始");

        //敵モブ削除
        owner.m_BM.m_BossDie = true;
        owner.m_BM.m_BossDie = false;

        owner.m_IsAnger = true;
        //操作停止
        owner.m_PCC.m_IsAllPlaying = false;
        //カメラズームON
        owner.m_ZO.m_IsZoomFlag = true;

        owner.m_Animator.SetBool("Houkou", true);
        //背景色変化
        owner.m_OCC.ObjectCollarChangeStart();
        //怒り状態true
        owner.m_BM.m_BossAnger = true;
        owner.StartCoroutine(SoundStart());
        owner.StartCoroutine(PunpunRoll());
    }

    public override void Stay()
    {

        //HPが0になっているかの判定
        if (owner.m_HP <= 0)
        {
            owner.ChangeState(AIState_ActionType.Die);
        }
    }

    public override void Exit()
    {
        owner.StopAllCoroutines();
        Debug.Log("Houkou終了");
        //操作開始
        owner.m_PCC.m_IsAllPlaying = true;
        owner.m_Animator.SetBool("Houkou", false);
    }
    /// <summary>
    /// 動かさない時間
    /// </summary>
    /// <returns></returns>
    IEnumerator PunpunRoll()
    {
        yield return new WaitForSeconds(3.0f);
        //カメラズームOFF
        owner.m_ZO.m_IsZoomFlag = false;
        yield return new WaitForSeconds(1.5f);
        owner.ChangeState(AIState_ActionType.Kaminari);
        Debug.Log("停止中");

    }

    IEnumerator SoundStart()
    {
        yield return new WaitForSeconds(0.5f);
        owner.m_Houkou.Stop();
        owner.m_Houkou.Play();
    }
}