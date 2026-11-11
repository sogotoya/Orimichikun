using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItem : MonoBehaviour
{
    [SerializeField]
    GameObject m_Player;

    private void Start()
    {
        if (m_Player == null)
            Debug.Log("m_Playerにアタッチされていません");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject==m_Player)
        {
            Destroy(gameObject);
        }
    }
}
