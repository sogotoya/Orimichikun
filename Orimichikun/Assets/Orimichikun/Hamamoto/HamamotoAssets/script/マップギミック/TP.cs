using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TP : MonoBehaviour
{
    public AudioSource m_Bgm1;
    public AudioSource m_Bgm2;
    [Header("画像")]
    public GameObject m_Image;
    public GameObject m_Image2;
    [Header("キャラクター")]
    public GameObject m_Player;
    [Header("テレポート先")]
    public Transform m_TP;
    //プレイヤーがTPエリアに入ったか？
    private bool m_PlayerTpTrigger = false;

    private void Start()
    {
        m_Image.SetActive(false);
        m_Image2.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //プレイヤーがエリアに入ったら
        if (collision.gameObject == m_Player)
        {
            m_PlayerTpTrigger = true;
            //表示
            m_Image.SetActive(true);
            m_Image2.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //プレイヤーがエリアに外れたら
        if (collision.gameObject == m_Player)
        {
            m_PlayerTpTrigger = false;
            //非表示
            m_Image.SetActive(false);
            m_Image2.SetActive(false);
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
            m_Bgm1.Stop();
            //シーンに移動
            SceneManager.LoadScene("TestSense");

            m_Bgm2.Play();
        }
    }
}
