
//スポーンする場所に危険マークを表示させるスクリプト
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderWarning : MonoBehaviour
{
    [SerializeField]
    [Header("危険マークオブジェクト")]
    GameObject m_WarningObj;

    [SerializeField]
    [Header("生成場所")]
    Transform[] m_Position;
    [Tooltip("生成場所に生成したオブジェクトのリスト")]
    List<GameObject> m_WarnObjList = new List<GameObject>();

    [SerializeField]
    public bool m_IsThunderWarning = false;
    private void Start()
    {
        if (m_WarningObj == null)
        {
            Debug.LogError($"{nameof(m_WarningObj)}アタッチしてください", this);
        }
        for (int i = 0; i < m_Position.Length; i++)
        {
            GameObject obj = Instantiate(m_WarningObj, m_Position[i].position, m_Position[i].rotation);
            obj.SetActive(false);
            m_WarnObjList.Add(obj);
        }
    }

    /// <summary>
    /// マーク順番の表示
    /// </summary>
    public IEnumerator OnThunderWarningCoroutine()
    {
        yield return StartCoroutine(OnThunder());
    }
    IEnumerator OnThunder()
    {
        for (int i = 0; i < m_Position.Length; i++)
        {
            m_WarnObjList[i].SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
        OffThunderWarning();
    }

    /// <summary>
    /// マーク順番の非表示
    /// </summary>
    public void OffThunderWarning()
    {
        StartCoroutine(OffThunder());
    }
    IEnumerator OffThunder()
    {
        for (int i = 0; i < m_Position.Length; i++)
        {
            m_WarnObjList[i].SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }

}
