using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Reflection;
using static UnityEngine.UI.GridLayoutGroup;

namespace StateMachineAI
{
    public enum AIState_ActionType
    {
        Idle,
        Move,
        Roll,
        Spown,
        Hari,
        JumpAttack,
        Houkou,
        Die,
    }


    public class AITester_StateMachine
        : StatefulObjectBase<AITester_StateMachine, AIState_ActionType>
    {
        [Header("BossのHP")]
        public int m_HP=50;

        [Header("StateManagerのListのキャラクター指定番号")]
        public int m_StatemanagerNo;

        public Transform m_Player;

        public StateManager m_StateManager;

        public Animator m_Animator;

        [Header("第二形態変化フラグ")]
        public bool m_IsAnger;


        public Fast_Roll m_FR;
        public Second_Roll m_SR;
        public SummoningMinions m_SM;
        public BossCollarChange m_BCC;
        /// <summary>
        /// クラス名を元にステートを生成して追加する
        /// </summary>
        /// <param name="ClassName">生成するクラスの名前</param>
        public bool AddStateByName(string ClassName)
        {
            try
            {
                // 現在のアセンブリからクラスを取得
                //Type StateType = Assembly.GetExecutingAssembly().GetType($"StateMachineAI.{ClassName}");
                Type StateType = Assembly.GetExecutingAssembly().GetType($"{ClassName}");

                // クラスが見つからなかった場合の対処
                if (StateType == null)
                {
                    Debug.LogError($"{ClassName} クラスが見つかりませんでした。");
                    return true;
                }

                // 型が State<AITester> かどうかをチェック
                if (!typeof(State<AITester_StateMachine>).IsAssignableFrom(StateType))
                {
                    Debug.LogError($"{ClassName} は State<EnemyAI> 型ではありません。");
                    return true;
                }

                // インスタンスを生成
                System.Reflection.ConstructorInfo Constructor =
                    StateType.GetConstructor(new[] { typeof(AITester_StateMachine) });


                if (Constructor == null)
                {
                    Debug.LogError($"{ClassName} のコンストラクタが見つかりませんでした。");
                    return true;
                }

                State<AITester_StateMachine> StateInstance =
                    Constructor.Invoke(new object[] { this }) as State<AITester_StateMachine>;

                if (StateInstance != null)
                {
                    // ステートリストに追加
                    stateList.Add(StateInstance);
                    Debug.Log($"{ClassName} をステートリストに追加しました。");
                    return true;
                }
                else
                {
                    Debug.LogError($"{ClassName} のインスタンス生成に失敗しました。");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"エラーが発生しました。: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 最初のセットアップ
        /// </summary>
        public void AISetUp()
        {

            Debug.Log($"{nameof(AISetUp)}起動", this);
            //ステートマシーンを自身として設定
            stateMachine = new StateMachine<AITester_StateMachine>();
            m_Animator = GetComponent<Animator>();
            m_FR = GameObject.Find("Fast_Roll").GetComponent<Fast_Roll>();
            m_SR = GameObject.Find("Second_Roll").GetComponent<Second_Roll>();
            m_SM = GameObject.Find("Spown").GetComponent<SummoningMinions>();
            m_BCC = GameObject.Find("CollarChange").GetComponent<BossCollarChange>();
            m_BCC.m_SR = gameObject.GetComponent<SpriteRenderer>();




            //初期起動時は、「???」に移行させる
            ChangeState(AIState_ActionType.Move);
        }

    }
}
