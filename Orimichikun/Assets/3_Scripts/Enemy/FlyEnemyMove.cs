using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemyMove : MonoBehaviour
{
    [Header("プレイヤー")]
    public GameObject m_Player;
    [Header("移動方向 (例: 上 Vector2.up / 右 Vector2.right)")]
    public Vector2 m_MoveDirection = Vector2.up;

    [Header("移動する距離")]
    public float m_MoveDistance = 3f;

    [Header("移動する速度")]
    public float m_MoveSpeed = 3f;

    // 初期位置（片側の端）
    private Vector3 m_StartPos;
    // もう片側の端
    private Vector3 m_TargetPos;
    //停止フラグ
    private bool m_AttackMoveStop;
    // 現在どちら方向へ移動しているか
    private bool m_GoingToTarget = true;
    private Parameta2D m_Parameta;

    void Start()
    {
        if (m_Player==null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            if (obj != null)
            {
                m_Player = obj;
            }
        }
        m_Parameta=GetComponent<Parameta2D>();
        // 初期位置を保存
        m_StartPos = transform.position;

        // 方向を正規化し距離を反映した移動点を設定
        m_TargetPos = m_StartPos + (Vector3)(m_MoveDirection.normalized * m_MoveDistance);
    }

    void Update()
    {
        if (m_Parameta.m_Hp==0)
        {
            m_AttackMoveStop = true;
        }
        if (m_Player != null)
        {
            if (m_Player.transform.position.x<transform.position.x)
            {
                transform.localScale = new Vector3(1,1,1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        //攻撃中ならストップ
        if (m_AttackMoveStop) return;
        // どちらの位置へ向かうか判定
        Vector3 destination = m_GoingToTarget ? m_TargetPos : m_StartPos;

        // 移動処理
        transform.position = Vector3.MoveTowards(
            transform.position,
            destination,
            m_MoveSpeed * Time.deltaTime
        );

        // 目的地に到達したら方向を反転
        if (Vector3.Distance(transform.position, destination) < 0.01f)
        {
            m_GoingToTarget = !m_GoingToTarget;
        }
    }
    //アニメーションイベントで関数を処理
    void AttackStart()
    {
       m_AttackMoveStop = true;
    }
    void AttackEnd()
    {
        m_AttackMoveStop = false;
    }
}
