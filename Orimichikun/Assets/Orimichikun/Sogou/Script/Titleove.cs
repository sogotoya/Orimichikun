using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Titleove : MonoBehaviour
{
    float m_MoveTime;
    [SerializeField]
    float m_StopTime;
    void Update()
    {
        if (m_MoveTime < m_StopTime)
        {
            transform.Translate(Vector2.down * 100 * Time.deltaTime);
            m_MoveTime += Time.deltaTime;
        }
    }
}
