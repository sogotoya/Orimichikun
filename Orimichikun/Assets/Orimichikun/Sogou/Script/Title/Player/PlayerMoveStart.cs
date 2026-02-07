using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveStart : MonoBehaviour
{
    [SerializeField]
    [Header("Bossをアタッチ")]
    GameObject m_Boss;

    [Header("Playerについているスクリプトをアタッチ")]
    [SerializeField]
    PlayScript m_PalyerController;
    [SerializeField]
    playershoot m_TutorialPlayershoot;
    [SerializeField]
    PlayerJump m_PlayerJump;
    //1回のみの起動用
    bool m_ChangeFlag=false;
    private void Awake()
    {
        m_PalyerController.enabled = false;
        m_TutorialPlayershoot.enabled = false;
    }
    private void Update()
    {
        if (m_Boss == null&&!m_ChangeFlag)
        {
            m_PlayerJump.enabled = false;
            m_PalyerController.enabled = true;
            m_TutorialPlayershoot.enabled=true;
            m_ChangeFlag = true;
        }
    }
}
