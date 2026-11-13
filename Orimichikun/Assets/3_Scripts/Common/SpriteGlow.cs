//スプライトのふちを光らせる処理
//同じ絵を色だけにして、まわりに少しずらして並べることで光るふちを作る
//SetGlow(true) で光り始め、SetGlow(false) で消える（ふわっと切り替わる）
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteGlow : MonoBehaviour
{
    [SerializeField, Header("光の色")]
    Color m_GlowColor = new Color(0.85f, 1f, 0.35f, 1f);

    [SerializeField, Header("光のふちの太さ（ドット数）")]
    [Range(1, 4)]
    int m_Thickness = 2;

    [SerializeField, Header("明滅の速さ")]
    float m_PulseSpeed = 3f;

    [SerializeField, Header("明滅の暗いときの明るさ（0〜1）")]
    [Range(0f, 1f)]
    float m_PulseMin = 0.45f;

    [SerializeField, Header("光り始め・消えるまでの秒数")]
    float m_FadeTime = 0.25f;

    //色だけで絵を描くマテリアル（Resources/Effect の中）
    const string k_MaterialPath = "Effect/SpriteSilhouette";

    SpriteRenderer m_Source;
    SpriteRenderer[] m_Outlines;
    bool m_IsGlow;
    //0 = 消えている, 1 = 光っている
    float m_Fade;

    private void Awake()
    {
        m_Source = GetComponent<SpriteRenderer>();
        CreateOutlines();
        SetAlpha(0f);
    }

    /// <summary>
    /// 光らせるかどうかを切り替える処理
    /// </summary>
    public void SetGlow(bool glow)
    {
        m_IsGlow = glow;
    }

    private void LateUpdate()
    {
        if (m_Outlines == null) return;

        //ふわっと切り替える
        float target = m_IsGlow ? 1f : 0f;
        float speed = m_FadeTime > 0f ? Time.deltaTime / m_FadeTime : 1f;
        m_Fade = Mathf.MoveTowards(m_Fade, target, speed);

        //消えているときは何もしない
        if (m_Fade <= 0f && !m_IsGlow)
        {
            SetAlpha(0f);
            return;
        }

        //明滅
        float pulse = Mathf.Lerp(m_PulseMin, 1f, (Mathf.Sin(Time.time * m_PulseSpeed) + 1f) * 0.5f);
        SetAlpha(m_Fade * pulse);

        //本体の絵や向きが変わったら合わせる
        foreach (SpriteRenderer sr in m_Outlines)
        {
            sr.sprite = m_Source.sprite;
            sr.flipX = m_Source.flipX;
            sr.flipY = m_Source.flipY;
        }
    }

    /// <summary>
    /// 光るふち用の絵を、本体のまわりに作る処理
    /// </summary>
    void CreateOutlines()
    {
        Material mat = Resources.Load<Material>(k_MaterialPath);
        if (mat == null)
        {
            Debug.LogWarning($"光らせるためのマテリアルが見つかりません: Resources/{k_MaterialPath}", this);
            return;
        }

        //1ドットの大きさ
        float pixel = m_Source.sprite != null ? 1f / m_Source.sprite.pixelsPerUnit : 0.01f;

        //上下左右と斜めにずらして並べる
        Vector2[] dirs =
        {
            new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1),
            new Vector2(1, 1), new Vector2(-1, 1), new Vector2(1, -1), new Vector2(-1, -1),
        };

        m_Outlines = new SpriteRenderer[dirs.Length];
        for (int i = 0; i < dirs.Length; i++)
        {
            GameObject obj = new GameObject("Glow");
            obj.transform.SetParent(transform, false);
            obj.transform.localPosition = (Vector3)(dirs[i] * pixel * m_Thickness);

            //本体のすぐ後ろに描く
            SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
            sr.sprite = m_Source.sprite;
            sr.sharedMaterial = mat;
            sr.sortingLayerID = m_Source.sortingLayerID;
            sr.sortingOrder = m_Source.sortingOrder - 1;
            m_Outlines[i] = sr;
        }
    }

    /// <summary>
    /// 光の明るさをセット
    /// </summary>
    void SetAlpha(float alpha)
    {
        if (m_Outlines == null) return;
        Color c = m_GlowColor;
        c.a *= alpha;
        foreach (SpriteRenderer sr in m_Outlines)
        {
            sr.color = c;
            sr.enabled = alpha > 0f;
        }
    }
}
