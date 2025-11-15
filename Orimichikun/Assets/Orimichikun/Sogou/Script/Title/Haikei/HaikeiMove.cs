using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaikeiMove : MonoBehaviour
{
    public float m_WalkSpeed = 0.5f;

    [SerializeField]
    [Header("背景のオブジェクト（左→右の順に並べる）")]
    Transform[] m_Haikei;

    [SerializeField]
    [Header("背景1枚分の幅")]
    Vector3 m_HaikeiTf;   

    Transform m_LastTf;
    Vector3 m_FastTf;

    private void Start()
    {
        if (m_Haikei.Length == 0)
        {
            Debug.LogError("背景がセットされていません");
            return;
        }

        //最後の背景（右端）
        m_LastTf = m_Haikei[m_Haikei.Length - 1];

        //最前（左端）の位置-背景幅
        m_FastTf = m_Haikei[0].position - m_HaikeiTf;
    }

    private void Update()
    {
        //全背景を左へ移動
        for (int i = 0; i < m_Haikei.Length; i++)
        {
            if (m_Haikei[i] == null) continue; 

            m_Haikei[i].position += Vector3.left * Time.deltaTime * m_WalkSpeed;

            // 一番左を通り過ぎたら右へワープ
            if (m_Haikei[i].position.x < m_FastTf.x)
            {
                m_Haikei[i].position = m_LastTf.position + m_HaikeiTf;
                m_LastTf = m_Haikei[i];
            }
        }
    }

}
