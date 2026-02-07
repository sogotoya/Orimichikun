using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
}
