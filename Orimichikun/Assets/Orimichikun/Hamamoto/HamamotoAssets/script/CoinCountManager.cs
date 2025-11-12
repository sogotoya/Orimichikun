using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CoinCountManager : MonoBehaviour
{
    [Header("コインの数")]
    public float m_CoinCount=0;
    [Header("回復アイテムの数")]
    public float m_RecoveryCount = 0;
    [Header("回復量")]
    public int m_HPRecovery = 2;
    [Header("回復SE")]
    public AudioClip m_RecoverySE;
    [Header("プレイヤーのParameta")]
    public Parameta2D m_PlayerParameta;
    [Header("コインの獲得数Text")]
    [SerializeField]private Text m_CoinCountText;
    [Header("コインの獲得数Text")]
    [SerializeField] private Text m_RecoveryCountText;
    private AudioSource m_Source;
    //コインのscript
    private Coin[] m_Coin;
    public List<Coin> m_Coins = new List<Coin>();
    public bool m_CoinReset=false;

    public void RegisterCoin(Coin coin)
    {
        if (!m_Coins.Contains(coin))
            m_Coins.Add(coin);
    }
    private void Start()
    {
        m_Source = GetComponent<AudioSource>();
    }
    private void Update()
    {

        //コインを取るたび加算される
        foreach (Coin coin in m_Coins)
        {
            if (coin.m_CoinGet)
            {
                m_CoinCount += 1;
                coin.m_CoinGet = false;
                //カウントが１０に達したらリセットと回復アイテム加算
                if (m_CoinCount>=10)
                {
                    m_RecoveryCount += 1;
                    m_CoinCount = 0;
                    UpdateCoinText();
                    UpdateRecoveryText();
                }
                else
                {
             
                    UpdateCoinText();
                }
               
            }
        }
        //回復アイテムを一個以上ゲットしたら使用可能
        if (m_RecoveryCount>=1)
        {      
            if (Input.GetKeyDown(KeyCode.E))
            {
                //回復
                Recovery();
            }
        }
        //死んだらリセット
        if (m_CoinReset)
        {
            m_CoinReset= false;
            m_RecoveryCount = 0;
            m_CoinCount = 0; 
            UpdateCoinText(); 
            UpdateRecoveryText();
        }
    }
    void UpdateCoinText()
    {
        //Textにコイン加算を表示
        m_CoinCountText.text="x"+((int)m_CoinCount).ToString();
    }
    void UpdateRecoveryText()
    {
        //Textに回復加算を表示
        m_RecoveryCountText.text = "x" + ((int)m_RecoveryCount).ToString();
    }
    void Recovery()
    {
        if (m_PlayerParameta.m_Hp<m_PlayerParameta.m_MaxHp)
        {
            m_Source.PlayOneShot(m_RecoverySE);
            m_PlayerParameta.m_Hp += m_HPRecovery;
            m_RecoveryCount -= 1;
            UpdateRecoveryText();
        }
        else
        {
            Debug.Log("HPは満タンなので回復できません");
        }
    }
}
