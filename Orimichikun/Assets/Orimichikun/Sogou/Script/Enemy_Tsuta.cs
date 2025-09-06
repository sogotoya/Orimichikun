using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Enemy_Tsuta : MonoBehaviour
{
    [Tooltip("Tsutaのコライダー")]
    BoxCollider2D m_BoxCollider2D;
    [Tooltip("コライダーの最長(Y軸)")]
    float m_MaxCollider;

    [SerializeField]
    [Tooltip("変化が始まる指定時間")]
    float m_ChangeStartTime;
    [SerializeField]
    [Tooltip("変化が終わって戻す指定時間")]
    float m_ChangeFinishTime;
    [Tooltip("経過時間")]
    float m_Time;
    private void Start()
    {
        m_BoxCollider2D = this.gameObject.GetComponent<BoxCollider2D>();
        if (m_BoxCollider2D == null)
        {
            m_BoxCollider2D = this.gameObject.AddComponent<BoxCollider2D>();
            Debug.Log("nullなのでコライダー追加");
        }
        m_MaxCollider = m_BoxCollider2D.size.y;
        m_BoxCollider2D.size = new Vector2(m_BoxCollider2D.size.x, 0);
        m_Time = 0f;
    }

    private void Update()
    {
        m_Time += Time.deltaTime;
        AttackCount();
    }

    void AttackCount()
    {


        if (m_Time == m_ChangeStartTime)
        {

        }
        else if (m_Time > m_ChangeFinishTime)
        {

        }
    }
}
