using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Reflection;

namespace StateMachineAI
{
    /// <summary>
    /// 敵のステートリスト
    /// ここでステートを登録していない場合、
    /// 該当する行動が全くでなきい。
    /// </summary>
    /// 
    public enum AIState_ABType
    {
        E_B_Idle,
        E_B_Attack,
        E_B_Dead,
    }


    public class AITester 
        : StatefulObjectBase<AITester, AIState_ABType>
    {
        public int DDDD;
        void Start()
        {
            //存在していないクラスが指定されたら本体消滅
            if (!AddStateByName("E_B_Idle"))
                Destroy(gameObject);
            if (!AddStateByName("E_B_Attack"))
                Destroy(gameObject);
            if (!AddStateByName("E_B_Dead"))
                Destroy(gameObject);

            //ステートマシーンを自身として設定
            stateMachine = new StateMachine<AITester>();

            //初期起動時は、A_Modeに移行させる
            ChangeState(AIState_ABType.E_B_Idle);
        }
        /// <summary>
        /// クラス名を元にステートを生成して追加する
        /// </summary>
        /// <param name="ClassName">生成するクラスの名前</param>
        public bool AddStateByName(string ClassName)
        {
            try
            {
                // 現在のアセンブリからクラスを取得
                Type StateType = Assembly.GetExecutingAssembly().GetType($"StateMachineAI.{ClassName}");

                // クラスが見つからなかった場合の対処
                if (StateType == null)
                {
                    Debug.LogError($"{ClassName} クラスが見つかりませんでした。");
                    return true;
                }

                // 型が State<AITester> かどうかをチェック
                if (!typeof(State<AITester>).IsAssignableFrom(StateType))
                {
                    Debug.LogError($"{ClassName} は State<EnemyAI> 型ではありません。");
                    return true;
                }

                // インスタンスを生成
                System.Reflection.ConstructorInfo Constructor =
                    StateType.GetConstructor(new[] { typeof(AITester) });
                

                if (Constructor == null)
                {
                    Debug.LogError($"{ClassName} のコンストラクタが見つかりませんでした。");
                    return true;
                }

                State<AITester> StateInstance = 
                    Constructor.Invoke(new object[] { this }) as State<AITester>;

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
                Debug.LogError($"エラーが発生しました。 {ex.Message}");
                return false;
            }
        }

    }
}
