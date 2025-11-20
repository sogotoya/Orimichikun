using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class ZoomObject : MonoBehaviour
{
    [SerializeField]
    [Header("ズームしたいオブジェクト")]
    GameObject m_ZoomObj;

    [SerializeField]
    [Header("ズームするカメラ")]
    Camera m_ZoomCamera;

    [SerializeField]
    [Header("ズームスピード")]
    float m_ZoomSpeed = 0.1f;

    [SerializeField]
    [Header("目標ズームサイズ")]
    float m_TargetSize = 3f;
    private void Update()
    {
        //カメラをオブジェクトの方向へ少しづつ近づける
        Vector3 targetPos = m_ZoomObj.transform.position;
        //Z座標は固定(オブジェクトに埋もれる対策)
        targetPos.z = m_ZoomCamera.transform.position.z;
        m_ZoomCamera.transform.position = Vector3.Lerp(m_ZoomCamera.transform.position, targetPos, m_ZoomSpeed * Time.deltaTime);

        //ズームを徐々に小さくする
        m_ZoomCamera.fieldOfView = Mathf.Lerp(m_ZoomCamera.fieldOfView, m_TargetSize, m_ZoomSpeed * Time.deltaTime);
    }
}
