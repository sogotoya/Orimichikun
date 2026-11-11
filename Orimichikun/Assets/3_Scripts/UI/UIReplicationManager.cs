#region　前提事項
//---* Grid Layout Groupを使用したスクリプト *---

//---Grid Layout Group の導入方法---
//Canvas(親)にPanel(子)を追加
//Panel(親)に使用するImage(画像)(子)を１つだけ追加
//PanelにGrid Layout Groupコンポーネントを追加
//(Grid Layout Group のセルサイズや間隔は個人で調整)

//---このスクリプトの導入方法---
//空のオブジェクトを新しく作る
//このスクリプトをアタッチする
//それぞれの変数にインスペクター上でドラッグ&ドロップ

//m_ImageList現在使ってません
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIReplicationManager : MonoBehaviour
{
    [Header("合計複製数(-1した数字を記入)")]
    [Tooltip("合計複製数(-1した数字を記入)")]
    public int m_UIMax = 3;
    [Tooltip("現在の数")]
    private int m_Count;

    [SerializeField]
    [Header("使用するUI")]
    [Tooltip("使用するUI")]
    Image m_Image;

    [SerializeField]
    [Header("GridLayoutGroupが付いたPanel")]
    [Tooltip("GridLayoutGroupが付いたPanel")]
    Transform m_ImagePanel;

    [Tooltip("並べたUIの数")]
    List<Image> m_ImageList = new List<Image>();

    private void Start()
    {
        if (m_Image == null && m_ImagePanel == null)
        {
            Debug.Log("使用するUI、またはGridLayoutGroupが付いたPanel がnullです");
        }
            //今回の合計値代入
            m_Count = m_UIMax;

        //HPのUIを今回のHP分均等感覚で増やす
        for (int i = 0; i < m_UIMax; i++)
        {
            AddUI();
        }

    }

    /// <summary>
    /// 画像複製追加
    /// </summary>
    void AddUI()
    {
        //複製
        Image m_UI = Instantiate(m_Image, m_ImagePanel);
        //リストの追加
        m_ImageList.Add(m_UI);

    }

    /// <summary>
    /// 表示非表示変更
    /// </summary>
    void RemoveUI()
    {
        //要素の数を取得して端から表示切替
        int m_Count = m_ImageList.Count;
        m_ImagePanel.gameObject.SetActive(!m_ImageList[m_Count].gameObject.activeSelf);
    }

}
