using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// GameOverの際のスマホ操作処理
/// </summary>
public class SmartPhoneGameOnerUI : MonoBehaviour
{
    [Header("プレイヤーのスクリプト")]
    public PlayScript m_PlayerScript;

    [Header("コイン・回復管理")]
    public CoinCountManager m_CoinCountManager;

    /// <summary>
    /// コンティニューの処理
    /// </summary>
    public void Continue()
    {
        if (m_PlayerScript == null) return;

        // コインリセットフラグを立てる（PlayScriptと同じ処理）
        if (m_CoinCountManager != null)
        {
            m_CoinCountManager.m_CoinReset = true;
        }

        // プレイヤーを復活させる
        m_PlayerScript.PlayerSpawn();
    }
}
