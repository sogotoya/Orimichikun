//オブジェクトにカメラをズームさせるスクリプト
//ズーム用の仮想カメラの優先度を上げて切り替える（切り替えの速さは CinemachineBrain の Blend 設定）
using Cinemachine;
using UnityEngine;

public class ZoomObject : MonoBehaviour
{
    //CameraManager の優先度（20）より高くしてズームを優先する
    const int k_ZoomPriority = 30;
    const int k_IdlePriority = 0;

    [Header("ズームしたいオブジェクト")]
    public GameObject m_ZoomObj;

    [SerializeField]
    [Header("ズーム用の仮想カメラ")]
    CinemachineVirtualCamera m_ZoomVcam;

    [SerializeField]
    [Header("ズーム時の FOV")]
    float m_TargetSize = 20f;

    [Tooltip("ズームするかしないかを決めるフラグ")]
    public bool m_IsZoomFlag;

    private void Start()
    {
        if (m_ZoomVcam == null)
        {
            Debug.LogError("アタッチしてください", this);
            return;
        }
        m_ZoomVcam.m_Lens.FieldOfView = m_TargetSize;
        m_ZoomVcam.Priority = k_IdlePriority;
    }

    private void Update()
    {
        if (m_ZoomVcam == null) return;

        bool zoom = m_IsZoomFlag && m_ZoomObj != null;
        if (zoom) m_ZoomVcam.Follow = m_ZoomObj.transform;
        m_ZoomVcam.Priority = zoom ? k_ZoomPriority : k_IdlePriority;
    }
}
