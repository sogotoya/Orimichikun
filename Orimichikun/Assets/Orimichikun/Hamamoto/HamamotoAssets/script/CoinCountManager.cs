using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [Header("スマホUIの参照")]
    public SmartPhoneUI m_SmartPhoneUI;
    [Header("回復のKey表示")]
    public GameObject m_RecoveryKey;
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
    private void Awake()
    {
        m_RecoveryKey.SetActive(false);
        m_Source = GetComponent<AudioSource>();

        // 値を安全に引き継ぐ
        if (ChangeSenseCoin.m_InstanceCoin != null)
        {
            m_CoinCount = ChangeSenseCoin.m_InstanceCoin.CS_CoinCount;
            m_RecoveryCount = ChangeSenseCoin.m_InstanceCoin.CS_RecoveryCount;
        }

        UpdateCoinText();
        UpdateRecoveryText();
    }
    private void Update()
    {

        //コインを取るたび加算される
        foreach (Coin coin in m_Coins)
        {   
            if (coin.m_CoinGetCount)
            {
                m_CoinCount += 1;
                coin.m_CoinGetCount = false;
                //カウントが１０に達したらリセットと回復アイテム加算
                if (m_CoinCount>=10)
                {
                    m_RecoveryCount += 1;
                    m_CoinCount = 0;
                   
                }

                if (ChangeSenseCoin.m_InstanceCoin != null)
                {
                    ChangeSenseCoin.m_InstanceCoin.CS_CoinCount = m_CoinCount;
                    ChangeSenseCoin.m_InstanceCoin.CS_RecoveryCount = m_RecoveryCount;
                }
                UpdateCoinText();
                UpdateRecoveryText();
            }
        }
        //回復アイテムを一個以上ゲットしたら使用可能
        if (m_RecoveryCount>=1)
        {
           //Hpが削れていたら回復Key表示
            if (m_PlayerParameta.m_Hp < m_PlayerParameta.m_MaxHp)
            {              
                m_RecoveryKey.SetActive(true);
                if (m_SmartPhoneUI != null) m_SmartPhoneUI.ShowHeelUI(true);
            }
            else
            {
                m_RecoveryKey.SetActive(false);
                if (m_SmartPhoneUI != null) m_SmartPhoneUI.ShowHeelUI(false);
            }
            if (Input.GetKeyDown(KeyCode.E)|| Input.GetKeyDown("joystick button 4"))
            {
                //回復
                Recovery();
            }
        }
        else
        {
            m_RecoveryKey.SetActive(false);
            if (m_SmartPhoneUI != null) m_SmartPhoneUI.ShowHeelUI(false);
        }
        //死んだらリセット
        if (m_CoinReset)
        {
            m_CoinReset= false;
            m_RecoveryCount = 0;
            m_CoinCount = 0;
            ChangeSenseCoin.m_InstanceCoin.CS_CoinCount = 0;
            ChangeSenseCoin.m_InstanceCoin.CS_RecoveryCount = 0;

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
    public void Recovery()
    {
        if (m_PlayerParameta.m_Hp<m_PlayerParameta.m_MaxHp)
        {
            m_Source.PlayOneShot(m_RecoverySE);
            m_PlayerParameta.m_Hp += m_HPRecovery;
            ChangeSenseHP.m_Instance.PlayerHP+=m_HPRecovery; 
           m_RecoveryCount -= 1;
            ChangeSenseCoin.m_InstanceCoin.CS_RecoveryCount = m_RecoveryCount;
            UpdateRecoveryText();
        }
        else
        {
            Debug.Log("HPは満タンなので回復できません");
        }
    }
}
