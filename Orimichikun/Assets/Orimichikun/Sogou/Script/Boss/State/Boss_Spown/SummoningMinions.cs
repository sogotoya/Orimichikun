//敵をスポーンさせる処理
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummoningMinions : MonoBehaviour
{
    [SerializeField]
    [Header("スポーンさせたいEnemyセット")]
    GameObject[] m_Enemy;

    [SerializeField]
    Transform[] m_Tf;

    /// <summary>
    /// 配下を召喚
    /// </summary>
    public void RandomSpown()
    {
        for (int i = 0; i < m_Tf.Length; i++)
        {
            Instantiate(m_Enemy[i], m_Tf[i].transform.position, m_Tf[i].rotation);
            Debug.Log("部下召喚");
        }

    }
}
