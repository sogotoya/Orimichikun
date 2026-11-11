using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerJump : MonoBehaviour
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
            m_TM.m_IsJump = true;
            Debug.Log("Player範囲内中(ジャンプ)");
        }
    }
    //範囲外になった場合
    private void OnTriggerExit2D(Collider2D other)
    {
        m_TM.m_IsJump = false;
    }
}
