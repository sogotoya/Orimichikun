using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTextManager : MonoBehaviour
{
    public System.Action EndBoss;

    [SerializeField]
    [Header("コメント")]
    GameObject[] m_Renderer;
    //繰り返し呼ばれない対策
    bool m_EndContactFlag = false;
    //1回のみ呼ばれるフラグ
    bool m_Flag = false;
    private void Start()
    {
        for (int i = 0; m_Renderer.Length > i; i++)
        {
            m_Renderer[i].SetActive(false);
        }
    }

    /// <summary>
    /// 最終会話スタート
    /// </summary>
    public void EndTextStart()
    {
        //1回のみ起動
        if (!m_EndContactFlag)
        {
            StartCoroutine(Contact());
            m_EndContactFlag = true;
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
            yield return new WaitForSeconds(5.0f);
            m_Renderer[i].SetActive(false);
        }
        EndBoss.Invoke();
        yield return null;
    }
}
