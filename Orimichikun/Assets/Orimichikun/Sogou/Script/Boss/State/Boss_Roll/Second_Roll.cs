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



    private void Update()
    {
        //if (!m_IsMoving)
        //    AngrylRoll(m_Prefab);
    }

    /// <summary>
    /// ボスの回転移動攻撃(怒り状態)
    /// </summary>
    public void AngrylRoll(GameObject obj)
    {
        if (!m_IsMoving)
        {
            StartCoroutine(MoveSequence(obj));
        }
    }

    /// <summary>
    /// 回転移動攻撃(怒り状態)移動処理
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
        }
        //端の地点以外待機時間付与
        if (m_Index != 1)
        {
            yield return new WaitForSeconds(0.4f);
            //Debug.Log("ていしーーーー");
        }

        m_IsMoving = false;
    }
}
