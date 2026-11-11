//一時停止機能
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePause : MonoBehaviour
{
    [SerializeField]
    GameObject m_PauseOnobj;
    [SerializeField]
    GameObject m_PauseOffobj;

    [Tooltip("一時停止中かどうか判定フラグ")]
    bool m_IsPause=false;

    private void Start()
    {
        m_PauseOnobj.SetActive(true);
        m_PauseOffobj.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!m_IsPause)
            {
                OnPause();
                m_IsPause=true;
            }else
            {
                OffPause();
                m_IsPause = false;
            }
        }
    }

    /// <summary>
    /// 一時停止開始
    /// </summary>
    public void OnPause()
    {
        Time.timeScale = 0;
        m_PauseOnobj.SetActive(false);
        m_PauseOffobj.SetActive(true);
    }


    /// <summary>
    /// 一時停止終了
    /// </summary>
    public void OffPause()
    {
        Time.timeScale = 1;
        m_PauseOnobj.SetActive(true);
        m_PauseOffobj.SetActive(false);
    }
}
