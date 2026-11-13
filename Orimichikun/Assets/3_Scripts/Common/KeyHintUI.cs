//画面右下に、今できる操作とキーを表示する処理
//コントローラー接続中はコントローラーのボタン、そうでないときは PC のキーを出す
//Title と Stage と BossStage を読み込んだときに自動で作られる（シーンに置く必要はない）
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KeyHintUI : MonoBehaviour
{
    //表示するシーン
    static readonly string[] k_TargetScenes = { "Title", "Stage", "BossStage" };

    //キー画像（Resources/KeyGuide の中）
    const string k_IconFolder = "KeyGuide/";

    //見た目の設定
    const int k_FontSize = 28;
    const float k_RowSpacing = 8f;
    const float k_KeySpacing = 8f;
    //キー画像の大きさ（元の画像に対する倍率。2/3 で小さめ、1 で元の大きさ）
    const float k_IconScale = 5f / 6f;
    static readonly Vector2 k_Margin = new Vector2(30f, 30f);

    //行ごとの表示内容
    struct Hint
    {
        public string m_Label;
        public string[] m_PcIcons;
        public string[] m_PadIcons;
        public Hint(string label, string[] pcIcons, string[] padIcons)
        {
            m_Label = label;
            m_PcIcons = pcIcons;
            m_PadIcons = padIcons;
        }
    }

    static readonly Hint k_Teleport = new Hint("ボス戦へ進む", new[] { "Key_E" }, new[] { "Pad_RB" });
    static readonly Hint k_Recovery = new Hint("回復する", new[] { "Key_E" }, new[] { "Pad_LB" });
    static readonly Hint k_Continue = new Hint("コンティニュー", new[] { "Key_Enter" }, new[] { "Pad_LT" });
    //タイトルが出たときの操作
    static readonly Hint k_GameStart = new Hint("ゲームスタート", new[] { "Key_E" }, new[] { "Pad_RB" });
    //常に出す操作
    static readonly Hint k_Attack = new Hint("攻撃", new[] { "Mouse_Left" }, new[] { "Pad_Y", "Pad_B" });
    static readonly Hint k_Jump = new Hint("ジャンプ", new[] { "Key_Space" }, new[] { "Pad_X", "Pad_A" });

    RectTransform m_RowParent;
    Font m_Font;

    //状態を調べる相手
    TP[] m_Teleports;
    CoinCountManager m_CoinManager;
    PlayScript m_Player;
    TriggerManager m_TitleManager;

    //今表示している内容（変わったときだけ作り直す）
    string m_CurrentKey = null;
    readonly List<Hint> m_Hints = new List<Hint>();
    readonly Dictionary<string, Sprite> m_IconCache = new Dictionary<string, Sprite>();

    /// <summary>
    /// ゲーム開始時にシーン切り替えの監視を始める処理
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Init()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
        //最初のシーンは sceneLoaded が来ないことがあるのでここでも作る
        CreateIfNeeded(SceneManager.GetActiveScene());
    }

    static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CreateIfNeeded(scene);
    }

    /// <summary>
    /// 対象のシーンならキー表示を作る処理
    /// </summary>
    static void CreateIfNeeded(Scene scene)
    {
        if (System.Array.IndexOf(k_TargetScenes, scene.name) < 0) return;
        //スマホでは画面のボタンがあるので出さない
        if (Application.isMobilePlatform) return;
        //二重に作らない
        if (FindObjectOfType<KeyHintUI>() != null) return;

        GameObject obj = new GameObject("KeyHintUI");
        SceneManager.MoveGameObjectToScene(obj, scene);
        obj.AddComponent<KeyHintUI>();
    }

    private void Awake()
    {
        BuildCanvas();
    }

    private void Update()
    {
        FindTargets();
        CollectHints();

        //表示内容とコントローラー接続の状態をまとめて、変わったときだけ作り直す
        bool pad = ControllerCheck.IsUsingPad;
        string key = pad ? "pad" : "pc";
        foreach (Hint h in m_Hints) key += "|" + h.m_Label;
        if (key == m_CurrentKey) return;
        m_CurrentKey = key;
        ShowHints(pad);
    }

    /// <summary>
    /// 今できる操作を集める処理（上から順に並ぶ）
    /// </summary>
    void CollectHints()
    {
        m_Hints.Clear();

        //ロード中と一時停止中は出さない
        if (LoadingScreen.IsLoading || Time.timeScale == 0f) return;

        //タイトルが出たらゲームスタートだけ
        if (m_TitleManager != null && m_TitleManager.m_IsTitle)
        {
            m_Hints.Add(k_GameStart);
            return;
        }

        //死んでいるときはコンティニューだけ
        if (m_Player != null && m_Player.IsDead)
        {
            m_Hints.Add(k_Continue);
            return;
        }

        if (m_Teleports != null)
        {
            foreach (TP tp in m_Teleports)
            {
                if (tp != null && tp.IsPlayerInArea)
                {
                    m_Hints.Add(k_Teleport);
                    break;
                }
            }
        }

        if (m_CoinManager != null && m_CoinManager.CanRecovery) m_Hints.Add(k_Recovery);

        //攻撃とジャンプはいつも出す（一番下）
        m_Hints.Add(k_Attack);
        m_Hints.Add(k_Jump);
    }

    /// <summary>
    /// 状態を調べる相手を探す処理（見つかっていないものだけ）
    /// </summary>
    void FindTargets()
    {
        if (m_Teleports == null) m_Teleports = FindObjectsOfType<TP>();
        if (m_CoinManager == null) m_CoinManager = FindObjectOfType<CoinCountManager>();
        if (m_Player == null) m_Player = FindObjectOfType<PlayScript>();
        if (m_TitleManager == null && gameObject.scene.name == "Title") m_TitleManager = FindObjectOfType<TriggerManager>();
    }

    /// <summary>
    /// 行を並べ直す処理
    /// </summary>
    void ShowHints(bool pad)
    {
        //前の行を消す
        for (int i = m_RowParent.childCount - 1; i >= 0; i--)
        {
            Destroy(m_RowParent.GetChild(i).gameObject);
        }

        foreach (Hint h in m_Hints)
        {
            string[] names = pad ? h.m_PadIcons : h.m_PcIcons;
            Sprite[] icons = new Sprite[names.Length];
            for (int i = 0; i < names.Length; i++) icons[i] = LoadIcon(names[i]);
            AddRow(h.m_Label, icons);
        }
    }

    /// <summary>
    /// キー画像を読み込む処理（一度読んだものは使い回す）
    /// </summary>
    Sprite LoadIcon(string name)
    {
        if (m_IconCache.TryGetValue(name, out Sprite sprite)) return sprite;
        sprite = Resources.Load<Sprite>(k_IconFolder + name);
        if (sprite == null) Debug.LogWarning($"キー画像が見つかりません: Resources/{k_IconFolder}{name}");
        m_IconCache[name] = sprite;
        return sprite;
    }

    /// <summary>
    /// 1行（文字 + キー画像）を追加する処理（キーが2つあるときは横に並べる）
    /// </summary>
    void AddRow(string label, Sprite[] icons)
    {
        GameObject row = new GameObject("Row", typeof(RectTransform));
        row.transform.SetParent(m_RowParent, false);
        HorizontalLayoutGroup h = row.AddComponent<HorizontalLayoutGroup>();
        h.childAlignment = TextAnchor.MiddleRight;
        h.spacing = k_KeySpacing;
        h.childControlWidth = true;
        h.childControlHeight = true;
        h.childForceExpandWidth = false;
        h.childForceExpandHeight = false;

        //文字（白に黒ふち）
        GameObject textObj = new GameObject("Label", typeof(RectTransform));
        textObj.transform.SetParent(row.transform, false);
        Text text = textObj.AddComponent<Text>();
        text.font = m_Font;
        text.fontSize = k_FontSize;
        text.color = Color.white;
        text.alignment = TextAnchor.MiddleRight;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.raycastTarget = false;
        text.text = label;
        Outline outline = textObj.AddComponent<Outline>();
        outline.effectColor = Color.black;
        outline.effectDistance = new Vector2(2f, -2f);

        //キー画像（元の大きさで出す）
        foreach (Sprite icon in icons)
        {
            GameObject iconObj = new GameObject("Key", typeof(RectTransform));
            iconObj.transform.SetParent(row.transform, false);
            Image image = iconObj.AddComponent<Image>();
            image.sprite = icon;
            image.raycastTarget = false;
            LayoutElement le = iconObj.AddComponent<LayoutElement>();
            if (icon != null)
            {
                le.preferredWidth = icon.rect.width * k_IconScale;
                le.preferredHeight = icon.rect.height * k_IconScale;
            }
        }
    }

    /// <summary>
    /// 表示用のキャンバスと、行を並べる場所を作る処理
    /// </summary>
    void BuildCanvas()
    {
        Canvas canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 50;
        CanvasScaler scaler = gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.matchWidthOrHeight = 0.5f;

        //右下にくっつけて、下から上へ積む
        GameObject parent = new GameObject("Rows", typeof(RectTransform));
        parent.transform.SetParent(transform, false);
        m_RowParent = parent.GetComponent<RectTransform>();
        m_RowParent.anchorMin = m_RowParent.anchorMax = new Vector2(1f, 0f);
        m_RowParent.pivot = new Vector2(1f, 0f);
        m_RowParent.anchoredPosition = new Vector2(-k_Margin.x, k_Margin.y);
        VerticalLayoutGroup v = parent.AddComponent<VerticalLayoutGroup>();
        v.childAlignment = TextAnchor.LowerRight;
        v.spacing = k_RowSpacing;
        v.childControlWidth = true;
        v.childControlHeight = true;
        v.childForceExpandWidth = false;
        v.childForceExpandHeight = false;
        ContentSizeFitter fitter = parent.AddComponent<ContentSizeFitter>();
        fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        m_Font = FindGameFont();
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
