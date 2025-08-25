using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBlock : MonoBehaviour
{
    [Header("落ちるまでの時間")]
    public float m_dropTime = 2f;
    [Header("落ちた後に復活の時間")]
    public float m_respawnDelay = 5f;
    private Rigidbody2D m_rb;
    //落ちているか？
    private bool m_isDropping = false;
    //初期位置
    private Vector3 m_startPos;
    private void Start()
    {
        m_startPos = transform.position;
        m_rb = GetComponent<Rigidbody2D>();
        // 最初は落ちない
        m_rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //playerがcollisionを踏んだら
        if (!m_isDropping&&collision.gameObject.CompareTag("Player"))
        {
            //落下の準備開始！
            StartCoroutine(DropAfterDelay());
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
    private IEnumerator DropAfterDelay()
    {
        m_isDropping = true;
        //二秒後にブロック落とす
        yield return new WaitForSeconds(m_dropTime);

        // RigidbodyをDynamicにして落下させる
        m_rb.bodyType = RigidbodyType2D.Dynamic;

        // 一定時間後に元の位置に戻す
        yield return new WaitForSeconds(m_respawnDelay);

        Respawn();
    }
    private void Respawn()
    {
        // 位置をリセット
        transform.position = m_startPos;
        transform.rotation = Quaternion.identity;

        // 速度をリセット
        m_rb.velocity = Vector2.zero;
        m_rb.angularVelocity = 0f;

        // 物理を止めて固定
        m_rb.bodyType = RigidbodyType2D.Kinematic;

        // 再び落下できるようにフラグを戻す
        m_isDropping = false;
    }
}
