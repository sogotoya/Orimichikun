//アタッチしたオブジェクトの色を徐々に変化させる処理
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollarChange : MonoBehaviour
{
    [Header("色を変えたい背景のオブジェクト")]
    [SerializeField]
    GameObject[] m_HaikeiObj;

    [Header("変えたい色")]
    [SerializeField]
    Color m_Color;

    [Header("変化に必要な時間")]
    [SerializeField]
    float m_CollarTimer=1.5f;
    //private void Start()
    //{
    //    ObjectCollarChangeStart();
    //}
    public  void ObjectCollarChangeStart()
    {
        StartCoroutine(ChangeObjCollar());
    }

    IEnumerator ChangeObjCollar()
    {
        float timer = 0;

        //オブジェクトの入れる箱生成
        Color[] startcolors = new Color[m_HaikeiObj.Length];
        //各オブジェクトの元の色を保存
        for (int i = 0; i < m_HaikeiObj.Length; i++)
        {
            startcolors[i] = m_HaikeiObj[i].GetComponent<SpriteRenderer>().color;
        }


        while (timer < m_CollarTimer)
        {
            timer += Time.deltaTime;
            float t = timer / m_CollarTimer;
            //色変化開始(徐々に)
            for (int i = 0; i < m_HaikeiObj.Length; i++)
            {
                SpriteRenderer sr = m_HaikeiObj[i].GetComponent<SpriteRenderer>();
                sr.color = Color.Lerp(startcolors[i], m_Color, t);
            }
            //無限ループ対策 1フレイム待つ
            yield return null;
        }

    }
}
