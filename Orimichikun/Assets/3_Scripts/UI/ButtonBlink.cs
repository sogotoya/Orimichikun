//操作説明用: コントローラーの絵で、指定のボタンを白く光らせるのを繰り返す
//ベース画像（何も光っていない絵）と光った画像を交互に表示する
//1 セットで何回光らせるかを決められる（例: 2 段ジャンプは「短く 2 回光る → 1 拍休む」）
//光らせる画像を複数入れると、セットごとに順番に切り替わる（例: LB と RB）
//プレハブ: 4_Prefabs/UI/ButtonGuide/ButtonGuide_〇〇
using UnityEngine;
using UnityEngine.UI;

public class ButtonBlink : MonoBehaviour
{
    [SerializeField, Tooltip("何も光っていない絵")]
    Sprite m_Base;

    [SerializeField, Tooltip("ボタンが光っている絵（複数ならセットごとに順番に光る）")]
    Sprite[] m_Lit;

    [SerializeField, Tooltip("1 回に光っている時間（秒）")]
    float m_LitTime = 0.5f;

    [SerializeField, Tooltip("1 セットの中で、光と光の間に消えている時間（秒）")]
    float m_BaseTime = 0.5f;

    [SerializeField, Tooltip("1 セットで光らせる回数（2 段ジャンプなら 2）")]
    [Min(1)]
    int m_FlashCount = 1;

    [SerializeField, Tooltip("1 セット光り終わった後に休む時間（秒）")]
    float m_PauseTime = 0.5f;

    [SerializeField, Tooltip("一時停止中（Time.timeScale = 0）でも点滅させる")]
    bool m_UnscaledTime = true;

    //UI なら Image、ワールドに置くなら SpriteRenderer に表示する
    Image m_Image;
    SpriteRenderer m_Renderer;

    float m_Timer;
    bool m_IsLit;
    int m_FlashIndex;   //セットの中で何回目の光か
    int m_LitIndex;     //どの光った絵を使うか

    private void Awake()
    {
        m_Image = GetComponent<Image>();
        m_Renderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        //表示されるたびにベースから始める
        m_Timer = 0f;
        m_IsLit = false;
        m_FlashIndex = 0;
        m_LitIndex = 0;
        SetSprite(m_Base);
    }

    private void Update()
    {
        Step(m_UnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);
    }

    /// <summary>
    /// 時間を進めて表示を切り替える。いま光っているかを返す
    /// </summary>
    public bool Step(float deltaTime)
    {
        if (m_Lit == null || m_Lit.Length == 0) return false;

        m_Timer += deltaTime;
        while (m_Timer >= CurrentLimit())
        {
            m_Timer -= CurrentLimit();

            if (m_IsLit)
            {
                //光る → 消える
                m_IsLit = false;
                m_FlashIndex++;
                if (m_FlashIndex >= m_FlashCount)
                {
                    //1 セット終わり: 次のセットは次の絵で
                    m_FlashIndex = 0;
                    m_LitIndex = (m_LitIndex + 1) % m_Lit.Length;
                }
                SetSprite(m_Base);
            }
            else
            {
                //消える → 光る
                m_IsLit = true;
                SetSprite(m_Lit[m_LitIndex]);
            }
        }
        return m_IsLit;
    }

    /// <summary>
    /// 今の表示を続ける時間
    /// 光っている間は m_LitTime、セットの途中の消灯は m_BaseTime、セットの後（最初も含む）は m_PauseTime
    /// </summary>
    float CurrentLimit()
    {
        float limit;
        if (m_IsLit) limit = m_LitTime;
        else limit = m_FlashIndex == 0 ? m_PauseTime : m_BaseTime;
        return Mathf.Max(0.01f, limit);
    }

    void SetSprite(Sprite sprite)
    {
        if (sprite == null) return;
        if (m_Image != null) m_Image.sprite = sprite;
        if (m_Renderer != null) m_Renderer.sprite = sprite;
    }
}
