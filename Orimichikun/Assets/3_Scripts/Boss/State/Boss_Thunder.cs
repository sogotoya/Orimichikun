using StateMachineAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Boss_Thunder : State<AITester_StateMachine>
{
    public Boss_Thunder(AITester_StateMachine owner) : base(owner) { }

    public override void Enter()
    {
        Debug.Log("雷開始");

        //雷終了時に JumpAttack に進むように設定
        owner.m_TP.OnThunderEnd = () =>
        {
            owner.ChangeState(AIState_ActionType.JumpAttack);
        };

        owner.m_TP.ThunderAttackStart();
    }

    public override void Stay()
    {

    }

    public override void Exit()
    {
        Debug.Log("雷終了");
    }
    /// <summary>
    /// 次のステート移行までの待機時間
    /// </summary>
    /// <returns></returns>
    IEnumerator ThunderWait()
    {
        yield return new WaitForSeconds(5f);
    }
}
