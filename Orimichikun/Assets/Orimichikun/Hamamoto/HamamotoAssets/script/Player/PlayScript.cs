using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayScript : MonoBehaviour
{
    //状態の定義
    public enum State
    {
        Idle,
        Move,
        Jump,
        Attack,
        Die,
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
    [Header("最大ジャンプ回数")]
    public int maxJumpCount = 2;
    [Header("落下判定")]
    public float m_deathY = -10f;
    [Header("UI画像")]
    public GameObject m_image;
    [Header("jumpの音")]
    public AudioClip[] m_jump;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private Parameta2D m_Parameta;
    private AudioSource m_Audio;
    // 何回ジャンプしたか
    private int jumpCount = 0; 
    //左右入力値
    private float moveX;
    //現在の状態
    private State CurrentState = State.Idle;
    //キャラクターの向きは右向きか
    private bool facingRight = true;
    //着地しているか？
    private bool isGrounded;
    // 地面にいたかどうか
    private bool wasGrounded = false;  

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Parameta = GetComponent<Parameta2D>();
        m_Audio = GetComponent<AudioSource>();
        m_image.SetActive(false);
    }

    private void Update()
    {
        // 入力取得
        moveX = Input.GetAxisRaw("Horizontal");

        // 接地判定（Raycast のみ使用）
        isGrounded = CheckGrounded();

        
        // 接地したらジャンプ回数リセット
        if (isGrounded)
        {
            jumpCount = 0;
        }

        // 今回の接地判定を保存
        wasGrounded = isGrounded;

        if (m_Parameta.m_Hp <= 1)
        {
            LockFall();
        }
        if (transform.position.y < m_deathY)
        {
            LockFall();
        }
        if (CurrentState == State.Die && Input.GetKeyDown(KeyCode.Return))
        {
            Respawn();
        }

        if (CurrentState != State.Die)
        {
            switch (CurrentState)
            {
                case State.Idle: ModeIdle(); break;
                case State.Move: ModeMove(); break;
                case State.Jump: ModeJump(); break;
                case State.Attack: ModeAttack(); break;
                case State.Die: ModeDie(); break;
                case State.Damage: ModeDamage(); break;
            }
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
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumpCount)
        {
              // 上方向に速度を与える
            m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, m_jumpForce);
            // ジャンプ回数を増やす
            jumpCount++;
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
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumpCount)
        {
            if (m_jump != null && m_jump.Length > 0)
            {
                int index = Mathf.Clamp(jumpCount - 1, 0, m_jump.Length - 1);
                if (m_jump[index] != null)
                {
                    m_Audio.PlayOneShot(m_jump[index]);
                }
            }
            // 上方向に速度を与える
            m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, m_jumpForce);
            // ジャンプ回数を増やす
            jumpCount++;
            ChangeState(State.Jump);
        }
        
    }
    void ModeJump()
    {
        // 空中でも左右移動できるようにする
        m_Rigidbody.velocity = new Vector2(moveX * m_runSpeed, m_Rigidbody.velocity.y);

        // 向き反転
        if (moveX > 0 && !facingRight) Flip();
        else if (moveX < 0 && facingRight) Flip();

        // アニメーターに速度を反映（空中でも動きがある時にアニメーション変える用）
        m_Animator.SetFloat("Speed", Mathf.Abs(moveX));

        // 状態をIdleに戻すのは「着地したら」
        StartCoroutine(CheckLanding());
    }
    void ModeAttack() { }

    void ModeDie(){}
    void ModeDamage()
    {

    }
    
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
    void LockFall()
    {
        //移動やジャンプを止める
        CurrentState=State.Die;
        // X方向の移動を無効化して、重力だけで落下させる
        m_Rigidbody.velocity = new Vector2(0,0);

        // もし水平移動も一切できないようにしたいなら：
        m_Rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        // UIを表示
        m_image.SetActive(true);
    }
    void Respawn()
    {
        // 座標を初期位置へ
        transform.position = Vector3.zero;

        // Rigidbodyの制約を解除
        m_Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        // StateをIdleに戻す
        CurrentState = State.Idle;
        m_Parameta.m_Hp=m_Parameta.m_MaxHp;
        // UIを消す
        m_image.SetActive(false);

  

    }
    bool CheckGrounded()
    {
        // 足元から下方向にRayを飛ばす
        RaycastHit2D hit = Physics2D.Raycast(m_Ground.position, Vector2.down, m_groundCheck, m_Layer);
        return hit.collider != null;
    }
    public bool IsFacingRight()
    {
        return facingRight;
    }
    IEnumerator CheckLanding()
    {
        // 空中にいる間は待機
        yield return new WaitUntil(() => isGrounded);

        // 着地したらIdleに戻す
        ChangeState(State.Idle);
    }

}
