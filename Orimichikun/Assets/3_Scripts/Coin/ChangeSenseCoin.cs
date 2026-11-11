using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSenseCoin : MonoBehaviour
{
    public static ChangeSenseCoin m_InstanceCoin;
    public float CS_CoinCount = 0;
    public float CS_RecoveryCount = 0;
    private void Awake()
    {
        if (m_InstanceCoin==null)
        {
            m_InstanceCoin = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
