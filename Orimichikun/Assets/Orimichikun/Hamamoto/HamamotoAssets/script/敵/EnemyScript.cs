using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public enum State
    {
        Idle,
        Patrol,
        Chase,
        Attack,
    }

    [Header("移動速度")]
    public float m_moveSpeed = 2f;
    [Header("攻撃範囲")]
    public float m_attackRange = 1.5f;
    [Header("追跡範囲")]
    public float m_chaseRange = 5f;
    [Header("パトロール範囲の左右端")]
    public Transform m_leftPoint;
    public Transform m_rightPoint;

    [Header("プレイヤー")]
    public Transform m_player;

    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;

    private State CurrentState = State.Idle;
    private bool facingRight = true;
    private Vector3 patrolTarget;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();

        // 初期はパトロール開始地点を設定
        patrolTarget = m_leftPoint != null ? m_leftPoint.position : transform.position;
        ChangeState(State.Patrol);
    }

    private void Update()
    {
        switch (CurrentState)
        {
            case State.Idle: ModeIdle(); break;
            case State.Patrol: ModePatrol(); break;
            case State.Chase: ModeChase(); break;
            case State.Attack: ModeAttack(); break;

        }
    }
    
    void ModeIdle()
    {
        m_Animator.SetFloat("Speed", 0f);

        // プレイヤーが近づいたら Chase へ
        if (Vector2.Distance(transform.position, m_player.position) < m_chaseRange)
        {
            ChangeState(State.Chase);
        }
    }

    void ModePatrol()
    {
        m_Animator.SetFloat("Speed", 1f);

        // 左右に歩く
        transform.position = Vector2.MoveTowards(transform.position, patrolTarget, m_moveSpeed * Time.deltaTime);

        // ターゲットに着いたら反転
        if (Vector2.Distance(transform.position, patrolTarget) < 0.1f)
        {
            patrolTarget = (patrolTarget == m_leftPoint.position) ? m_rightPoint.position : m_leftPoint.position;
            Flip();
        }

        // プレイヤーが範囲内に来たら Chase
        if (Vector2.Distance(transform.position, m_player.position) < m_chaseRange)
        {
            ChangeState(State.Chase);
        }
    }

    void ModeChase()
    {
        m_Animator.SetFloat("Speed", 1f);

        // プレイヤー方向へ移動
        Vector2 dir = (m_player.position - transform.position).normalized;
        m_Rigidbody.velocity = new Vector2(dir.x * m_moveSpeed, m_Rigidbody.velocity.y);

        // 向きを反転
        if ((dir.x > 0 && !facingRight) || (dir.x < 0 && facingRight))
        {
            Flip();
        }

        // 攻撃可能距離なら攻撃
        if (Vector2.Distance(transform.position, m_player.position) < m_attackRange)
        {
            ChangeState(State.Attack);
        }
        // 追跡範囲外ならパトロールに戻る
        else if (Vector2.Distance(transform.position, m_player.position) > m_chaseRange * 1.5f)
        {
            ChangeState(State.Patrol);
        }
    }

    void ModeAttack()
    {
        m_Animator.SetTrigger("Attack");

        // 攻撃アニメの終了後に再び Chase に戻る
        StartCoroutine(BackToChase());
    }

    IEnumerator BackToChase()
    {
        // 攻撃モーションの長さに合わせる
        yield return new WaitForSeconds(1f); 
        ChangeState(State.Chase);
    }



    void ChangeState(State newState)
    {
        CurrentState = newState;
    }
//反転の関数 
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_chaseRange);
    }
}
