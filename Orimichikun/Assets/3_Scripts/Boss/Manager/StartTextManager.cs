//Boss戦の時Bossとの会話をするときに使用するスクリプト
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTextManager : MonoBehaviour
{
    [SerializeField]
    [Header("コメント")]
    GameObject[] m_Renderer;

    [SerializeField]
    [Tooltip("会話を表示するキャンバス。これが画面に出てから会話を始める（未設定ならコメントの親）")]
    GameObject m_ShowRoot;

    //IEnumerator Contact()が終わったかの判定
    public bool m_ContactFlag = false;
    //1回のみ呼ばれるフラグ
    bool m_Flag = false;
    private void Start()
    {
        for (int i = 0; m_Renderer.Length > i; i++)
        {
            m_Renderer[i].SetActive(false);
        }
        if (m_ShowRoot == null && m_Renderer.Length > 0 && m_Renderer[0].transform.parent != null)
        {
            m_ShowRoot = m_Renderer[0].transform.parent.gameObject;
        }
    }


    private void Update()
    {
            //会話のキャンバスが画面に出てから、1回のみ起動
            //（ボスが近づいている間にタイマーが進んで、最初の会話がすぐ消えるのを防ぐ）
            if (!m_Flag && (m_ShowRoot == null || m_ShowRoot.activeInHierarchy))
            {
                StartCoroutine(Contact());
                m_Flag = true;
            }
    }
    /// <summary>
    /// それぞれのコメント順に表示
    /// </summary>
    /// <returns></returns>
    public IEnumerator Contact()
    {
        yield return new WaitForSeconds(MessageTiming.StartDelay);
        for (int i = 0; m_Renderer.Length > i; i++)
        {
            m_Renderer[i].SetActive(true);
            yield return new WaitForSeconds(MessageTiming.DisplayTime);
            m_Renderer[i].SetActive(false);
        }
        //Contact()終わったのでtrue
        m_ContactFlag = true;
        yield return null;
    }
}
