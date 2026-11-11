//Boss戦時プレイヤーに近づくためのスクリプト
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachBoss : MonoBehaviour
{
    [SerializeField]
    GameObject m_Player;

    [SerializeField, Tooltip("Cinemachine で画面を揺らすコンポーネント")]
    CinemachineShake m_Shake;

    public bool m_ApFlag=false;

    //自分が揺れを始めたか（他の揺れを止めないため）
    bool m_IsShaking = false;

    // プレイヤーに近づくスピード
    float m_MoveSpeed = 1.0f;

    // プレイヤーにどこまで近づくか（停止距離）
    float m_StopDistance = 2f;

    public IEnumerator BossMoveStart()
    {
        Vector2 dir = m_Player.transform.position - transform.position;
        //座標が空中になるためYは0
        dir.y = 0f;
        float distance = dir.magnitude;
        //Debug.Log($"距離: {m_Distance}, 停止距離: {m_StopDistance}");

        if (distance > m_StopDistance)
        {
            // 移動（移動中は 0.5 秒後から揺らし続ける）
            if (!m_IsShaking)
            {
                m_IsShaking = true;
                m_Shake.StartShake(0.5f);
            }
            Vector3 move = dir.normalized * m_MoveSpeed * Time.deltaTime;
            transform.position += move;
        }
        else//指定した地点についた
        {
            //自分で始めた揺れだけ止める
            if (m_IsShaking)
            {
                m_IsShaking = false;
                m_Shake.StopShake();
            }
            yield return new WaitForSeconds(1f);
            m_ApFlag = true;
            Debug.Log("到着");
        }
    }
}
