using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class CoinMove : MonoBehaviour
{
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
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Coin == null)
        {
            MoveStart();
        }
    }

    /// <summary>
    /// 起動時の移動
    /// </summary>
    void MoveStartup()
    {
        //Camera cam = Camera.ma;
    }


    /// <summary>
    /// 最初の動き
    /// </summary>
    void MoveStart()
    {
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

        }
    }
}
