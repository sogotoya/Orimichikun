using StateMachineAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Set : MonoBehaviour
{
    public GameObject m_coin;
    public AITester_StateMachine m_test;
    public bool m_testDone=false;
    private void Start()
    {
        m_coin.SetActive(false);
    }

    private void Update()
    {


        // Boss ‚ªİ’è‚³‚ê‚Ä‚¢‚Ä HP ‚ª 0 ˆÈ‰º‚È‚çƒRƒCƒ“o‚·
        if (m_test != null && m_test.m_HP <= 0)
        {
            m_testDone = true;
            m_coin.SetActive(true);
        }
    }
}

