using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayScript : MonoBehaviour
{
    //状態の定義
    public enum State
    {
        Idle,
        Move,
        Jump,
        Attack,
        Damage
    }

    [Header("移動速度")]
    public float m_runSpeed = 6f;
    [Header("ジャンプ力")]
    public float m_jumpForce = 7f;
    [Header("地面判定用レイヤー")]
    public LayerMask m_Layer;
    [Header("地面チェック位置")]
    public Transform m_Ground;
    [Header("着地判定")]
    public float m_groundCheck;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    //左右入力値
    private float moveX;
    //現在の状態
    private State CurrentState = State.Idle;
    //キャラクターの向きは右向きか
    private bool facingRight = true;
    //着地しているか？
    private bool isGrounded;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // 入力取得
        moveX = Input.GetAxisRaw("Horizontal");

        // 接地判定
        isGrounded = Physics2D.OverlapCircle(m_Ground.position, m_groundCheck, m_Layer);
        // ステート処理
        switch (CurrentState)
        {
            case State.Idle:
                ModeIdle();
                break;
            case State.Move:
                ModeMove();
                break;
            case State.Jump:
                ModeJump();
                break;
            case State.Attack:
                ModeAttack();
                break;
            case State.Damage:
                ModeDamage();
                break;
        }
    }

    void ModeIdle()
    {
        m_Animator.SetFloat("Speed", 0f);

        // 入力があるならMoveに切り替え
        if (Mathf.Abs(moveX) > 0f)
        {
            ChangeState(State.Move);
        }
        //地面を踏んでいてspaceキーを押したらジャンプする
        if (Input.GetKeyDown(KeyCode.Space)&&isGrounded)
        {
            ChangeState(State.Jump);
        }
    }

    void ModeMove()
    {
        // Rigidbody2D の速度設定（Y速度は維持）
        m_Rigidbody.velocity = new Vector2(moveX * m_runSpeed, m_Rigidbody.velocity.y);

        // キャラの向き反転
        if (moveX > 0 && !facingRight) Flip();
        else if (moveX < 0 && facingRight) Flip();

        // Blend Tree用パラメータ（正規化）
        m_Animator.SetFloat("Speed", Mathf.Abs(moveX));

        // 入力がなくなったらIdleに戻す
        if (Mathf.Abs(moveX) < 0.01f)  // 少し余裕を持たせる
        {
            ChangeState(State.Idle);
        }
        //地面を踏んでいてspaceキーを押したらジャンプする
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            ChangeState(State.Jump);
        }
        
    }
    void ModeJump()
    {
        // 上方向に速度を与える
        m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, m_jumpForce);

        // アニメーション用
        m_Animator.SetTrigger("Jump");

        // 状態をIdleに戻すのは「着地したら」
        StartCoroutine(CheckLanding());
    }
    void ModeAttack() { }
    void ModeDamage() { }
    //ステートチェンジ関数
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
    IEnumerator CheckLanding()
    {
        // 空中にいる間は待機
        yield return new WaitUntil(() => isGrounded);

        // 着地したらIdleに戻す
        ChangeState(State.Idle);
    }

}
