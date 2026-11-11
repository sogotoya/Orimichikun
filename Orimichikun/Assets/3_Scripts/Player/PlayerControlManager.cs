//プレイヤーの操作on、offできるスクリプト
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlManager : MonoBehaviour
{
    [SerializeField]
    PlayScript m_SP;
    [SerializeField]
    playershoot m_PS;

    [Header("操作可能判断フラグ")]
    public bool m_IsAllPlaying = false;
    public bool m_IsSPPlaying = false;
    public bool m_IsPSPlaying = false;

    private void Start()
    {
        if (m_SP == null || m_PS == null)
        {
            Debug.LogError("アタッチして下さい", this);
        }
        m_SP.enabled = false;
        m_PS.enabled = false;
    }

    private void Update()
    {
        //m_IsPlayingがtrueだったらON、falseならOFF
        if (m_IsAllPlaying&&!m_IsSPPlaying&&!m_IsPSPlaying)
        {
            m_SP.enabled = true;
            m_PS.enabled = true;
        }
        else if (m_IsAllPlaying&&m_IsSPPlaying)
        {
            m_SP.enabled=false;
        }
        else if (m_IsAllPlaying&&m_IsPSPlaying)
        {
            m_PS.enabled = false;
        }
        else if(!m_IsAllPlaying)
        {
            m_SP.enabled = false;
            m_PS.enabled = false;
        }
    }
}
