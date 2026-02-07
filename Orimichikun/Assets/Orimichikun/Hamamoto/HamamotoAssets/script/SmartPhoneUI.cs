using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// スマホの操作の処理
/// </summary>
public class SmartPhoneUI : MonoBehaviour
{
    [Header("プレイヤーのスクリプト")]
    public PlayScript playerScript;

    [Header("タイトル用ジャンプスクリプト（任意）")]
    public PlayerJump titleJumpScript;

    [Header("移動用ジョイスティック")]
    public FixedJoystick moveJoystick;

    [Header("次へ進む用ボタンUI"), SerializeField]
    private GameObject m_NextStageUI;

    [Header("回復するUI"), SerializeField]
    private GameObject m_HeelUI;

    [Header("コイン・回復管理")]
    public CoinCountManager m_CoinCountManager;

    // ボタンが押された時に実行する処理を保持する（タイトル用、TP用など）
    private System.Action m_OnNextStageClick;

    /// <summary>
    /// 開始
    /// </summary>
    private void Start()
    {
        if (m_NextStageUI != null) m_NextStageUI.SetActive(false);
        if (m_HeelUI != null) m_HeelUI.SetActive(false);
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Update()
    {
        if (playerScript != null && playerScript.enabled && moveJoystick != null)
        {
            // ジョイスティックの水平入力をプレイヤーに送る
            playerScript.SetMobileInput(moveJoystick.Horizontal);
        }
    }

    public void Jamp()
    {
        if (playerScript != null && playerScript.enabled)
        {
            playerScript.MobileJump();
        }
        else if (titleJumpScript != null && titleJumpScript.enabled)
        {
            titleJumpScript.MobileJump();
        }
    }

    /// <summary>
    /// 攻撃処理
    /// </summary>
    public void Attack()
    {
        if (playerScript != null && playerScript.enabled)
        {
            playerScript.MobileAttack();
        }
    }

    /// <summary>
    /// 次へ進むボタンを表示/非表示にする。実行する処理(Action)を渡せる。
    /// </summary>
    public void ShowNextStageUI(bool show, System.Action onClick = null)
    {
        m_OnNextStageClick = onClick;
        if (m_NextStageUI != null)
        {
            m_NextStageUI.SetActive(show);
        }
    }

    /// <summary>
    /// 次へ進むボタンが押された時の処理
    /// </summary>
    public void NextStage()
    {
        // 渡された処理がある場合はそれを実行
        if (m_OnNextStageClick != null)
        {
            m_OnNextStageClick.Invoke();
        }
        else
        {
            // デフォルト：ChangeStage.cs や ChangeScene.cs と同様に Stage シーンを読み込む
            SceneManager.LoadScene("Stage");
        }
    }

    /// <summary>
    /// 回復ボタンの表示・非表示を切り替える
    /// </summary>
    public void ShowHeelUI(bool show)
    {
        if (m_HeelUI != null)
        {
            m_HeelUI.SetActive(show);
        }
    }

    /// <summary>
    /// 回復をするときのUI処理
    /// </summary>
    public void Heel()
    {
        if (m_CoinCountManager != null)
        {
            m_CoinCountManager.Recovery();
        }
    }
}
