// 仮想カメラの位置を指定範囲内に制限する Cinemachine 拡張
// （旧 PlayerCamera の minX / maxX / minY / maxY と同じ役割）
using Cinemachine;
using UnityEngine;

[AddComponentMenu("")] // 仮想カメラの Extensions から追加する
public class CinemachineClampPosition : CinemachineExtension
{
    public float m_MinX = -10f;
    public float m_MaxX = 1000f;
    public float m_MinY = 1.5f;
    public float m_MaxY = 3.3f;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage,
        ref CameraState state, float deltaTime)
    {
        // Body（追従）の後で位置を制限する。揺れ（Noise / Impulse）はこの後に加わる
        if (stage != CinemachineCore.Stage.Body) return;

        Vector3 pos = state.RawPosition;
        pos.x = Mathf.Clamp(pos.x, m_MinX, m_MaxX);
        pos.y = Mathf.Clamp(pos.y, m_MinY, m_MaxY);
        state.RawPosition = pos;
    }
}
