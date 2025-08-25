using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Camera : MonoBehaviour
{
    [Header("追跡するターゲット")]
    public Transform m_Player;
    [Header("カメラとのオフセット")]
    public Vector3 offset = new Vector3(0f, 1f, -10f);
    [Header("追従のなめらかさ（大きいほどゆっくり追従）")]
    [Range(0f, 1f)]
    public float m_smoothSpeed = 0.1f;
    [Header("カメラ制限")]
    public float minX, maxX;
    public float minY, maxY;

    void LateUpdate()
    {
        if (m_Player == null) return;

        // 目標位置（プレイヤーの位置 + オフセット）
        Vector3 desiredPosition = m_Player.position + offset;

        // 範囲内に制限
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);

        // 現在位置から目標位置へスムーズに移動
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, m_smoothSpeed);

        transform.position = smoothedPosition;
    }
}
