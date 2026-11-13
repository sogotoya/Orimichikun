//using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

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
    public RespawnPoint[] m_RespawnPoint;
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
    [Header("SavePointのスクリプト")]
    public SavePoint m_SavePoint;
    [Header("CoinCountManagerのscript")]
    public CoinCountManager m_Manager;

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
    private bool m_isTouchingWall;
    private Vector2 m_wallNormal;
    private Vector3 m_LastSavePosition;
    public bool m_TPPush = false;

    //復活中フラグToya
    private bool m_IsRespawning = false;
    //死んでいて、コンティニューを待っているか。右下のキー表示（KeyHintUI）で使う
    public bool IsDead => CurrentState == State.Die && !m_IsRespawning;
    //ゲームオーバー表示が出て、コンティニューできるか。右下のキー表示（KeyHintUI）で使う
    public bool CanContinue => IsDead && m_GameOver != null && m_GameOver.IsReady;
    //死んだときの演出（カメラ揺れ・SE・BGM・ゲームオーバー表示）
    private GameOverEffect m_GameOver;

    // スマホ操作用の水平入力
    private float m_MobileHorizontal = 0f;

    private void Start()
    {
        m_LastSavePosition = transform.position;
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        //物理の動きを描画の間で補間して、移動とカメラのカクつきを減らす
        if (m_Rigidbody != null) m_Rigidbody.interpolation = RigidbodyInterpolation2D.Interpolate;
        m_Parameta = GetComponent<Parameta2D>();
        m_Audio = GetComponent<AudioSource>();
        //死んだときの演出を付ける
        m_GameOver = GetComponent<GameOverEffect>();
        if (m_GameOver == null) m_GameOver = gameObject.AddComponent<GameOverEffect>();

        // UI は消す
        if (m_image != null) m_image.SetActive(false);
        //コントローラー接続中はゲームオーバーのコンティニュー表示を LT にする
        KeyGuideIcon.Attach(m_image, "", "KeyGuide/GameOver_LT");

        // 移動の制約だけ初期化
        if (m_Rigidbody != null)
        {
            m_Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            m_Rigidbody.velocity = Vector2.zero;
        }
        CurrentState = State.Idle;
        if (m_Parameta != null && SceneManager.GetActiveScene().name == "Stage")
        {
            m_Parameta.m_Hp = m_Parameta.m_MaxHp;
        }

        //中間地点を取っていたら、Stage に戻ったときはそこから始める（ボス戦で死んだときなど）
        if (MidPoint.HasSavedPosition && SceneManager.GetActiveScene().name == "Stage")
        {
            Vector3 pos = MidPoint.SavedPosition;
            pos.z = transform.position.z;
            transform.position = pos;
            if (m_Rigidbody != null) m_Rigidbody.position = pos;
            m_LastSavePosition = pos;

            //カメラもすぐに合わせる
            PlayerCamera playerCamera = GetComponentInChildren<PlayerCamera>();
            if (playerCamera == null) playerCamera = FindObjectOfType<PlayerCamera>();
            if (playerCamera != null) playerCamera.SnapToPlayer();
        }
      
    }
    private void Update()
    {
     
        if (m_IsRespawning) return;


        // 入力取得
        moveX = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(m_MobileHorizontal) > 0.01f) moveX = m_MobileHorizontal;

        // 接地判定（Raycast のみ使用）
        isGrounded = CheckGrounded();


        // 接地したらジャンプ回数リセット（跳んだ直後の上昇中はまだ地面判定に当たるのでリセットしない）
        if (isGrounded && (m_Rigidbody == null || m_Rigidbody.velocity.y <= 0.01f))
        {
            jumpCount = 0;
        }

        // 今回の接地判定を保存
        wasGrounded = isGrounded;

        if (m_Parameta != null && m_Parameta.m_Hp <= 0)
        {
            LockFall();
        }
        if (transform.position.y < m_deathY)
        {
            LockFall();
        }
        //死んでいる間はダメージのアニメーションで死亡アニメーションが止まらないようにする
        if (CurrentState == State.Die && m_Animator != null) m_Animator.ResetTrigger("Damage");

        //コンティニューは Enter キーかコントローラーの LT（ゲームオーバー表示が出てから）
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 6") || Input.GetKeyDown("joystick button 7") || ControllerCheck.IsLTDown())
    && CurrentState == State.Die
    && !m_IsRespawning
    && (m_GameOver == null || m_GameOver.IsReady))
        {
            m_Manager.m_CoinReset = true;
            PlayerSpawn();
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
        if (m_Animator != null) m_Animator.SetFloat("Speed", 0f);

        // 入力があるならMoveに切り替え
        if (Mathf.Abs(moveX) > 0f)
        {
            ChangeState(State.Move);
        }
        //Input.GetKeyDown(KeyCode.Space)
        //地面を踏んでいてspaceキーを押したらジャンプする
        if ((Input.GetKeyDown("joystick button 2") || Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.Space)) && jumpCount < maxJumpCount)
        {
            if (m_jump != null && m_jump.Length > 0)
            {
                int index = Mathf.Clamp(jumpCount, 0, m_jump.Length - 1);
                if (m_Audio != null && m_jump[index] != null)
                {
                    m_Audio.PlayOneShot(m_jump[index]);
                }
            }
            // 上方向に速度を与える
            if (m_Rigidbody != null) m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, m_jumpForce);
            // ジャンプ回数を増やす
            jumpCount++;
            ChangeState(State.Jump);
        }
    }

    void ModeMove()
    {
        // 壁に接触しており 同じ方向に入力している場合
        if (m_isTouchingWall && Mathf.Sign(moveX) == -Mathf.Sign(m_wallNormal.x))
        {
            moveX = 0; // 壁方向入力を無効化
        }
        // Rigidbody2D の速度設定（Y速度は維持）
        m_Rigidbody.velocity = new Vector2(moveX * m_runSpeed, m_Rigidbody.velocity.y);

        // キャラの向き反転
        if (moveX > 0 && !facingRight) Flip();
        else if (moveX < 0 && facingRight) Flip();

        // Blend Tree用パラメータ（正規化）
        if (m_Animator != null) m_Animator.SetFloat("Speed", Mathf.Abs(moveX));

        // 入力がなくなったらIdleに戻す
        if (Mathf.Abs(moveX) < 0.01f)  // 少し余裕を持たせる
        {
            ChangeState(State.Idle);
        }
        //地面を踏んでいてspaceキーを押したらジャンプする
        if ((Input.GetKeyDown("joystick button 2") || Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.Space)) && jumpCount < maxJumpCount)
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
            if (m_Rigidbody != null) m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, m_jumpForce);
            // ジャンプ回数を増やす
            jumpCount++;
            ChangeState(State.Jump);
        }

    }
    void ModeJump()
    {
        // 壁に接触しており 同じ方向に入力している場合
        if (m_isTouchingWall && Mathf.Sign(moveX) == -Mathf.Sign(m_wallNormal.x))
        {
            moveX = 0; // 壁方向入力を無効化
        }
        // 空中でも左右移動できるようにする
        m_Rigidbody.velocity = new Vector2(moveX * m_runSpeed, m_Rigidbody.velocity.y);

        // 向き反転
        if (moveX > 0 && !facingRight) Flip();
        else if (moveX < 0 && facingRight) Flip();

        // アニメーターに速度を反映（空中でも動きがある時にアニメーション変える用）
        if (m_Animator != null) m_Animator.SetFloat("Speed", Mathf.Abs(moveX));

        //空中でもジャンプ（二段ジャンプ）
        if ((Input.GetKeyDown("joystick button 2") || Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.Space)) && jumpCount < maxJumpCount)
        {
            if (m_jump != null && m_jump.Length > 0)
            {
                int index = Mathf.Clamp(jumpCount, 0, m_jump.Length - 1);
                if (m_Audio != null && m_jump[index] != null)
                {
                    m_Audio.PlayOneShot(m_jump[index]);
                }
            }
            m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, m_jumpForce);
            jumpCount++;
            return;
        }

        // 状態をIdleに戻すのは「着地したら」（上昇中は戻さない）
        //（前は毎フレーム CheckLanding のコルーチンを起動していて、空中にいる間どんどん増えて重くなっていた）
        if (isGrounded && m_Rigidbody.velocity.y <= 0.01f)
        {
            ChangeState(State.Idle);
        }
    }
    void ModeAttack() { }

    void ModeDie() { }
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
        //死んだ瞬間かどうか（LockFall は死んでいる間ずっと呼ばれるため）
        bool justDied = CurrentState != State.Die;

        // 移動やジャンプを止める
        CurrentState = State.Die;
        if (m_Rigidbody != null)
        {
            m_Rigidbody.velocity = Vector2.zero;

            // X方向の移動も凍結して落下のみ
            m_Rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }

        // 死んだ瞬間だけ、死亡アニメーションと演出（ゲームオーバー表示は演出の中で少し遅れて出す）
        if (justDied)
        {
            if (m_Animator != null) m_Animator.SetTrigger("Die");
            if (m_GameOver != null) m_GameOver.Play(m_image);
            else if (m_image != null) m_image.SetActive(true);
        }
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
    //壁のcollisionに触れたら
    private void OnCollisionStay2D(Collision2D collision)
    {
        //すべての接触点を1つずつ取り出してその接触点の向きを調べている
        foreach (var contact in collision.contacts)
        {
            // 左右方向の壁か判定
            if (Mathf.Abs(contact.normal.x) > 0.5f)
            {
                m_isTouchingWall = true;
                m_wallNormal = contact.normal;
                return;
            }
        }
    }
    //壁のcollisionに触れたら
    private void OnCollisionExit2D(Collision2D collision)
    {
        m_isTouchingWall = false;
    }
    //セーブポイントに触れたらプレイヤーのスポーンの位置変更
    public void UpdateSavePoint(Vector3 pos)
    {
        m_LastSavePosition = pos;
    }
    public void PlayerSpawn()
    {
        if (SceneManager.GetActiveScene().name=="BossStage")
        {
            m_Parameta.m_Hp = m_Parameta.m_MaxHp;
            SceneManager.LoadScene("Stage");
            transform.position = m_LastSavePosition;
            foreach (var point in m_RespawnPoint)
                point?.Respawn();
        }

        //死んだときの演出を止める（ボス戦からは Stage を読み直すので、BGMは流し直さない）
        if (m_GameOver != null) m_GameOver.Stop(SceneManager.GetActiveScene().name != "BossStage");
        //死亡アニメーションから元に戻す
        if (m_Animator != null)
        {
            m_Animator.ResetTrigger("Die");
            m_Animator.Play("Blend Tree", 0, 0f);
        }

        // 通常のセーブポイントリスポーン
        if (m_image != null) m_image.SetActive(false);
        transform.position = m_LastSavePosition;
        //物理側の位置も合わせる（補間をオンにしているため、transform だけだと戻されることがある）
        if (m_Rigidbody != null) m_Rigidbody.position = m_LastSavePosition;
        foreach (var point in m_RespawnPoint)
            point?.Respawn();

        if (m_Rigidbody != null)
        {
            m_Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            m_Rigidbody.velocity = Vector2.zero;
        }
        CurrentState = State.Idle;
        if (m_Parameta != null) m_Parameta.m_Hp = m_Parameta.m_MaxHp;

        m_IsRespawning = false;

        //カメラをリスポーン先へ一気に合わせる（ズームアウトしたまま動いて背景の外が見えるのを防ぐ）
        PlayerCamera playerCamera = GetComponentInChildren<PlayerCamera>();
        if (playerCamera == null) playerCamera = FindObjectOfType<PlayerCamera>();
        if (playerCamera != null) playerCamera.SnapToPlayer();

        Debug.Log("リスポーン！");
    }    /// <summary>
    /// スマホUIから移動入力を受け取る関数
    /// </summary>
    public void SetMobileInput(float horizontalInput)
    {
        m_MobileHorizontal = horizontalInput;
    }

    /// <summary>
    /// スマホUIからジャンプを呼び出すための関数
    /// </summary>
    public void MobileJump()
    {
        if (CurrentState == State.Die || m_IsRespawning) return;

        if (jumpCount < maxJumpCount)
        {
            if (m_jump != null && m_jump.Length > 0)
            {
                int index = Mathf.Clamp(jumpCount, 0, m_jump.Length - 1);
                if (m_Audio != null && m_jump[index] != null)
                {
                    m_Audio.PlayOneShot(m_jump[index]);
                }
            }
            if (m_Rigidbody != null) m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, m_jumpForce);
            jumpCount++;
            ChangeState(State.Jump);
        }
    }

    // スマホ操作用の攻撃フラグ
    private bool m_MobileAttackFlag = false;

    /// <summary>
    /// スマホUIから攻撃を呼び出すための関数
    /// </summary>
    public void MobileAttack()
    {
        if (CurrentState == State.Die || m_IsRespawning) return;
        m_MobileAttackFlag = true;
    }

    /// <summary>
    /// モバイル攻撃フラグを取得してリセットする（playershootから呼ばれる）
    /// </summary>
    public bool ConsumeMobileAttack()
    {
        if (m_MobileAttackFlag)
        {
            m_MobileAttackFlag = false;
            return true;
        }
        return false;
    }
}
