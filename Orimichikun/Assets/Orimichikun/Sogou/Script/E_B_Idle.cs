using StateMachineAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_B_Idle : State<AITester>
{
    public E_B_Idle(AITester owner) : base(owner) {}

    //このAIが起動した瞬間に実行(Startと同義)
    public override void Enter()
    {
        Debug.Log("E_B_Idle起動しました!!");
    }
    //このAIが起動中に常に実行(Updateと同義)
    public override void Stay()
    {
    }
    public override void Exit() { }
}
