using StateMachineAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Boss_Die : State<AITester_StateMachine>
{
    public Boss_Die(AITester_StateMachine owner) : base(owner) { }

    float m_Timer = 0f;

    public override void Enter()
    {
        Debug.Log("Die開始");
        owner.m_ETM.EndBoss = () =>
        {
            owner.m_OCC.ObjectCollarResetChangeStart();
            //カメラズームON
            owner.m_ZO.m_IsZoomFlag = true;
            owner.m_Animator.SetTrigger("Die");
            owner.StartCoroutine(OnCoin());

        };
        owner.m_Animator.SetTrigger("Idle");

     

        owner.m_GC.m_BossDie = true;
        owner.m_Hari.Stop();
        owner.m_Houkou.Stop();
        owner.m_Jump.Stop();
        owner.m_Move.Stop();
        owner.m_Spown.Stop();
        owner.m_BoxColliderObj.SetActive(false);
        //会話スタート
        owner.m_ETM.EndTextStart();

        //操作停止
        owner.m_PCC.m_IsPSPlaying = true;
    }

    public override void Stay()
    {

    }

    public override void Exit()
    {
        Debug.Log("Die終了");
    }

    /// <summary>
    /// コイン出現処理
    /// </summary>
    /// <returns></returns>
    public IEnumerator OnCoin()
    {
        yield return new WaitForSeconds(1f);
        owner.m_Die.Play();
        owner.m_BoxColliderObj.SetActive(false);
        owner.m_BM.m_BossDie=true ;
        //カメラズームOFF
        owner.m_ZO.m_IsZoomFlag = false;
    }
}
