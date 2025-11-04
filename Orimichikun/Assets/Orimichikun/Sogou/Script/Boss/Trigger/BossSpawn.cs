using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawn : MonoBehaviour
{
    [SerializeField]
    [Header("Bossのゲームオブジェクト")]
    GameObject m_Boss;

    [SerializeField]
    [Header("Bossの出現位置")]
    Transform m_SpawnTf;

    //1回のみの判定フラグ
    bool m_BossFlag;
    private void OnTriggerStay2D(Collider2D other)
    {
        //プレイヤーのタグと一致したら
        if (other.CompareTag("Player"))
        {
            Debug.Log("プレイヤーが範囲内にいる");
            if(!m_BossFlag)
            {
                //生成
                Instantiate(m_Boss,m_SpawnTf.position,m_SpawnTf.rotation);

                m_BossFlag = true;
            }
        }
    }
}
