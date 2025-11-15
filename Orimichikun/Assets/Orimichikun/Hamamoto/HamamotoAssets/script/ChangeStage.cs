using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeStage : MonoBehaviour
{
    [SerializeField] TriggerManager m_TM;
    private void Update()
    {
        if (m_TM.m_IsMoveTitle&& Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("ステージ移行可能（Eキー）");
            SceneManager.LoadScene("Stage");
        }
    }
}
