using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommentManager : MonoBehaviour
{
    [SerializeField]
    [Header("テキストの流れるスピード")]
    float m_TextSpeed = 0.1f;

    [SerializeField]
    [Header("テキストの表示させるUI")]
    Text m_TextUI;

    [SerializeField]
    [Header("会話データ(行ごと)")]
    string[] m_Scenario;
    [Tooltip("セリフの番号")]
    int m_CurrentIndex = 0;
    [Tooltip("文字を流している最中かどうか")]
    bool m_IsPlaying = false;

    private void Start()
    {
        StartScenario();
    }
    private void Update()
    {
        //クリックされたら次のテキストへ
        if (Input.GetMouseButtonDown(0))
        {
            OnClick();
        }
    }

    /// <summary>
    /// クリックされた（スキップされた処理）
    /// </summary>
    void OnClick()
    {
        //テキスト表示中なら
        if (m_IsPlaying)
        {
            //スキップされる(クリックされた)ので、今から表示中じゃなくなる。
            m_IsPlaying = false;
            StopAllCoroutines();
            //テキスト全表示
            m_TextUI.text = m_Scenario[m_CurrentIndex];
            return;
        }

        //次のセリフへ
        m_CurrentIndex++;
        if (m_CurrentIndex < m_Scenario.Length)
        {
            StartCoroutine(DrawText(m_Scenario[m_CurrentIndex]));
        }
        else
        {
            Debug.Log("会話終了しました。");
            m_TextUI.enabled = false;
        }
    }

    /// <summary>
    /// テキスト呼び出し関数
    /// </summary>
    public void StartScenario()
    {
        m_TextUI.enabled = true;
        //0番目のセリフからスタート
        m_CurrentIndex = 0;
        //表示開始
        StartCoroutine(DrawText(m_Scenario[m_CurrentIndex]));
    }

    /// <summary>
    /// テキスト１文字づつ表示処理
    /// </summary>
    /// <returns></returns>
    IEnumerator DrawText(string sentence)
    {
        //テキスト表示中なのでフラグON
        m_IsPlaying = true;
        //テキスト初期化
        m_TextUI.text = "";


        foreach (char text in sentence)
        {
            //スキップされた瞬間終了
            if (!m_IsPlaying) yield break;
            m_TextUI.text += text;
            yield return new WaitForSeconds(m_TextSpeed);
        }
        //テキスト表示中なのでフラグOFF
        m_IsPlaying = false;
    }
}
