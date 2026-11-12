//チュートリアル時にBossと会話する時のスクリプト
using System.Collections;
using UnityEngine;

public class FastMessage : MonoBehaviour
{
    [SerializeField]
    PanelHight m_PH;

    [SerializeField]
    GameObject m_EnemyCoin;

    [SerializeField]
    [Tooltip("最初の会話の後に再生するコイン奪取の演出（未設定なら従来の暗転）")]
    CoinSteal m_CoinSteal;

    [SerializeField]
    GameObject[] m_Renderer;
    //前の行動が終わったかの判定
    public bool m_MessageFlag=false;
    //IEnumerator Contact()が終わったかの判定
    public bool m_ContactFlag=false;
    //1回のみ呼ばれるフラグ
    bool m_Flag = false;
    private void Start()
    {
        for (int i = 0; m_Renderer.Length > i; i++)
        {
            m_Renderer[i].SetActive(false);
        }
        m_EnemyCoin.SetActive(false);
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
        yield return new WaitForSeconds(MessageTiming.StartDelay);
        for (int i = 0; m_Renderer.Length > i; i++)
        {
            //会話を表示して、決まった秒数（MessageTiming.DisplayTime）で次へ切り替える
            m_Renderer[i].SetActive(true);
            yield return new WaitForSeconds(MessageTiming.DisplayTime);
            m_Renderer[i].SetActive(false);

            //0番目の会話の後: コインを奪われる
            if(i==0)
            {
                if (m_CoinSteal != null)
                {
                    //攻撃 → 暗転 → 被ダメの演出
                    yield return m_CoinSteal.Play();
                    yield return new WaitForSeconds(1.0f);
                }
                else
                {
                    //従来: 暗転してボスがコインを持つ
                    m_PH.Hight();
                    m_EnemyCoin.SetActive(true);
                }
            }
        }
        //Contact()終わったのでtrue
        m_ContactFlag = true;
        yield return null;
    }
}