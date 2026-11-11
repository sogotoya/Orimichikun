using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP_Player : MonoBehaviour
{
    public int m_MaxHP = 100;

    //デリゲートの宣言
    //増減した値を取得するために、変更前の値を引数としています
    public delegate void ValueChangedHandler(int preValue);
    //ここに対象の値が変化したときの処理を追加していく
    public event ValueChangedHandler HpChanged;

    public int m_CurrentHP
    {
        get { return m_currentHp; }
        set
        {
            if (m_currentHp != value)
            {
                var m_pre = m_currentHp;
                m_currentHp = value;
                //ここで登録したデリゲートが呼ばれる
                HpChanged(m_pre);
            }
        }
    }
    int m_currentHp = 100;

    public float m_PerHP
    {
        get
        {
            float value = (float)m_CurrentHP / (float)m_MaxHP;
            return Mathf.Clamp(value, 0, 1);
        }
        private set { }
    }
}
