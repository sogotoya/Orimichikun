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

    bool m_IsMoving = false;
    //
    bool m_Flag;
    //何回目か判定変数
    int m_Cnt = 0;

    private void Update()
    {
        //if (!m_IsMoving)
        //    AngrylRoll(m_Prefab);
    }

    /// <summary>
    /// ボスの回転移動攻撃(怒り状態)(処理を実行させたいオブジェクト、何回させるか)
    /// </summary>
    public int AngrylRoll(GameObject obj, int cnt)
    {
        StartCoroutine(SequenceMove());

        if (!m_IsMoving && m_Cnt != cnt)
        {
            StartCoroutine(MoveSequence(obj));
            return 0;
        }
        else
        {
            //初期化
            m_Cnt = 0;
            m_Flag = false;
            return 100;
        }
    }

    /// <summary>
    /// 回転移動攻撃(怒り状態)移動処理(処理を実行させたいオブジェクト、何回させるか)
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    IEnumerator MoveSequence(GameObject obj)
    {

        m_IsMoving = true;

        Transform next_Point = m_Tf[m_Index];

        //移動処理(目的地に着くまで)
        while (Vector3.Distance(obj.transform.position, next_Point.position) > 0.05f)
        {
            obj.transform.position = Vector3.MoveTowards(
                obj.transform.position,
                next_Point.position,
                m_MoveSpeed * Time.deltaTime
            );
            //1フレーム待機
            yield return null;
        }
        //目的地に着いたら
        m_Index++;


        if (m_Index >= m_Tf.Length)
        {
            m_Index = 0;
            m_Cnt++;
        }
        //端の地点以外待機時間付与
        if (m_Index != 1)
        {
            yield return new WaitForSeconds(0.4f);
            //Debug.Log("ていしーーーー");
        }
        m_IsMoving = false;
        yield return 0;
    }


    /// <summary>
    /// 動かさない時間
    /// </summary>
    /// <returns></returns>
    IEnumerator SequenceMove()
    {
        if (!m_Flag)
        {
            yield return new WaitForSeconds(1.0f);
            m_Flag = true;
        }

    }
}
