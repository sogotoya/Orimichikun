//会話スタート時Bossの出現を管理するスクリプト
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawn : MonoBehaviour
{
    //1回のみの判定フラグ
    [Header("ボス出現フラグ")]
    public bool m_BossFlag;

    private void OnTriggerStay2D(Collider2D other)
    {
        //プレイヤーのタグと一致したら
        if (other.CompareTag("Player"))
        {
            Debug.Log("プレイヤーが範囲内にいる");
            if(!m_BossFlag)
            {
                m_BossFlag = true;
            }
        }
    }
}
