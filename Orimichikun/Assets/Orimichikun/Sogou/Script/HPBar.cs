using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    Slider m_Slider;
    public Parameta2D m_Parameta2;
    void Start()
    {
        // スライダーを取得する
        m_Slider = GameObject.Find("Slider").GetComponent<Slider>();
        if(m_Parameta2 != null )
        {
            Debug.Log("Parameta2Dアタッチされてません");
        }
    }

    float m_Hp = 0;
    bool m_Switch = false;
    void Update()
    {
        if ((!m_Switch))
        {
            // HP上昇
            m_Hp += 0.003f;
            if (m_Hp > 1)
            {
                m_Switch = true;
                m_Slider.maxValue = m_Parameta2.m_MaxHp;
                m_Slider.value = m_Parameta2.m_Hp;
            }

            // HPゲージに値を設定
            m_Slider.value = m_Hp;
        }
        else
        {
            m_Slider.value = m_Parameta2.m_Hp;
        }
    }

}
