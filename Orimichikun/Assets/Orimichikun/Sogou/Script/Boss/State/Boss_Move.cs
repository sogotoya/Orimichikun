using StateMachineAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Move : State<AITester_StateMachine>
{
    public Boss_Move(AITester_StateMachine owner) : base(owner) { }

    //攻撃先指定変数
    int m_No;

    //１回判定フラグ
    bool m_Flag;
    //最初の移動判定フラグ
    bool m_FastFlag = false;
    public override void Enter()
    {
        Debug.Log("Move開始");

        //攻撃先ランダム指定
        m_No = Random.Range(1, 5);
        m_Flag = false;
    }

    public override void Stay()
    {
        if (!m_Flag)
        {
            owner.StartCoroutine(NextAction());
        }
        //HPが0になっているかの判定
        if (owner.m_HP <= 0)
        {
            owner.ChangeState(AIState_ActionType.Die);
        }
    }

    public override void Exit()
    {
        Debug.Log("Move終了");
    }

    IEnumerator NextAction()
    {
        m_Flag = true;

        if (!m_FastFlag)
        {
            m_FastFlag = true;
            yield return new WaitForSeconds(3.0f);
        }
        else
        {
            yield return new WaitForSeconds(1.5f);
        }
        //行動先
        switch (m_No)
        {
            case 1:
                owner.ChangeState(AIState_ActionType.Roll);
                break;
            case 2:
                owner.ChangeState(AIState_ActionType.Spown);
                break;
            case 3:
                owner.ChangeState(AIState_ActionType.Hari);
                break;
            case 4:
                owner.ChangeState(AIState_ActionType.JumpAttack);
                break;
        }
        yield return null;
    }
}
