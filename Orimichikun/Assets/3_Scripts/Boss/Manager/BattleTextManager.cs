//上のテキスト表示するかしないかのスクリプト
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTextManager : MonoBehaviour
{
    [SerializeField]
    GameObject m_BTMText;

    public bool m_BTMFlag = false;
    private void Start()
    {
        if(m_BTMText==null)
        {
            Debug.LogError("アタッチしてください",this);
        }
        m_BTMText.SetActive(false);
    }

    private void Update()
    {
        //true中は表示、flase中は非表示
        if(m_BTMFlag)
        {
            m_BTMText.SetActive(true);
        }
        else
        {
            m_BTMText.SetActive(false);
        }
    }
}
