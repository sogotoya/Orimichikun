using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeStage : MonoBehaviour
{
    [SerializeField] TriggerManager m_TM;
    private void Update()
    {
        if(m_TM.m_IsTitle)
        if (( Input.GetKeyDown(KeyCode.E)|| Input.GetKeyDown("joystick button 4") || Input.GetKeyDown("joystick button 5")&&m_TM.m_IsMoveTitle ))
        {
            Debug.Log("ステージ移行可能（Eキー）");
            SceneManager.LoadScene("Stage");
        }
    }
}
