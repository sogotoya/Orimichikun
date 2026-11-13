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
    //上に乗っているもの（プレイヤーや敵）
    private readonly List<Rigidbody2D> m_Riders = new List<Rigidbody2D>();
    //自分の当たり判定
    private Collider2D m_Collider;
    //自分の Rigidbody2D（あれば物理で動かす）
    private Rigidbody2D m_Body;

    private void Awake()
    {
        m_Collider = GetComponent<Collider2D>();
        m_Body = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// 物理の更新に合わせてブロックを動かし、上に乗っているものも同じだけ動かす処理
    /// （前はプレイヤーをブロックの子にしていたが、物理で動くプレイヤーと食い違って動きが鈍くなっていた）
    /// </summary>
    private void FixedUpdate()
    {
        //現在の目標位置を決定
        Vector3 destination=m_GoingToTarget ? m_startPos : m_targetPos;
        Vector3 before = transform.position;
        // 移動
        Vector3 next = Vector3.MoveTowards(
            before,
            destination,
            m_moveSpeed * Time.fixedDeltaTime
        );
        if (m_Body != null) m_Body.MovePosition(next);
        else transform.position = next;

        //乗っているものを同じだけ動かす（縦の動きだけ。横に運ぶと流されてしまうので運ばない）
        Vector2 delta = next - before;
        delta.x = 0f;
        for (int i = m_Riders.Count - 1; i >= 0; i--)
        {
            Rigidbody2D rider = m_Riders[i];
            if (rider == null) { m_Riders.RemoveAt(i); continue; }
            rider.position += delta;
        }

        // 到達したら方向を反転
        if (Vector3.Distance(next, destination) < 0.01f)
        {
            m_GoingToTarget = !m_GoingToTarget;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        //playerか敵が上に乗っていたら一緒に動かす
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Enemy")) return;
        Rigidbody2D rb = collision.rigidbody;
        if (rb == null) return;

        bool onTop = IsOnTop(collision.collider);
        if (onTop && !m_Riders.Contains(rb)) m_Riders.Add(rb);
        else if (!onTop) m_Riders.Remove(rb);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        //床から離れたら一緒に動かさない
        if (collision.rigidbody != null) m_Riders.Remove(collision.rigidbody);
    }

    /// <summary>
    /// 相手がブロックの上に乗っているか調べる処理（横からぶつかったときは乗っていない）
    /// </summary>
    private bool IsOnTop(Collider2D other)
    {
        if (m_Collider == null) return true;
        return other.bounds.min.y >= m_Collider.bounds.max.y - 0.1f;
    }
}

