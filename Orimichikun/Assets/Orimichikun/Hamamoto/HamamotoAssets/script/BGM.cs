using StateMachineAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public AITester_StateMachine m_test;
    private AudioSource bgm;
    public GameObject m_pl;

    void Start()
    {
        // AudioSource ‚ğæ“¾‚µ‚Ä•Ï”‚É•Û
        bgm = GameObject.Find("BossBGM").GetComponent<AudioSource>();
        bgm.Play();
    }


}

