//タイトル冒頭: ボスがプレイヤーからコインを奪う演出
//攻撃モーション → 突進した瞬間フェードアウト → 暗転中にボスを戻してコインを持たせる
//→ フェードイン → プレイヤーが被ダメ演出
//FastMessage の最初の会話の後に Play() が呼ばれる
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CoinSteal : MonoBehaviour
{
    [Header("ボス")]
    [SerializeField, Tooltip("動かすボスの親（未設定ならこのオブジェクト）")]
    Transform m_Boss;
    [SerializeField]
    Animator m_BossAnimator;
    [SerializeField, Tooltip("攻撃モーションのトリガー（Roll_1 = 転がり攻撃の構え → 転がり）")]
    string m_AttackTrigger = "Roll_1";
    [SerializeField, Tooltip("演出後に戻すトリガー")]
    string m_IdleTrigger = "Idle";
    [SerializeField, Tooltip("攻撃の構えから突進し始めるまでの時間")]
    float m_WindupTime = 1.0f;
    [SerializeField, Tooltip("突進の速さ")]
    float m_DashSpeed = 12f;
    [SerializeField, Tooltip("突進で止まるプレイヤーとの距離")]
    float m_DashStopDistance = 0.6f;
    [SerializeField, Tooltip("ボスが持つコイン（暗転中に表示）")]
    GameObject m_EnemyCoin;

    [Header("プレイヤー")]
    [SerializeField]
    Transform m_Player;
    [SerializeField]
    Animator m_PlayerAnimator;
    [SerializeField]
    SpriteRenderer m_PlayerSprite;
    [SerializeField]
    string m_DamageTrigger = "Damage";
    [SerializeField, Tooltip("暗転中にプレイヤーを後ろへ下げる距離")]
    float m_KnockBack = 0.6f;
    [SerializeField, Tooltip("被ダメで点滅する時間")]
    float m_BlinkTime = 0.8f;

    [Header("暗転")]
    [SerializeField, Tooltip("暗転用パネルの Image（Canvas(暗転用)/Panel）")]
    Image m_FadeImage;
    [SerializeField, Tooltip("暗転の色")]
    Color m_FadeColor = Color.black;
    [SerializeField, Tooltip("突進し始めてからフェードアウトし始めるまでの時間")]
    float m_FadeDelay = 0.05f;
    [SerializeField]
    float m_FadeOutTime = 0.15f;
    [SerializeField, Tooltip("真っ暗なままの時間")]
    float m_BlackTime = 0.5f;
    [SerializeField]
    float m_FadeInTime = 0.5f;

    [Header("音・揺れ（任意）")]
    [SerializeField]
    AudioSource m_HitSE;
    [SerializeField]
    CinemachineShake m_Shake;

    public bool IsPlaying { get; private set; }

    //暗転パネルを元に戻すための保存
    Color m_PanelColor;
    bool m_PanelWasActive;
    Sprite m_PanelSprite;
    Vector2 m_PanelOffsetMin;
    Vector2 m_PanelOffsetMax;
    bool m_PanelPrepared;

    //暗転パネルを画面より広げる量（端の取りこぼし防止）
    const float k_FadeOverscan = 50f;

    //演出中に物理を止めたボスの Rigidbody2D と、その元の状態
    Rigidbody2D[] m_BossBodies;
    RigidbodyType2D[] m_BossBodyTypes;
    Vector3[] m_BossBodyLocalPos;

    private void Awake()
    {
        if (m_Boss == null) m_Boss = transform;
    }

    /// <summary>
    /// 演出を最初から最後まで再生する（終わるまで yield で待てる）
    /// </summary>
    public IEnumerator Play()
    {
        IsPlaying = true;
        if (m_FadeImage != null)
        {
            m_PanelColor = m_FadeImage.color;
            m_PanelWasActive = m_FadeImage.gameObject.activeSelf;
            m_PanelSprite = m_FadeImage.sprite;
            m_PanelOffsetMin = m_FadeImage.rectTransform.offsetMin;
            m_PanelOffsetMax = m_FadeImage.rectTransform.offsetMax;
            m_PanelPrepared = false;
        }
        Vector3 bossStart = m_Boss.position;

        // 演出中はボスが落ちたりプレイヤーを押したりしないよう、物理と当たり判定を止める
        FreezeBoss(true);

        // 1. 攻撃の構え
        if (m_BossAnimator != null) m_BossAnimator.SetTrigger(m_AttackTrigger);
        yield return new WaitForSeconds(m_WindupTime);

        // 2. 突進 → 突進した瞬間にフェードアウト
        Coroutine fadeOut = StartCoroutine(FadeOutAfter(m_FadeDelay));
        if (m_Shake != null) m_Shake.StartShake(0f);
        while (!DashStep()) yield return null;
        yield return fadeOut;
        if (m_Shake != null) m_Shake.StopShake();
        if (m_HitSE != null) m_HitSE.Play();

        // 3. 暗転中: ボスを元の位置に戻してコインを持たせ、プレイヤーを少し下げる
        m_Boss.position = bossStart;
        FreezeBoss(false);
        if (m_BossAnimator != null) m_BossAnimator.SetTrigger(m_IdleTrigger);
        if (m_EnemyCoin != null) m_EnemyCoin.SetActive(true);
        KnockBackPlayer(bossStart);
        yield return new WaitForSeconds(m_BlackTime);

        // 4. フェードインと同時にプレイヤーの被ダメ演出
        if (m_PlayerAnimator != null) m_PlayerAnimator.SetTrigger(m_DamageTrigger);
        Coroutine blink = StartCoroutine(Blink());
        yield return Fade(1f, 0f, m_FadeInTime);
        RestorePanel();
        yield return blink;

        IsPlaying = false;
    }

    /// <summary>
    /// プレイヤーへ X 方向に 1 フレーム分突進する。止まる距離に着いたら true
    /// </summary>
    bool DashStep()
    {
        if (m_Player == null) return true;
        float dx = m_Player.position.x - m_Boss.position.x;
        float remain = Mathf.Abs(dx) - m_DashStopDistance;
        if (remain <= 0f) return true;

        float step = Mathf.Min(m_DashSpeed * Time.deltaTime, remain);
        m_Boss.position += new Vector3(Mathf.Sign(dx) * step, 0f, 0f);
        return false;
    }

    /// <summary>
    /// ボスの物理（重力）とプレイヤーとの当たり判定を止める / 元に戻す
    /// </summary>
    void FreezeBoss(bool freeze)
    {
        if (freeze)
        {
            m_BossBodies = m_Boss.GetComponentsInChildren<Rigidbody2D>();
            m_BossBodyTypes = new RigidbodyType2D[m_BossBodies.Length];
            m_BossBodyLocalPos = new Vector3[m_BossBodies.Length];
            for (int i = 0; i < m_BossBodies.Length; i++)
            {
                m_BossBodyTypes[i] = m_BossBodies[i].bodyType;
                m_BossBodyLocalPos[i] = m_BossBodies[i].transform.localPosition;
                m_BossBodies[i].velocity = Vector2.zero;
                m_BossBodies[i].bodyType = RigidbodyType2D.Kinematic;
            }
        }
        else if (m_BossBodies != null)
        {
            for (int i = 0; i < m_BossBodies.Length; i++)
            {
                if (m_BossBodies[i] == null) continue;
                m_BossBodies[i].transform.localPosition = m_BossBodyLocalPos[i];
                m_BossBodies[i].bodyType = m_BossBodyTypes[i];
                m_BossBodies[i].velocity = Vector2.zero;
            }
        }

        // ボスとプレイヤーの当たり判定
        if (m_Player == null) return;
        var playerCols = m_Player.GetComponentsInChildren<Collider2D>();
        foreach (var bossCol in m_Boss.GetComponentsInChildren<Collider2D>())
            foreach (var playerCol in playerCols)
                Physics2D.IgnoreCollision(bossCol, playerCol, freeze);
    }

    void KnockBackPlayer(Vector3 bossPos)
    {
        if (m_Player == null || m_KnockBack == 0f) return;
        float dir = Mathf.Sign(m_Player.position.x - bossPos.x);
        Vector3 pos = m_Player.position + new Vector3(dir * m_KnockBack, 0f, 0f);

        var rb = m_Player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.position = pos;
        }
        m_Player.position = pos;
    }

    IEnumerator FadeOutAfter(float delay)
    {
        if (delay > 0f) yield return new WaitForSeconds(delay);
        yield return Fade(0f, 1f, m_FadeOutTime);
    }

    /// <summary>
    /// 暗転パネルの透明度を変える（パネルのアニメーターは止めて直接動かす）
    /// </summary>
    IEnumerator Fade(float from, float to, float time)
    {
        if (m_FadeImage == null) yield break;

        PreparePanel();

        float t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            SetAlpha(Mathf.Lerp(from, to, t / time));
            yield return null;
        }
        SetAlpha(to);
    }

    /// <summary>
    /// 暗転パネルを演出用にする
    /// アニメーターを止め、角の丸い UI 画像を外してただの塗りつぶしにし、画面より一回り広げる
    /// </summary>
    void PreparePanel()
    {
        if (m_PanelPrepared) return;
        m_PanelPrepared = true;

        GameObject panel = m_FadeImage.gameObject;
        var animator = panel.GetComponent<Animator>();
        if (animator != null) animator.enabled = false;
        m_FadeImage.sprite = null;
        m_FadeImage.rectTransform.offsetMin = new Vector2(-k_FadeOverscan, -k_FadeOverscan);
        m_FadeImage.rectTransform.offsetMax = new Vector2(k_FadeOverscan, k_FadeOverscan);
        panel.SetActive(true);
    }

    /// <summary>
    /// 暗転パネルを演出前の状態（画像・大きさ・色・表示・アニメーター）に戻す
    /// </summary>
    void RestorePanel()
    {
        if (m_FadeImage == null) return;
        GameObject panel = m_FadeImage.gameObject;
        panel.SetActive(m_PanelWasActive);
        m_FadeImage.sprite = m_PanelSprite;
        m_FadeImage.rectTransform.offsetMin = m_PanelOffsetMin;
        m_FadeImage.rectTransform.offsetMax = m_PanelOffsetMax;
        m_FadeImage.color = m_PanelColor;
        var animator = panel.GetComponent<Animator>();
        if (animator != null) animator.enabled = true;
        m_PanelPrepared = false;
    }

    void SetAlpha(float a)
    {
        Color c = m_FadeColor;
        c.a = a;
        m_FadeImage.color = c;
    }

    /// <summary>
    /// 被ダメの点滅
    /// </summary>
    IEnumerator Blink()
    {
        if (m_PlayerSprite == null) yield break;

        float t = 0f;
        while (t < m_BlinkTime)
        {
            m_PlayerSprite.enabled = !m_PlayerSprite.enabled;
            yield return new WaitForSeconds(0.08f);
            t += 0.08f;
        }
        m_PlayerSprite.enabled = true;
    }
}
