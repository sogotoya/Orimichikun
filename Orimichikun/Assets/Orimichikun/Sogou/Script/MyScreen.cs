using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyScreen : MonoBehaviour
{
    int m_W;
    int m_H;
    private void Start()
    {
        m_W = Screen.width;
        m_H = Screen.height;
    }
}
