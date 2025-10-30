using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastMessage : MonoBehaviour
{
    [SerializeField]
    PanelHight m_PH;

    [SerializeField]
    GameObject[] m_Renderer;
    //前の行動が終わったかの判定
    public bool m_MessageFlag;
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
        if (m_MessageFlag)
        {
            //1回のみ起動
            if (!m_Flag)
            {
                StartCoroutine(Contact());
                m_Flag = true;
            }
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

            yield return new WaitForSeconds(1.5f);
            //0番目だった場合暗転
            if(i==0)
            {
                m_PH.Hight();

            }
            yield return new WaitForSeconds(1.5f);
            m_Renderer[i].SetActive(false);
        }
        yield return null;
    }
}
