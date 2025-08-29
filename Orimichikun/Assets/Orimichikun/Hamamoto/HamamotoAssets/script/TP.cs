using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP : MonoBehaviour
{
    [Header("キャラクター")]
    public GameObject m_Player;
    [Header("テレポート先")]
    public Transform m_TP;
    //プレイヤーがTPエリアに入ったか？
    private bool m_PlayerTpTrigger = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //プレイヤーがエリアに入ったら
        if (collision.gameObject == m_Player)
        {
            m_PlayerTpTrigger = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //プレイヤーがエリアに外れたら
        if (collision.gameObject == m_Player)
        {
            m_PlayerTpTrigger = false;
        }
    }
    private void Update()
    {
        if (m_PlayerTpTrigger && Input.GetKeyDown(KeyCode.E))
        {
            TpPlayer();
        }
    }
    private void TpPlayer()
    {
        if (m_TP != null)
        {
            //プレイヤーがTPに移動
            m_Player.transform.position = m_TP.position;
        }
    }
}
