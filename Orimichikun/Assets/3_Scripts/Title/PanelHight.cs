using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelHight : MonoBehaviour
{
    [SerializeField]
    GameObject m_Panel;

    private void Start()
    {
        if (m_Panel == null) return;
        m_Panel.SetActive(false);
    }
    //ˆÃ“]•\Ž¦
    public void Hight()
    {
        m_Panel.SetActive(true);
    }
}
