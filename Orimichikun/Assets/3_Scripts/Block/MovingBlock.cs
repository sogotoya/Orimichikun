using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovingBlock : MonoBehaviour
{
    [Header("移動する方向(1, 0) = 右 / (0, 1) = 上")]
    public Vector2 m_moveDirection = Vector2.right;
    [Header("移動する距離")]
    public float m_moveDistance = 3f;
    [Header("移動速度")]
    public float m_moveSpeed = 3f;
    //初期位置
    private Vector3 m_startPos;
    //移動の目的地
    private Vector3 m_targetPos;
    //進行方向フラグ
    private bool m_GoingToTarget = true;
    private void Start()
    {
        //初期位置
        m_startPos = transform.position;
        //方向と距離を決める
        m_targetPos=m_startPos + (Vector3)(m_moveDirection.normalized * m_moveDistance);
    }
    private void Update()
    {
        //現在の目標位置を決定
        Vector3 destination=m_GoingToTarget ? m_startPos : m_targetPos;
        // 移動
        transform.position = Vector3.MoveTowards(
            transform.position,
            destination,
            m_moveSpeed * Time.deltaTime
        );

        // 到達したら方向を反転
        if (Vector3.Distance(transform.position, destination) < 0.01f)
        {
            m_GoingToTarget = !m_GoingToTarget;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //playerがcollisionを踏んだら
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            //playerを床にくっつける
            collision.transform.SetParent(transform);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        //playerがcollisionを踏んだら
        if (collision.gameObject.CompareTag("Player"))
        {
            //playerを床から離れる
            collision.transform.SetParent(null);
        }
    }
}


