using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class CoinMove : MonoBehaviour
{
    [SerializeField]
    FastMessage m_FM;

    [SerializeField]
    CameraShake m_CameraShake;
    // プレイヤーに近づくスピード
    float m_MoveSpeed = 1.0f;

    // プレイヤーにどこまで近づくか（停止距離）
    float m_StopDistance = 2f;
    [SerializeField]
    GameObject m_Coin;

    [SerializeField]
    GameObject m_Player;
    void Start()
    {
        if (m_Coin == null) return;
        if (m_CameraShake == null) return;
        if(m_FM == null) return;
    }

    void Update()
    {
        if (m_Coin == null)
        {
            MoveStart();
        }
    }

    /// <summary>
    /// 最初の動き
    /// </summary>
    void MoveStart()
    {
        //コルーチン止めるための保存
        Coroutine shakeCoroutine = StartCoroutine(m_CameraShake.Shake(0.5f, 0.1f));

        Vector2 dir = m_Player.transform.position - transform.position;
        //座標が空中になるためYは0
        dir.y = 0f;
        float distance = dir.magnitude;
        //Debug.Log($"距離: {m_Distance}, 停止距離: {m_StopDistance}");

        if (distance > m_StopDistance)
        {
            // 移動
            Vector3 move = dir.normalized * m_MoveSpeed * Time.deltaTime;
            transform.position += move;
        }
        else
        {
            StopCoroutine(shakeCoroutine);
            m_FM.m_MessageFlag = true;
        }
    }
}
