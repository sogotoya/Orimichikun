//雷限定の効果処理
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder_Damage : MonoBehaviour
{
    // パラメータ（自分のParameta2D）
    public Parameta2D m_Parameta;

    PlayerControlManager m_PCManager;

    //プレイヤーオブジェクト
    GameObject m_Player;
    SpriteRenderer m_SR;

    //重複対策
    [SerializeField]
    GameObject[] m_Thunder;

    Coroutine[] m_ThunderRoutine;


    private void Start()
    {
        m_PCManager = GameObject.Find("PlayerControlManager").GetComponent<PlayerControlManager>();
        m_Player = GameObject.Find("プレイヤ-");
        m_SR = m_Player.GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!m_Parameta) return;

        Parameta2D otherParam = other.gameObject.GetComponent<Parameta2D>();
        if (otherParam)
        {
            // チームが違う場合のみダメージ
            if (otherParam.m_Team != m_Parameta.m_Team)
            {

                StartCoroutine(ThunderStan());
            }

        }
    }

    /// <summary>
    /// 一定期間弾打てなくなる
    /// </summary>
    /// <returns></returns>
    IEnumerator ThunderStan()
    {
        if (m_ThunderRoutine != null)
        {
            foreach (Coroutine c in m_ThunderRoutine)
            StopCoroutine(c);
        }
        m_SR.color = Color.blue;
        m_PCManager.m_IsPSPlaying = true;
        yield return new WaitForSeconds(1.0f);
        m_SR.color = Color.white;
        m_PCManager.m_IsPSPlaying=false;
        yield return null;
    }
}

