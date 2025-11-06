using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_B_Move : MonoBehaviour
{
    [Header("行動するまで時間の最小値と最大値")]
    /// <summary>
    /// 行動するまで時間の最小値と最大値
    /// </summary>
    public int m_Min, m_Max;

    /// <summary>
    ///行動するまでの時間 
    /// </summary>
    int m_Count;

    /// <summary>
    /// 時間を加算する変数
    /// </summary>
    float m_MoveTime;


    [Header("行動の最小値と最大値")]
    /// <summary>
    /// 行動の最小値と最大値
    /// </summary>
    public int m_MoveMin, m_MoveMax;

    Vector3 m_Position;

    /// <summary>
    /// 移動の開始位置
    /// </summary>
    Vector3 m_StartPosition;
    /// <summary>
    /// 移動の目標位置
    /// </summary>
    Vector3 m_EndPosition;
    /// <summary>
    /// 移動にかける時間
    /// </summary>
    float m_MoveDuration;

    void Start()
    {
        m_Count = Random.Range(m_Min, m_Max);
        m_Position = transform.position;
        m_StartPosition = transform.position;
    }


    void Update()
    {
        if ((int)m_MoveTime > m_Count)
        {
            //左右移動
            transform.position = new Vector3(Mathf.Sin(Time.time) * Random.Range(m_MoveMin, m_MoveMax) + m_Position.x, m_Position.y, m_Position.z);

            //数値のリセット
            m_MoveTime = 0;
            m_Count = Random.Range(m_Min, m_Max);
        }
        else
        {
            //毎秒加算
            m_MoveTime += 1.0f * Time.deltaTime;
        }
        /*if(m_MoveTime<m_MoveDuration)
        {
            m_MoveTime = Time.deltaTime;

            //現在どれだけ移動したか0.0から1.0の割合で示す
            float t =m_MoveTime/m_MoveDuration;

            transform.position = Vector3.Lerp(m_StartPosition,m_EndPosition, t);
        }
        else
        {
            //新しい移動先を決定
            m_StartPosition = transform.position;
            m_EndPosition = new Vector3(Mathf.Sin(Time.time) * Random.Range(m_MoveMin, m_MoveMax) + m_Position.x,m_Position.y,m_Position.z);

            //リセット
            m_MoveDuration=Random.Range(m_Min,m_Max);
            m_MoveTime = 0;
        }*/
    }
}
