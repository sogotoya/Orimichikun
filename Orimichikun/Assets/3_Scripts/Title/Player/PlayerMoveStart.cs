using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveStart : MonoBehaviour
{
    [SerializeField]
    [Header("Bossをアタッチ")]
    GameObject m_Boss;

    [Header("Playerについているスクリプトをアタッチ")]
    [SerializeField]
    PlayScript m_PalyerController;
    [SerializeField]
    playershoot m_TutorialPlayershoot;
    [SerializeField]
    PlayerJump m_PlayerJump;

    [Header("開始時に出すジャンプの説明（コインを取ったら消す。未設定なら出さない）")]
    [SerializeField]
    GameObject m_StartJumpGuide;
    [SerializeField, Tooltip("最初に取るコイン（これが無くなったらジャンプの説明を消す）")]
    GameObject m_StartCoin;

    [Header("動けるようになったときに出す移動の説明（左スティックが光る絵、未設定なら出さない）")]
    [SerializeField]
    GameObject m_MoveGuide;
    [SerializeField, Tooltip("プレイヤーの頭からどれだけ上に出すか")]
    Vector2 m_MoveGuideOffset = new Vector2(0f, 0.9f);
    [SerializeField, Tooltip("この距離だけ動いたら説明を消す")]
    float m_MoveGuideHideDistance = 1.5f;

    //1回のみの起動用
    bool m_ChangeFlag=false;
    //移動の説明を出した位置（プレイヤーの x）
    float m_GuideStartX;
    private void Awake()
    {
        m_PalyerController.enabled = false;
        m_TutorialPlayershoot.enabled = false;
        if (m_MoveGuide != null) m_MoveGuide.SetActive(false);
        //最初はジャンプしてコインを取るので、ジャンプの説明を出しておく
        if (m_StartJumpGuide != null) m_StartJumpGuide.SetActive(m_StartCoin != null);
    }
    private void Update()
    {
        //コインを取ったらジャンプの説明を消す
        if (m_StartJumpGuide != null && m_StartJumpGuide.activeSelf && m_StartCoin == null)
        {
            m_StartJumpGuide.SetActive(false);
        }

        if (m_Boss == null&&!m_ChangeFlag)
        {
            m_PlayerJump.enabled = false;
            m_PalyerController.enabled = true;
            m_TutorialPlayershoot.enabled=true;
            m_ChangeFlag = true;
            ShowMoveGuide();
        }

        //少し動いたら移動の説明を消す（一度消したら出さない）
        if (m_MoveGuide != null && m_MoveGuide.activeSelf)
        {
            float moved = Mathf.Abs(m_PalyerController.transform.position.x - m_GuideStartX);
            if (moved >= m_MoveGuideHideDistance) m_MoveGuide.SetActive(false);
        }
    }

    /// <summary>
    /// 動けるようになった瞬間に、プレイヤーの頭の上へ移動の説明を出す
    /// </summary>
    void ShowMoveGuide()
    {
        if (m_MoveGuide == null) return;
        Vector3 p = m_PalyerController.transform.position;
        m_GuideStartX = p.x;
        m_MoveGuide.transform.position = new Vector3(p.x + m_MoveGuideOffset.x, p.y + m_MoveGuideOffset.y, m_MoveGuide.transform.position.z);
        m_MoveGuide.SetActive(true);
    }
}
