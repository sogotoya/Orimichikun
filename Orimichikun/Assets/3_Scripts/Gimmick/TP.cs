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
    [Header("プレイヤーのscript")]
    public PlayScript m_PlayScript;
    [Header("スマホUIの参照")]
    public SmartPhoneUI m_SmartPhoneUI;
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

            // スマホUIの「次へ」ボタンを表示（テレポート用ボタンとして流用）
            if (m_SmartPhoneUI != null)
            {
                m_SmartPhoneUI.ShowNextStageUI(true, TpPlayerFromMobile);
            }
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

            // スマホUIの「次へ」ボタンを非表示
            if (m_SmartPhoneUI != null)
            {
                m_SmartPhoneUI.ShowNextStageUI(false);
            }
        }
    }
    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown("joystick button 4")|| Input.GetKeyDown("joystick button 5")) && m_PlayerTpTrigger)
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
            SceneManager.LoadScene("BossStage");
            m_Bgm2.Play();
        }
    }

    /// <summary>
    /// スマホUIからテレポートを実行するための関数
    /// </summary>
    public void TpPlayerFromMobile()
    {
        if (m_PlayerTpTrigger)
        {
            TpPlayer();
        }
    }
}
