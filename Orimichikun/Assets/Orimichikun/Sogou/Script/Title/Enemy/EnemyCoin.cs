using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCoin : MonoBehaviour
{
    public GameObject m_Coin;

    private void Start()
    {
        if (m_Coin == null) return;
        m_Coin.SetActive(false);
    }

    /// <summary>
    /// ƒRƒCƒ“‚Ì•\Ž¦
    /// </summary>
    public void OnCoin()
    {
        m_Coin.SetActive(true);
    }
}
