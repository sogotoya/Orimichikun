using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHaikei : MonoBehaviour
{
    [SerializeField]
    TriggerManager m_TM;
    private void Start()
    {
        if (m_TM == null)
        {
            Debug.LogError($"{nameof(m_TM)}がアタッチされていません");
        }
    }
    //範囲内にいる場合
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            m_TM.m_IsMoveTitle = true;
            Debug.Log("背景動ている最中");
        }
    }

}
