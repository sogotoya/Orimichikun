//中間地点の処理
//プレイヤーが触ると宝石が光り、効果音を鳴らして「中間地点！」の文字を出し、リスポーン地点をここにする
//一度取ると、Stage のどこで死んでも、ボス戦で死んでも、ここから再開する（タイトルに戻るとリセット）
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class MidPoint : MonoBehaviour
{
    [Header("触る前の絵（宝石が暗い）")]
    public Sprite m_OffSprite;
    [Header("触った後の絵（宝石が光る）")]
    public Sprite m_OnSprite;
    [Header("触ったときのSE")]
    public AudioClip m_SE;
    [Header("SEの音量")]
    [Range(0f, 1f)]
    public float m_SEVolume = 0.8f;
    [Header("リスポーンする位置（未設定なら石の少し上）")]
    public Transform m_RespawnPoint;
    [Header("光の色")]
    public Color m_GlowColor = new Color(1f, 0.55f, 0.9f, 1f);
    [Header("ポップする文字")]
    public string m_PopupText = "中間地点！";
    [Header("文字の色")]
    public Color m_PopupColor = new Color(1f, 0.67f, 0.92f, 1f);

    //タイトルに戻るまで覚えておく中間地点
    static bool s_HasSaved;
    static Vector3 s_SavedPosition;
    static string s_SavedId;

    /// <summary>
    /// 中間地点を取っているか
    /// </summary>
    public static bool HasSavedPosition => s_HasSaved;

    /// <summary>
    /// 取った中間地点のリスポーン位置
    /// </summary>
    public static Vector3 SavedPosition => s_SavedPosition;

    SpriteRenderer m_SR;
    SpriteGlow m_Glow;
    AudioSource m_Audio;
    bool m_IsActivated;

    /// <summary>
    /// タイトルに戻ったら中間地点を忘れるようにする処理
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Init()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Title") ResetSave();
    }

    /// <summary>
    /// 中間地点の記録を消す処理（最初からやり直すとき用）
    /// </summary>
    public static void ResetSave()
    {
        s_HasSaved = false;
        s_SavedId = null;
    }

    private void Awake()
    {
        m_SR = GetComponent<SpriteRenderer>();
        if (m_OffSprite != null) m_SR.sprite = m_OffSprite;

        //光らせる処理を付けて色をセット
        m_Glow = GetComponent<SpriteGlow>();
        if (m_Glow == null) m_Glow = gameObject.AddComponent<SpriteGlow>();
        m_Glow.GlowColor = m_GlowColor;

        //SEは距離で小さくならないように2Dで鳴らす
        m_Audio = gameObject.AddComponent<AudioSource>();
        m_Audio.playOnAwake = false;
        m_Audio.spatialBlend = 0f;
    }

    private void Start()
    {
        //前に取っていた中間地点なら、最初から光らせておく（ボス戦から戻ったときなど）
        if (s_HasSaved && s_SavedId == GetId()) Activate(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_IsActivated || !collision.CompareTag("Player")) return;

        PlayScript player = collision.GetComponentInParent<PlayScript>();
        if (player == null) return;

        //リスポーン位置をここにする
        Vector3 pos = GetRespawnPosition();
        pos.z = player.transform.position.z;
        player.UpdateSavePoint(pos);

        //シーンが変わっても覚えておく
        s_HasSaved = true;
        s_SavedPosition = pos;
        s_SavedId = GetId();

        Activate(true);
    }

    /// <summary>
    /// 宝石を光らせる処理（演出ありなら SE と文字も出す）
    /// </summary>
    void Activate(bool withEffect)
    {
        m_IsActivated = true;
        if (m_OnSprite != null) m_SR.sprite = m_OnSprite;
        m_Glow.SetGlow(true);

        if (!withEffect) return;
        if (m_SE != null) m_Audio.PlayOneShot(m_SE, m_SEVolume);
        StartCoroutine(Popup());
    }

    /// <summary>
    /// リスポーンする位置を返す処理
    /// </summary>
    Vector3 GetRespawnPosition()
    {
        if (m_RespawnPoint != null) return m_RespawnPoint.position;
        return transform.position + Vector3.up * 0.5f;
    }

    /// <summary>
    /// 中間地点を見分けるための名前（シーン名と位置）
    /// </summary>
    string GetId()
    {
        Vector3 p = transform.position;
        return $"{gameObject.scene.name}:{Mathf.RoundToInt(p.x * 10)}:{Mathf.RoundToInt(p.y * 10)}";
    }

    /// <summary>
    /// 石の上に文字をふわっと出して消す処理
    /// </summary>
    IEnumerator Popup()
    {
        //文字を出すためのキャンバスを石の上に作る
        GameObject canvasObj = new GameObject("MidPointPopup");
        canvasObj.transform.SetParent(transform, false);
        float top = m_SR.sprite != null ? m_SR.sprite.bounds.max.y : 0.8f;
        Vector3 start = new Vector3(0f, top + 0.15f, 0f);
        canvasObj.transform.localPosition = start;
        canvasObj.transform.localScale = Vector3.one * 0.01f;

        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.sortingLayerID = m_SR.sortingLayerID;
        canvas.sortingOrder = m_SR.sortingOrder + 10;
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.dynamicPixelsPerUnit = 4f;
        canvasObj.GetComponent<RectTransform>().sizeDelta = new Vector2(400f, 80f);

        //ドット文字に黒ふち
        GameObject textObj = new GameObject("Text", typeof(RectTransform));
        textObj.transform.SetParent(canvasObj.transform, false);
        RectTransform rt = textObj.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = rt.offsetMax = Vector2.zero;
        Text text = textObj.AddComponent<Text>();
        text.font = FindGameFont();
        text.fontSize = 26;
        text.alignment = TextAnchor.MiddleCenter;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.text = m_PopupText;
        text.color = m_PopupColor;
        Outline outline = textObj.AddComponent<Outline>();
        outline.effectColor = new Color(0.12f, 0.08f, 0.12f, 1f);
        outline.effectDistance = new Vector2(3f, -3f);

        //上へふわっと動かして、最後に消す
        const float duration = 1.4f;
        const float fadeStart = 0.9f;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float rate = t / duration;
            canvasObj.transform.localPosition = start + Vector3.up * (0.35f * (1f - (1f - rate) * (1f - rate)));
            float alpha = t < fadeStart ? 1f : 1f - (t - fadeStart) / (duration - fadeStart);
            Color c = m_PopupColor;
            c.a = alpha;
            text.color = c;
            Color oc = outline.effectColor;
            oc.a = alpha;
            outline.effectColor = oc;
            yield return null;
        }
        Destroy(canvasObj);
    }

    /// <summary>
    /// ゲームで使っているドット文字のフォントを探す処理（無ければ標準のフォント）
    /// </summary>
    Font FindGameFont()
    {
        foreach (Font f in Resources.FindObjectsOfTypeAll<Font>())
        {
            if (f != null && f.name.Contains("DotGothic")) return f;
        }
        return Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
    }
}
