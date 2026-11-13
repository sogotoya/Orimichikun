//プレイヤーが死んだときの演出
//カメラを少し揺らす → やられたSE → BGMを止める → 少し待ってゲームオーバー表示をポンッと出す（SE）と同時にゲームオーバーBGM
//コンティニューしたら元に戻す（ゲームオーバーBGMを止めて、止めたBGMを最初から流す）
//PlayScript が自動で付けて使う（シーンに置く必要はない）
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverEffect : MonoBehaviour
{
    [Header("ゲームオーバー表示を出すまでの秒数")]
    [SerializeField] float m_ShowDelay = 1.2f;
    [Header("カメラの揺れ")]
    [SerializeField] float m_ShakeTime = 0.35f;
    [SerializeField] float m_ShakeStrength = 0.12f;
    [Header("表示がポンッと出る時間")]
    [SerializeField] float m_PopTime = 0.35f;
    [Header("音量")]
    [SerializeField, Range(0f, 1f)] float m_DieSEVolume = 0.9f;
    [SerializeField, Range(0f, 1f)] float m_ShowSEVolume = 0.8f;
    [SerializeField, Range(0f, 1f)] float m_BGMVolume = 0.6f;
    [Header("この長さ（秒）より長い音を BGM とみなして止める（ループしていなくても止める）")]
    [SerializeField] float m_BGMMinLength = 8f;

    //音（Resources/GameOver の中）
    const string k_DieSEPath = "GameOver/SE_PlayerDie";
    const string k_ShowSEPath = "GameOver/SE_GameOverShow";
    const string k_BGMPath = "GameOver/BGM_GameOver";

    AudioSource m_SESource;
    AudioSource m_BGMSource;
    AudioClip m_DieSE;
    AudioClip m_ShowSE;

    //止めたBGM（コンティニューで流し直す）
    readonly List<AudioSource> m_StoppedBGMs = new List<AudioSource>();
    Coroutine m_Routine;
    GameObject m_UI;
    Vector3 m_UIScale = Vector3.one;
    bool m_IsPlaying;

    /// <summary>
    /// ゲームオーバー表示が出て、コンティニューできる状態か
    /// </summary>
    public bool IsReady { get; private set; }

    private void Awake()
    {
        //SE と BGM 用の音を鳴らす部品（距離で小さくならないように2D）
        m_SESource = gameObject.AddComponent<AudioSource>();
        m_SESource.playOnAwake = false;
        m_SESource.spatialBlend = 0f;
        m_BGMSource = gameObject.AddComponent<AudioSource>();
        m_BGMSource.playOnAwake = false;
        m_BGMSource.spatialBlend = 0f;
        m_BGMSource.loop = true;

        //音を読み込む
        m_DieSE = Resources.Load<AudioClip>(k_DieSEPath);
        m_ShowSE = Resources.Load<AudioClip>(k_ShowSEPath);
        m_BGMSource.clip = Resources.Load<AudioClip>(k_BGMPath);
        if (m_DieSE == null || m_ShowSE == null || m_BGMSource.clip == null)
        {
            Debug.LogWarning("ゲームオーバー用の音が Resources/GameOver に見つかりません", this);
        }
    }

    /// <summary>
    /// 死んだときの演出を始める処理（1回だけ呼ぶ）
    /// </summary>
    public void Play(GameObject gameOverUI)
    {
        if (m_IsPlaying) return;
        m_IsPlaying = true;
        IsReady = false;

        //カメラを少し揺らす
        PlayerCamera playerCamera = GetComponentInChildren<PlayerCamera>();
        if (playerCamera == null) playerCamera = FindObjectOfType<PlayerCamera>();
        if (playerCamera != null) playerCamera.Shake(m_ShakeTime, m_ShakeStrength);

        //やられたSE
        if (m_DieSE != null) m_SESource.PlayOneShot(m_DieSE, m_DieSEVolume);

        //BGMを止める
        StopSceneBGM();

        //ゲームオーバー表示は少し待ってから出すので、いったん隠す
        m_UI = gameOverUI;
        if (m_UI != null)
        {
            m_UIScale = m_UI.transform.localScale;
            m_UI.SetActive(false);
        }

        m_Routine = StartCoroutine(ShowRoutine());
    }

    /// <summary>
    /// コンティニューしたときに元に戻す処理
    /// restartBGM が true なら、止めたBGMを最初から流し直す
    /// </summary>
    public void Stop(bool restartBGM)
    {
        if (m_Routine != null) StopCoroutine(m_Routine);
        m_Routine = null;
        m_BGMSource.Stop();

        //表示の大きさを戻す
        if (m_UI != null) m_UI.transform.localScale = m_UIScale;

        //止めたBGMを流し直す
        if (restartBGM)
        {
            foreach (AudioSource src in m_StoppedBGMs)
            {
                if (src != null) src.Play();
            }
        }
        m_StoppedBGMs.Clear();

        m_IsPlaying = false;
        IsReady = false;
    }

    /// <summary>
    /// 流れているBGM（長い音）を探して止める処理（ボス戦のBGMはループしない設定なので、ループは見ない）
    /// </summary>
    void StopSceneBGM()
    {
        m_StoppedBGMs.Clear();
        foreach (AudioSource src in FindObjectsOfType<AudioSource>())
        {
            if (src == m_BGMSource || src == m_SESource) continue;
            if (!src.isPlaying || src.clip == null) continue;
            if (src.clip.length < m_BGMMinLength) continue;
            src.Stop();
            m_StoppedBGMs.Add(src);
        }
    }

    /// <summary>
    /// 少し待ってから、ゲームオーバー表示をポンッと出してBGMを流す処理
    /// </summary>
    IEnumerator ShowRoutine()
    {
        yield return new WaitForSecondsRealtime(m_ShowDelay);

        //表示SEとゲームオーバーBGMを同時に
        if (m_ShowSE != null) m_SESource.PlayOneShot(m_ShowSE, m_ShowSEVolume);
        if (m_BGMSource.clip != null)
        {
            m_BGMSource.volume = m_BGMVolume;
            m_BGMSource.Play();
        }

        if (m_UI != null)
        {
            //小さい所から少し大きくなって、元の大きさに戻る
            m_UI.SetActive(true);
            float t = 0f;
            while (t < m_PopTime)
            {
                t += Time.unscaledDeltaTime;
                float rate = Mathf.Clamp01(t / m_PopTime);
                float scale = rate < 0.7f
                    ? Mathf.Lerp(0f, 1.12f, rate / 0.7f)
                    : Mathf.Lerp(1.12f, 1f, (rate - 0.7f) / 0.3f);
                m_UI.transform.localScale = m_UIScale * scale;
                yield return null;
            }
            m_UI.transform.localScale = m_UIScale;
        }

        IsReady = true;
        m_Routine = null;
    }
}
