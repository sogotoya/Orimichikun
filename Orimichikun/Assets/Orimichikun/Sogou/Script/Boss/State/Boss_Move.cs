using StateMachineAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Move : State<AITester_StateMachine>
{
    public Boss_Move(AITester_StateMachine owner) : base(owner) { }

    public override void Enter()
    {
        Debug.Log("Move開始");

        //攻撃先ランダム指定
        int no = Random.Range(1, 5);
        switch (no)
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
        Debug.Log("Move終了");
    }
}
