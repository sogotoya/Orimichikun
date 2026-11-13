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

    //しびれが終わる時間（雷が何回当たっても、最後に当たってから1秒で元に戻す）
    static float s_StanEndTime;


    private void Start()
    {
        FindReferences();
    }

    /// <summary>
    /// プレイヤーなどの参照を取得する（まだ取得できていないものだけ）
    /// 雷が出た瞬間に当たると Start より先に OnTriggerEnter2D が呼ばれるため、当たったときにも呼ぶ
    /// </summary>
    void FindReferences()
    {
        if (m_PCManager == null)
        {
            GameObject pcm = GameObject.Find("PlayerControlManager");
            if (pcm != null) m_PCManager = pcm.GetComponent<PlayerControlManager>();
        }
        if (m_Player == null) m_Player = GameObject.Find("プレイヤ-");
        if (m_SR == null && m_Player != null) m_SR = m_Player.GetComponent<SpriteRenderer>();
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

                FindReferences();
                //雷のオブジェクトはすぐ消えるので、消えないプレイヤー操作管理の上で動かす
                //（雷の上で動かすと途中で止まり、青いまま・攻撃できないままになっていた）
                MonoBehaviour runner = m_PCManager != null ? (MonoBehaviour)m_PCManager : this;
                runner.StartCoroutine(ThunderStan());
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
        //参照を手元に持っておく（雷が消えても使えるように）
        SpriteRenderer sr = m_SR;
        PlayerControlManager pcManager = m_PCManager;
        s_StanEndTime = Time.time + 1.0f;
        if (sr != null) sr.color = Color.blue;
        if (pcManager != null) pcManager.m_IsPSPlaying = true;
        //最後に当たってから1秒たつまで待つ
        while (Time.time < s_StanEndTime) yield return null;
        if (sr != null) sr.color = Color.white;
        if (pcManager != null) pcManager.m_IsPSPlaying = false;
        yield return null;
    }
}

