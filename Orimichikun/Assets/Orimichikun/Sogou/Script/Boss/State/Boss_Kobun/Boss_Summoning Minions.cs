using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_SummoningMinions : MonoBehaviour
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
            int no = Random.Range(0, m_Enemy.Length);
            Instantiate(m_Enemy[no], m_Tf[i].transform.position, m_Tf[i].rotation);
            Debug.Log("部下召喚");
        }

    }
}
