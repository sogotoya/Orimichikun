//回転２段階目（怒っている）時の回転処理
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Second_Roll : MonoBehaviour
{
    [SerializeField]
    [Header("移動ポジション（左から右へ）")]
    Transform[] m_Tf;

    [SerializeField]
    [Header("移動スピード")]
    float m_MoveSpeed = 1f;

    //どのポジション(配列)に移動するか指定する変数
    int m_Index;
    //何回目か判定変数
    int m_Cnt = 0;

    bool m_IsRunning = false;

    /// <summary>
    /// ボスの回転移動攻撃(怒り状態)(処理を実行させたいオブジェクト、何回させるか)
    /// </summary>
    public int AngrylRoll(GameObject obj, int cnt)
    {
        if (m_IsRunning) return 0;
        if(m_Cnt>=cnt)
        {
            //初期化
            m_Cnt = 0;
            m_Index = 0;
            return 100;
        }
        // コルーチンを1回だけ動かす
        StartCoroutine(MoveSequence(obj));
        return 0;
    }

    /// <summary>
    /// 回転移動攻撃(怒り状態)移動処理(処理を実行させたいオブジェクト、何回させるか)
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    IEnumerator MoveSequence(GameObject obj)
    {

        m_IsRunning= true;
        yield return new WaitForSeconds(1.0f);
        Transform next_Point = m_Tf[m_Index];

        //移動処理(目的地に着くまで)
        while (true)
        {
            // 次のポイント
            Transform next = m_Tf[m_Index];

            // 移動
            while (Vector3.Distance(obj.transform.position, next.position) > 0.05f)
            {
                obj.transform.position = Vector3.MoveTowards(
                    obj.transform.position,
                    next.position,
                    m_MoveSpeed * Time.deltaTime
                );
                yield return null;
            }

            // 目的地ついたら
            m_Index++;

            // 配列終わったら1周完了
            if (m_Index >= m_Tf.Length)
            {
                m_Index = 0;
                m_Cnt++;  // 回数増加
                break;    // 一回のループ終了
            }

            // 1番目以外で停止
            if (m_Index != 1)
                yield return new WaitForSeconds(0.4f);
        }

        m_IsRunning = false;
    }


}
