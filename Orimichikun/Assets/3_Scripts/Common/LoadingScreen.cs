//ロード画面（プレハブ: 4_Prefabs/UI/LoadingScreen_Boss）
//フェードインで表示 → 裏でシーンを読み込み、読み込みが終わってから指定秒数待つ
//→ 緑のキャラクターから光へ波打ちながらズームインして白く光る → 自動でシーンを切り替える（クリック不要）→ フェードアウト
//使い方: LoadingScreen.Load(ロード画面プレハブ, "シーン名");
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField, Tooltip("読み込みが終わってから、さらに表示し続ける秒数")]
    float m_ExtraTime = 2f;

    [SerializeField, Tooltip("ロード画面の表示・消去をフェードさせる CanvasGroup")]
    CanvasGroup m_CanvasGroup;

    [SerializeField, Tooltip("ロード画面を出すフェードの秒数（0 でパッと出す）")]
    float m_FadeInTime = 0.5f;

    [SerializeField, Tooltip("ロード画面を消すフェードの秒数（0 ですぐ消す）")]
    float m_FadeOutTime = 0.3f;

    [Header("切り替え前のズームイン（任意）")]
    [SerializeField, Tooltip("ズームさせる絵（未設定ならズームしない）")]
    RectTransform m_ZoomImage;

    [SerializeField, Tooltip("ズームの始点＝緑のキャラクター（絵の左下 = (0,0)、右上 = (1,1)）")]
    Vector2 m_ZoomStartPoint = new Vector2(0.31f, 0.37f);

    [SerializeField, Tooltip("ズームの終点＝光の中心（絵の左下 = (0,0)、右上 = (1,1)）")]
    Vector2 m_ZoomPoint = new Vector2(0.845f, 0.78f);

    [SerializeField, Tooltip("始点から終点へ進むときの横揺れの大きさ（絵の大きさに対する割合）")]
    float m_WaveAmplitude = 0.06f;

    [SerializeField, Tooltip("横揺れの回数（半波の数。2 で S 字）")]
    int m_WaveCount = 2;

    [SerializeField, Tooltip("最終的な拡大率")]
    float m_ZoomScale = 6f;

    [SerializeField, Tooltip("ズームにかける秒数")]
    float m_ZoomTime = 2.0f;

    [SerializeField, Tooltip("画面を白く光らせる Image（未設定なら光らない）")]
    UnityEngine.UI.Image m_Flash;

    [SerializeField, Tooltip("ズームのどこから白くなり始めるか（0 = 最初から, 1 = 最後）")]
    [Range(0f, 1f)]
    float m_FlashStart = 0.65f;

    //二重にロードしないためのフラグ
    static bool s_IsLoading;
    public static bool IsLoading => s_IsLoading;

    //エディタで「ドメインのリロード」を切っていてもフラグが残らないように
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetStatic()
    {
        s_IsLoading = false;
    }

    /// <summary>
    /// ロード画面を出してシーンを読み込む。プレハブ未設定ならそのまま切り替える
    /// </summary>
    public static void Load(LoadingScreen prefab, string sceneName)
    {
        if (s_IsLoading) return;

        if (prefab == null)
        {
            SceneManager.LoadScene(sceneName);
            return;
        }

        LoadingScreen screen = Instantiate(prefab);
        DontDestroyOnLoad(screen.gameObject);
        //フェードインするので最初は透明にしておく（1 フレームだけパッと見えるのを防ぐ）
        if (screen.m_CanvasGroup != null && screen.m_FadeInTime > 0f) screen.m_CanvasGroup.alpha = 0f;
        screen.StartCoroutine(screen.LoadRoutine(sceneName));
    }

    IEnumerator LoadRoutine(string sceneName)
    {
        s_IsLoading = true;

        // ロード画面をフェードインで表示（表示しきってから読み込みを始めて、フェードをなめらかにする）
        yield return FadeCanvas(0f, 1f, m_FadeInTime);

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        // 読み込み完了（allowSceneActivation = false のときは 0.9 で止まる）
        while (op.progress < 0.9f) yield return null;

        // 読み込みが終わってからも指定秒数は表示する
        yield return new WaitForSecondsRealtime(m_ExtraTime);

        // 光っている所へズームインして白く光る
        yield return ZoomIn();

        // シーンを切り替える
        op.allowSceneActivation = true;
        while (!op.isDone) yield return null;

        // ロード画面をフェードアウトで消す
        yield return FadeCanvas(1f, 0f, m_FadeOutTime);

        s_IsLoading = false;
        Destroy(gameObject);
    }

    /// <summary>
    /// 緑のキャラクター（m_ZoomStartPoint）から光（m_ZoomPoint）へ、波打ちながら進みつつズームインする。後半で白く光る
    /// </summary>
    IEnumerator ZoomIn()
    {
        if (m_ZoomImage == null || m_ZoomTime <= 0f) yield break;

        // 画面に合わせて大きさを決めるコンポーネントが位置を戻さないよう止める
        var fitter = m_ZoomImage.GetComponent<UnityEngine.UI.AspectRatioFitter>();
        if (fitter != null) fitter.enabled = false;

        Vector2 startPos = m_ZoomImage.anchoredPosition;

        float t = 0f;
        while (t < m_ZoomTime)
        {
            t += Time.unscaledDeltaTime;
            ApplyZoom(Mathf.Clamp01(t / m_ZoomTime), startPos);
            yield return null;
        }
        ApplyZoom(1f, startPos);
    }

    /// <summary>
    /// ズーム演出の rate（0〜1）時点の状態を絵と白い光に当てはめる
    /// </summary>
    public void ApplyZoom(float rate, Vector2 startPos)
    {
        Rect rect = m_ZoomImage.rect;

        // 拡大率: だんだん加速して吸い込まれるように
        float scale = Mathf.Lerp(1f, m_ZoomScale, rate * rate);
        m_ZoomImage.localScale = new Vector3(scale, scale, 1f);

        // 注目する点: キャラクター → 光 へ、進む方向と直角に波打ちながら移動
        float u = Mathf.SmoothStep(0f, 1f, rate);
        Vector2 dir = m_ZoomPoint - m_ZoomStartPoint;
        Vector2 side = new Vector2(-dir.y, dir.x).normalized;
        Vector2 focus = Vector2.Lerp(m_ZoomStartPoint, m_ZoomPoint, u)
                      + side * Mathf.Sin(u * Mathf.PI * m_WaveCount) * m_WaveAmplitude;
        Vector2 local = new Vector2((focus.x - 0.5f) * rect.width, (focus.y - 0.5f) * rect.height);

        // 注目する点を画面中央へ寄せる（最初の 25% で寄せ始める）
        float follow = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(rate / 0.25f));
        Vector2 offset = -local * scale * follow;

        // 絵の外（黒い部分）が見えないよう、絵が画面全体を覆う範囲に収める
        var parent = m_ZoomImage.parent as RectTransform;
        if (parent != null)
        {
            Vector2 screen = parent.rect.size;
            float maxX = Mathf.Max(0f, (rect.width * scale - screen.x) * 0.5f);
            float maxY = Mathf.Max(0f, (rect.height * scale - screen.y) * 0.5f);
            offset.x = Mathf.Clamp(offset.x, -maxX, maxX);
            offset.y = Mathf.Clamp(offset.y, -maxY, maxY);
        }
        m_ZoomImage.anchoredPosition = startPos + offset;

        // 後半で白く光る
        if (m_Flash != null)
        {
            Color c = m_Flash.color;
            c.a = Mathf.InverseLerp(m_FlashStart, 1f, rate);
            m_Flash.color = c;
        }
    }

    /// <summary>
    /// ロード画面全体の透明度を変える（時間が止まっていても動くよう unscaled）
    /// </summary>
    IEnumerator FadeCanvas(float from, float to, float time)
    {
        if (m_CanvasGroup == null) yield break;
        if (time <= 0f)
        {
            m_CanvasGroup.alpha = to;
            yield break;
        }

        float t = 0f;
        while (t < time)
        {
            t += Time.unscaledDeltaTime;
            m_CanvasGroup.alpha = Mathf.Lerp(from, to, t / time);
            yield return null;
        }
        m_CanvasGroup.alpha = to;
    }

    private void OnDestroy()
    {
        s_IsLoading = false;
    }
}
