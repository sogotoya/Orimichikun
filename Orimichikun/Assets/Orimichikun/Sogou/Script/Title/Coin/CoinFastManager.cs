//タイトルの最初のコインに関する処理
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinFastManager : MonoBehaviour
{
    [SerializeField]
    BossFastManager m_BFM;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ヒットしたら削除
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        //削除されたらフラグON
        m_BFM.m_IsCoin = true;
    }
}
