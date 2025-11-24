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
        Move,
        Roll,
        Spown,
        Hari,
        JumpAttack,
        Houkou,
        Kaminari,
        Die,
    }


    public class AITester_StateMachine
        : StatefulObjectBase<AITester_StateMachine, AIState_ActionType>
    {
        [Header("BossのHP")]
        public int m_HP=50;
        public int m_MaxHP;
        [Header("StateManagerのListのキャラクター指定番号")]
        public int m_StatemanagerNo;

        //public Transform m_Player;

        public StateManager m_StateManager;

        public Animator m_Animator;

        [Header("第二形態変化フラグ")]
        public bool m_IsAnger;


        public Fast_Roll m_FR;
        public Second_Roll m_SR;
        public SummoningMinions m_SM;
        public BossCollarChange m_BCC;
        public Scatter_Shot m_SS;
        public BossManager m_BM;
        public ReturnToStartPosition m_RTSP;
        public JumpPosition m_JP;
        public GameClear m_GC;
        public ZoomObject m_ZO;
        public SpownWarning m_SW;
        public Thunder_Position m_TP;


        public AudioSource m_Die;
        public AudioSource m_Hari;
        public AudioSource m_Houkou;
        public AudioSource m_Jump;
        public AudioSource m_Spown;
        public AudioSource m_Move;
        public BoxCollider2D m_BoxCollider;

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
            m_BCC.m_SR = this.gameObject.GetComponent<SpriteRenderer>();
            m_BCC.m_SR=gameObject.GetComponent<SpriteRenderer>();
            m_BCC.m_BossAnimt=this.gameObject.GetComponent<Animator>();
            m_SS = GameObject.Find("Scatter_Shot").GetComponent<Scatter_Shot>();
            m_BM=GameObject.Find("BossManager").GetComponent<BossManager>();
            m_BM.m_BossObj=this.gameObject;
            m_BM.m_AITSM = this.gameObject.GetComponent<AITester_StateMachine>();
            m_RTSP = GameObject.Find("ReturnToStartPosition").GetComponent<ReturnToStartPosition>();
            m_JP = GameObject.Find("JumpPosition").GetComponent<JumpPosition>();
            m_GC = GameObject.Find("GameClearEnemy").GetComponent<GameClear>();
            m_ZO = GameObject.Find("ZoomObjectEnemy").GetComponent<ZoomObject>();
            m_ZO.m_ZoomObj = this.gameObject;
            m_SW = GameObject.Find("SpownWarning").GetComponent<SpownWarning>();
            m_TP = GameObject.Find("ThunderAttackPosition").GetComponent<Thunder_Position>();


            m_MaxHP = m_HP;
            Debug.Log("生成完了");
            //初期起動時は、「???」に移行させる
            ChangeState(AIState_ActionType.Roll);

        }

    }
}
