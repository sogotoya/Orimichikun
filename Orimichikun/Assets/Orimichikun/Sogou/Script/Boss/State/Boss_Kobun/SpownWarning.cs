//スポーンする場所に危険マークを表示させるスクリプト
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpownWarning : MonoBehaviour
{
    [SerializeField]
    [Header("危険マークオブジェクト")]
    GameObject m_WarningObj;

    [SerializeField]
    [Header("生成場所")]
    Transform[] m_Position;
    [Tooltip("生成場所に生成したオブジェクトのリスト")]
    List<GameObject> m_WarnObjList;
    private void Start()
    {
        if(m_WarningObj==null)
        {
            Debug.LogError($"{nameof(m_WarningObj)}アタッチしてください",this);
        }
        for(int i = 0; i < m_Position.Length; i++)
        {
            //m_WarnObjList=Instantiate(m_WarningObj, m_Position[i].position, m_Position[i].rotation);
        }
    }

    public void OnWarning()
    {

    }
}
