//ボス戦にてカメラをプレイヤーカメラ、ステージカメラを変えれるスクリプト
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //プレイヤーカメラ
    [SerializeField]
    Camera m_PCamera;

    //ステージカメラ
    [SerializeField]
    Camera m_SCamera;

    private void Start()
    {
        if (m_PCamera == null || m_SCamera == null)
        {
            Debug.LogError("アタッチしてください", this);
        }
        m_SCamera.enabled = false;
    }

    /// <summary>
    /// プレイヤーカメラに変更
    /// </summary>
    public void PlayerCameraChange()
    {
        m_SCamera.enabled = false;
        m_PCamera.enabled = true;
    }

    /// <summary>
    /// ステージカメラに変更
    /// </summary>
    public void StageCameraChange()
    {
        m_PCamera.enabled = false;
        m_SCamera.enabled=true;
    }
}
