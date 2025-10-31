using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoubleJump : MonoBehaviour
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
            m_TM.m_IsDoubleJump = true;
            Debug.Log("Player範囲内中(ダブルジャンプ)");
        }
    }
    //範囲外になった場合
    private void OnTriggerExit2D(Collider2D other)
    {
        m_TM.m_IsDoubleJump = false;
    }
}
