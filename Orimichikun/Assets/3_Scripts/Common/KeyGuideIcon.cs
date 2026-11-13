//キー表示の画像を、コントローラー接続中はコントローラー用、そうでないときは PC 用に切り替える処理
//SpriteRenderer / Image / RawImage のどれが付いていても使える
//画像は Resources フォルダから読み込む（PC 用を空にすると、もとから付いている画像を PC 用として使う）
using UnityEngine;
using UnityEngine.UI;

public class KeyGuideIcon : MonoBehaviour
{
    [SerializeField, Tooltip("PC 用の画像（Resources からのパス）。空ならもとの画像を使う")]
    string m_PcImagePath;

    [SerializeField, Tooltip("コントローラー用の画像（Resources からのパス）")]
    string m_PadImagePath;

    SpriteRenderer m_SpriteRenderer;
    Image m_Image;
    RawImage m_RawImage;

    //PC 用・コントローラー用の画像
    Sprite m_PcSprite;
    Sprite m_PadSprite;
    Texture m_PcTexture;
    Texture m_PadTexture;

    bool m_IsReady;
    //今コントローラー用を表示しているか（null = まだ表示していない）
    bool? m_ShowingPad;

    /// <summary>
    /// 表示オブジェクトにこの処理を付ける用の関数
    /// </summary>
    public static KeyGuideIcon Attach(GameObject target, string pcImagePath, string padImagePath)
    {
        if (target == null) return null;

        //すでに付いていればそのまま使う（2回目の設定で画像を取り違えないように）
        KeyGuideIcon icon = target.GetComponent<KeyGuideIcon>();
        if (icon != null) return icon;

        icon = target.AddComponent<KeyGuideIcon>();
        icon.m_PcImagePath = pcImagePath;
        icon.m_PadImagePath = padImagePath;
        icon.Apply();
        return icon;
    }

    private void OnEnable()
    {
        //表示された瞬間に正しい画像にする
        Apply();
    }

    private void Update()
    {
        Apply();
    }

    /// <summary>
    /// 接続状態に合わせて画像を切り替える処理
    /// </summary>
    void Apply()
    {
        Setup();
        if (!m_IsReady) return;

        bool pad = ControllerCheck.IsConnected;
        //状態が変わったときだけ入れ替える
        if (m_ShowingPad == pad) return;
        m_ShowingPad = pad;

        if (m_SpriteRenderer != null) m_SpriteRenderer.sprite = pad ? m_PadSprite : m_PcSprite;
        if (m_Image != null) m_Image.sprite = pad ? m_PadSprite : m_PcSprite;
        if (m_RawImage != null) m_RawImage.texture = pad ? m_PadTexture : m_PcTexture;
    }

    /// <summary>
    /// 表示の部品と画像を用意する処理（最初の1回だけ）
    /// </summary>
    void Setup()
    {
        //AddComponent した直後はまだパスが入っていないので、入るまで待つ
        if (m_IsReady || string.IsNullOrEmpty(m_PadImagePath)) return;
        m_IsReady = true;

        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Image = GetComponent<Image>();
        m_RawImage = GetComponent<RawImage>();

        //もとの画像を PC 用として取っておく
        if (m_SpriteRenderer != null) m_PcSprite = m_SpriteRenderer.sprite;
        if (m_Image != null) m_PcSprite = m_Image.sprite;
        if (m_RawImage != null) m_PcTexture = m_RawImage.texture;

        //PC 用の画像が指定されていれば読み込む
        if (!string.IsNullOrEmpty(m_PcImagePath))
        {
            Sprite pcSprite = Resources.Load<Sprite>(m_PcImagePath);
            Texture2D pcTexture = Resources.Load<Texture2D>(m_PcImagePath);
            if (pcSprite != null) m_PcSprite = pcSprite;
            if (pcTexture != null) m_PcTexture = pcTexture;
            if (pcSprite == null && pcTexture == null) Debug.LogWarning($"PC 用のキー画像が見つかりません: Resources/{m_PcImagePath}", this);
        }

        //コントローラー用の画像を読み込む（見つからなければ PC 用のまま）
        m_PadSprite = Resources.Load<Sprite>(m_PadImagePath);
        m_PadTexture = Resources.Load<Texture2D>(m_PadImagePath);
        if (m_PadSprite == null && m_PadTexture == null)
        {
            Debug.LogWarning($"コントローラー用のキー画像が見つかりません: Resources/{m_PadImagePath}", this);
        }
        if (m_PadSprite == null) m_PadSprite = m_PcSprite;
        if (m_PadTexture == null) m_PadTexture = m_PcTexture;
    }
}
