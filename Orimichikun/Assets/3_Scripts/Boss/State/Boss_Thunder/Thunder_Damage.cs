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
        if (m_SR != null) m_SR.color = Color.blue;
        if (m_PCManager != null) m_PCManager.m_IsPSPlaying = true;
        yield return new WaitForSeconds(1.0f);
        if (m_SR != null) m_SR.color = Color.white;
        if (m_PCManager != null) m_PCManager.m_IsPSPlaying = false;
        yield return null;
    }
}

