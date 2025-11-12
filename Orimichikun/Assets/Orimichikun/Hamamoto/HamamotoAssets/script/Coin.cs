using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("コイン獲得のエフェクト")]
    public GameObject m_effect;
    [Header("コインのSprite")]
    public SpriteRenderer m_Coin;
    [Header("コイン獲得SE")]
    public AudioClip m_CoinGetSE;
    public Animator m_animator;
    public float m_CoinCount=0f;
    public AudioSource m_CoinSource;
    //コインをゲットしたかどうか
    public bool m_CoinGet=false;
    //エフェクトしたかどうか
    public bool m_Iseffect=false;
    private void Start()
    {
        //非表示
        m_effect.SetActive(false);
        //格納
        m_CoinSource =GetComponent<AudioSource>();
        m_animator = GetComponentInChildren<Animator>();
        // 自動登録
        FindObjectOfType<CoinCountManager>().RegisterCoin(this);
    }
    private void Update()
    {
        //コインをゲットしていてエフェクトを流してなければエフェクト出現
        if (m_CoinGet&&!m_Iseffect)
        {
            m_effect.SetActive(true);
            Destroy(gameObject,0.5f);
            m_Iseffect = true;
            
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //プレイヤーが拾ったらSEを入れてコイン削除
        if (other.CompareTag("Player"))
        {
            m_CoinGet=true;
            m_CoinSource.PlayOneShot(m_CoinGetSE);
           Destroy(m_Coin);
            GetComponent<Collider2D>().enabled = false;
        }
    }


}
