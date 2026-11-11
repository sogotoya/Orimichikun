using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlySound : MonoBehaviour
{
    [Header("çUåÇÇ∑ÇÈSE")]
    public AudioClip m_FlyAttack;
    public AudioSource m_Source;
    private void Start()
    {
        m_Source = GetComponent<AudioSource>();
    }

}
