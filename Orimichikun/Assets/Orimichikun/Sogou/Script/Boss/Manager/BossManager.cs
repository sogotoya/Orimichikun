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
    private void Start()
    {
        m_AB = m_FastTextBoss.GetComponentInChildren<ApproachBoss>();
        if (m_FastText == null || m_BS == null|| m_AB==null||m_STM==null)
        {
            Debug.LogError("アタッチしてください");
        }

        m_FastText.SetActive(false);
    }

    private void Update()
    {
        if (m_BS.m_BossFlag&& m_FastTextBoss!=null)
        {
            StartCoroutine(m_AB.BossMoveStart());
            if (m_AB.m_ApFlag)
            {
                m_FastText.SetActive(true);
            }
        }

        if(m_STM.m_ContactFlag == true)
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

    IEnumerator EndText()
    {
        yield return new WaitForSeconds(2.0f);
        m_PH.Hight();
        m_CS = null;
        Destroy(m_FastTextBoss);
        yield return null;
    }
}
