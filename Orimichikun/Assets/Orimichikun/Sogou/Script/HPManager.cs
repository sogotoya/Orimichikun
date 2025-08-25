using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPManager : MonoBehaviour
{
    [Header("プレイヤーのベースとなるHP")]
    [Tooltip("ベースとなるHP")]
    public int m_HPMax = 3;
    [Tooltip("現在のHP")]
    private int m_HP;

    [SerializeField]
    [Header("HPのUI")]
    Image m_HPImage;

    [SerializeField]
    [Header("UIの間隔")]
    float m_Spacing;

    [Tooltip("並べたUIの数")]
    List<Image> m_HPImageList = new List<Image>();

    [Header("並びの開始位置")]
    [Tooltip("並びの開始位置")]
    public Vector2 m_StartPos = Vector3.zero;


    private void Start()
    {
        //今回のHP代入
        m_HP = m_HPMax;

        //HPのUIを今回のHP分均等感覚で増やす
        for (int i = 0; i < m_HPMax; i++)
        {
            AddUI();
        }

    }

    /// <summary>
    /// 追加
    /// </summary>
    void AddUI()
    {
        //最初の座標、回転無し
        Image m_UI = Instantiate(m_HPImage, m_StartPos, Quaternion.identity);
        //リストに追加
        m_HPImageList.Add(m_UI);

        Rearrange();
    }

    /// <summary>
    /// 再配置
    /// </summary>
    void Rearrange()
    {
        //
        for (int i = 0; i < m_HPImageList.Count; i++)
        {
            // X方向に等間隔
            Vector2 m_Pos = m_StartPos + new Vector2(i * m_Spacing, 0);
            //それぞれ均等に配置
            m_HPImageList[i].transform.position = m_Pos;
        }
    }
}
