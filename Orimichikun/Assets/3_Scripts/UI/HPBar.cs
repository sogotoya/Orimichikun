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
        // スライダー取得
        m_Slider = GameObject.Find("Slider").GetComponent<Slider>();

        // スタート時にプレイヤーHPをセット
        m_Slider.maxValue = m_Parameta2.m_MaxHp;
        m_Slider.value = m_Parameta2.m_Hp;
    }

    void Update()
    {
        // 常に最新HPを表示
        m_Slider.value = m_Parameta2.m_Hp;
    }
}