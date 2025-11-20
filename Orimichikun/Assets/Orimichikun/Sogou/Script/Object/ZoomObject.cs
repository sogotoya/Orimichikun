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

    [SerializeField]
    [Tooltip("ズームするかしないか判定フラグ")]
    public bool m_IsZoomFlag;

    //カメラの最初の座標を保存する変数
    Vector3 m_CameraPos;

    //カメラの元のFOVサイズ
    float m_CameraFOV = 0f;

    private void Start()
    {
        //初期値代入
        m_CameraPos = m_ZoomCamera.transform.position;
        m_CameraFOV = m_ZoomCamera.fieldOfView;
    }

    private void Update()
    {
        //ズームON
        if (m_IsZoomFlag)
        {
            //カメラをオブジェクトの方向へ少しづつ近づける
            Vector3 targetPos = m_ZoomObj.transform.position;
            //Z座標は固定(オブジェクトに埋もれる対策)
            targetPos.z = m_ZoomCamera.transform.position.z;
            m_ZoomCamera.transform.position = Vector3.Lerp(m_ZoomCamera.transform.position, targetPos, m_ZoomSpeed * Time.deltaTime);

            //ズームを徐々に小さくする
            m_ZoomCamera.fieldOfView = Mathf.Lerp(m_ZoomCamera.fieldOfView, m_TargetSize, m_ZoomSpeed * Time.deltaTime);
        }
        else//ズームOFF
        {
            //カメラをオブジェクトの方向へ少しづつ近づける
            Vector3 targetPos = m_CameraPos;
            //Z座標は固定(オブジェクトに埋もれる対策)
            targetPos.z = m_ZoomCamera.transform.position.z;
            m_ZoomCamera.transform.position = Vector3.Lerp(m_ZoomCamera.transform.position, targetPos, m_ZoomSpeed * Time.deltaTime);

            //ズームを徐々に小さくする
            m_ZoomCamera.fieldOfView = Mathf.Lerp(m_ZoomCamera.fieldOfView, m_CameraFOV, m_ZoomSpeed * Time.deltaTime);
        }
    }
}
