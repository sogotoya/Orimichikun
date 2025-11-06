//Bossに関することを管理しているマネージャー
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [SerializeField]
    [Header("ボスの再登場テキスト")]
    GameObject m_FastText;

    [SerializeField]
    [Header("ボステキストオブジェクト")]
    GameObject m_FastTextBoss;

    [SerializeField]
    BossSpawn m_BS;
    [SerializeField]
    ApproachBoss m_AB;
    [SerializeField]
    StartTextManager m_STM;
    [SerializeField]
    PanelHight m_PH;
    [SerializeField]
    CameraShake m_CS;
    //1回のみ呼び出す対策
    bool m_Flag=false;
    bool m_RoarFlag=false;
    bool m_CSFlag = false;


    [SerializeField]
    PlayerControlManager m_PCM;
    [SerializeField]
    BattleTextManager m_BTM;

    private void Start()
    {
        m_AB = m_FastTextBoss.GetComponentInChildren<ApproachBoss>();
        if (m_FastText == null || m_BS == null|| m_AB==null||m_STM==null||m_PCM==null||m_BTM==null)
        {
            Debug.LogError("アタッチしてください",this);
        }

        m_FastText.SetActive(false);
    }

    private void Update()
    {
        //ボスが出現したら移動開始
        if (m_BS.m_BossFlag && !m_CSFlag)
        {
            StartCoroutine(m_AB.BossMoveStart());
            //移動完了したら会話開始
            if (m_AB.m_ApFlag)
            {
                m_FastText.SetActive(true);
            }
        }
        //会話終わり間際咆哮開始
        if(m_STM.m_ContactFlag == true&&!m_CSFlag)
        {
            if (!m_RoarFlag)
            {
                m_RoarFlag = true;
                Animator anim = m_FastTextBoss.GetComponentInChildren<Animator>();
                anim.SetTrigger("Roar");
                StartCoroutine(EndText());
            }
            StartCoroutine(m_CS.Shake(0.5f, 0.1f,0.78f));
        }
    }


    //会話終了時に呼ばれるコルーチン
    IEnumerator EndText()
    {
        yield return new WaitForSeconds(2.0f);
        m_PH.Hight();
        m_CSFlag = true;
        m_PCM.m_IsPlaying = true;
        m_BTM.m_BTMFlag = true;
        Destroy(m_FastTextBoss);
        yield return null;
    }
}
