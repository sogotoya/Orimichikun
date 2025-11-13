using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpPosition : MonoBehaviour
{
    [SerializeField]
    [Header("空中の飛ぶ高さ")]
    //空中高さを決めるTransform
    Transform m_Tf; 

    [SerializeField]
    [Header("着地ポイント")]
    //着地点を決めるTransform（Y座標用）
    Transform m_Tfpt; 

    [SerializeField]
    [Header("移動スピード")]
    float m_MoveSpeed = 1f;

    [SerializeField]
    [Header("プレイヤー")]
    Transform m_PlayerTf;
    //現在の目的地
    Vector3 m_Position = Vector3.zero; 

    bool m_Flag;
    //同時に複数コルーチン起動しないよう制御
    bool m_IsJumping = false; 

    public IEnumerator JumpAttackStart(GameObject obj,System.Action<int>callback)
    {

        //プレイヤーの上の高さへジャンプ
        m_Position = m_PlayerTf.position;
        // 飛ぶ高さに設定
        m_Position.y = m_Tf.position.y; 

        while (Vector3.Distance(obj.transform.position, m_Position) > 0.05f)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, m_Position, m_MoveSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        //着地ポイントの高さへ落下
        m_Position.y = m_Tfpt.position.y; 

        while (Vector3.Distance(obj.transform.position, m_Position) > 0.05f)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, m_Position, m_MoveSpeed * Time.deltaTime);
            yield return null;
        }

        //完了フラグ
        callback?.Invoke(100);
    }
}
