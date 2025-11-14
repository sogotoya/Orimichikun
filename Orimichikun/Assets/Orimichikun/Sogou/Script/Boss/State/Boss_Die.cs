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
        owner.m_Animator.SetTrigger("Die");
        owner.StartCoroutine(OnCoin());
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
        owner.m_Coin.SetActive(true);
    }
}
