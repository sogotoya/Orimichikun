//ダメージ処理とHPを連動させるスクリプト
using StateMachineAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intermediary : MonoBehaviour
{

    [SerializeField] GameObject m_Boss;
    [SerializeField] AITester_StateMachine m_AISM;

    [SerializeField, Header("ヒット時の色")]
    Color m_Color;
    //元の色を保持する変数
    Color m_DefaultColor;

    private void Start()
    {
        //ボスの元の色をスタート時に保存しておく
        m_DefaultColor = m_Boss.GetComponent<SpriteRenderer>().color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            m_AISM.m_HP--;
            DamageChange();
            Destroy(other.gameObject);
        }
    }

    void DamageChange()
    {
        StartCoroutine(DamageChangeColor());
    }

    IEnumerator DamageChangeColor()
    {
        //色変更
        m_Boss.GetComponent<SpriteRenderer>().color = m_Color;
        yield return new WaitForSeconds(0.5f);

        //元の色に戻す
        m_Boss.GetComponent<SpriteRenderer>().color = m_DefaultColor;
    }
}
