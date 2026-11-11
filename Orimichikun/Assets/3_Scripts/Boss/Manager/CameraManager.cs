//ボス戦にてカメラをプレイヤーカメラ、ステージカメラに変えるスクリプト
//Cinemachine の仮想カメラの優先度（Priority）を切り替えて行う
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //有効にする側の優先度（ZoomObject のズームはこれより高い）
    const int k_ActivePriority = 20;
    const int k_InactivePriority = 0;

    //プレイヤーを追う仮想カメラ
    [SerializeField]
    CinemachineVirtualCameraBase m_PlayerVcam;

    //ステージ全体を映す仮想カメラ
    [SerializeField]
    CinemachineVirtualCameraBase m_StageVcam;

    private void Start()
    {
        if (m_PlayerVcam == null || m_StageVcam == null)
        {
            Debug.LogError("アタッチしてください", this);
            return;
        }
        PlayerCameraChange();
    }

    /// <summary>
    /// プレイヤーカメラに変更
    /// </summary>
    public void PlayerCameraChange()
    {
        m_PlayerVcam.Priority = k_ActivePriority;
        m_StageVcam.Priority = k_InactivePriority;
    }

    /// <summary>
    /// ステージカメラに変更
    /// </summary>
    public void StageCameraChange()
    {
        m_PlayerVcam.Priority = k_InactivePriority;
        m_StageVcam.Priority = k_ActivePriority;
    }
}
