using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTrigger : MonoBehaviour
{
    [SerializeField]
    AudioSource m_Source;
    [SerializeField]
    BossManager m_Manager;
    [SerializeField]
    GameObject m_HPBar;
    [SerializeField]
    GameObject m_TitleBar;
    [SerializeField]
    GameObject m_TitleNextBar;
    private void Start()
    {
        m_Source.Stop();
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer==LayerMask.NameToLayer("Player"))
        {
            m_Source.Play();
            m_Manager.m_Clear.SetActive(true);
            m_HPBar.SetActive(false);
            m_TitleBar.SetActive(false);
            m_TitleNextBar.SetActive(true);
            Destroy(gameObject);
        }
    }
}
