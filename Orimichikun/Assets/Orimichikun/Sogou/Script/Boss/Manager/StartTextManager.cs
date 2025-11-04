using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTextManager : MonoBehaviour
{
    [SerializeField]
    [Header("コメント")]
    GameObject[] m_Renderer;
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
    }


    private void Update()
    {
            //1回のみ起動
            if (!m_Flag)
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
        yield return new WaitForSeconds(1.0f);
        for (int i = 0; m_Renderer.Length > i; i++)
        {
            m_Renderer[i].SetActive(true);
            yield return new WaitForSeconds(10.0f);
            m_Renderer[i].SetActive(false);
        }
        //Contact()終わったのでtrue
        m_ContactFlag = true;
        yield return null;
    }
}
