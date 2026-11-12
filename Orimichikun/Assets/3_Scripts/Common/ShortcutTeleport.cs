//デバッグ用ショートカット: キーを押すとプレイヤーを指定の場所へテレポートする
//Stage では「」」キーでボス戦の入口（=マップ=/Tp/Checkpoint (1)、E キーでボス戦へ進む所）へ飛ぶ
//日本語キーボードの「」」キーは Unity では ] キー（KeyCode.RightBracket）として扱われる
using UnityEngine;

public class ShortcutTeleport : MonoBehaviour
{
    [SerializeField, Tooltip("テレポート先")]
    Transform m_Target;

    [SerializeField, Tooltip("テレポートさせるプレイヤー")]
    PlayScript m_Player;

    [SerializeField, Tooltip("テレポートするキー（」キー = RightBracket）")]
    KeyCode m_Key = KeyCode.RightBracket;

    [SerializeField, Tooltip("入力された文字でも判定する（キー配置の違い対策）")]
    string m_KeyChars = "]」";

    [SerializeField, Tooltip("テレポート先をリスポーン地点にする")]
    bool m_UpdateSavePoint = true;

    [SerializeField, Tooltip("エディタと Development Build でだけ使える（製品版では無効）")]
    bool m_DebugOnly = true;

    private void Update()
    {
        if (m_DebugOnly && !Debug.isDebugBuild) return;
        if (m_Target == null || m_Player == null) return;
        if (LoadingScreen.IsLoading) return;

        if (IsPressed()) Teleport();
    }

    bool IsPressed()
    {
        if (Input.GetKeyDown(m_Key)) return true;

        string typed = Input.inputString;
        if (string.IsNullOrEmpty(typed) || string.IsNullOrEmpty(m_KeyChars)) return false;
        foreach (char c in typed)
        {
            if (m_KeyChars.IndexOf(c) >= 0) return true;
        }
        return false;
    }

    void Teleport()
    {
        Vector3 pos = m_Target.position;
        pos.z = m_Player.transform.position.z;

        var rb = m_Player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.position = pos;
        }
        m_Player.transform.position = pos;

        if (m_UpdateSavePoint) m_Player.UpdateSavePoint(pos);
        Debug.Log($"ショートカット: {m_Target.name} へテレポート {pos}", this);
    }
}
