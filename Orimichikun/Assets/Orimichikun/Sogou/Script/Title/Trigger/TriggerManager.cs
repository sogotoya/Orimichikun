using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// チュートリアルの表示やスポーン管理
/// </summary>
public class TriggerManager : MonoBehaviour
{
    //ジャンプ表示
    public bool m_IsJump;


    //ダブルジャンプ表示
    public bool m_IsDoubleJump;


    //敵表示
    public bool m_IsEnemy;
    [Header("敵の出現位置とプレハブ")]
    [SerializeField]
    GameObject m_Enemy;
    [SerializeField]
    Transform m_Tf;
    bool m_IsSpown;//1回のみ生成


    [SerializeField]
    [Header("ゴーストジャンプアニメーションが入っているオブジェクト")]
    GameObject m_GhostJump;
    [SerializeField]
    [Header("ゴーストダブルジャンプアニメーションが入っているオブジェクト")]
    GameObject m_GhostDoubleJump;
    private void Update()
    {
        //範囲内にいるなら表示・範囲内にいなければ非表示
        if(m_IsJump)
        {
            m_GhostJump.SetActive(true);
        }
        else
        {
            m_GhostJump.SetActive(false);
        }
        if (m_IsDoubleJump)
        {
            m_GhostDoubleJump.SetActive(true);
        }
        else
        {
            m_GhostDoubleJump.SetActive(false);
        }

        if(m_IsEnemy)
        {
            if(!m_IsSpown)
            {
                EnemySpown();
            }
        }
    }


    /// <summary>
    /// 敵の生成
    /// </summary>
    void EnemySpown()
    {
        Instantiate(m_Enemy, m_Tf.position, m_Enemy.transform.rotation);
        m_IsSpown = true;
    }
}
