//ボスが第二形態へ変化したときの色変更処理
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCollarChange : MonoBehaviour
{
    public SpriteRenderer m_SR;

    [Header("第二形態の色")]
    [SerializeField]
    Color m_2ndColor=new Color(1f, 0.6f, 0.6f, 1f); // 少し赤みがかった色
    //最初の色保存先
    Color m_1stColor;

    private void Start()
    {
        if (m_SR == null)
        {
            Debug.LogError("アタッチしてください", this);
        }
        //最初の色取得
        //m_1stColor=m_SR.color;

        //StartCoroutine(FadeToPhase2());
    }

    /// <summary>
    /// 色変化開始処理
    /// </summary>
    public void CollarChangeStart()
    {

        StartCoroutine(FadeToPhase2());
    }

    /// <summary>
    /// だんだんと色が変化していく
    /// </summary>
    /// <returns></returns>
    public IEnumerator FadeToPhase2()
    {
        Color end = m_2ndColor;
        float timer = 0f;

        while(timer<3f)
        {
            timer += Time.deltaTime;
            m_SR.color = Color.Lerp(m_1stColor, end, timer);
            yield return null;
        }
    }

    //元の色に戻す
    public void ResetColor()
    {
        m_SR.color = m_1stColor;
    }
}
