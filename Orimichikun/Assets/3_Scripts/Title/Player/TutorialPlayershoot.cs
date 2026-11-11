using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayershoot : MonoBehaviour
{

    // 弾プレハブ
    public GameObject m_bulletPrefab;
    // 発射位置（空オブジェクトを子として置く）
    public Transform m_firePoint;
    public AudioClip m_Shoot;
    private AudioSource m_Source;
    private TutorialPlayerController m_Player;
    private Animator m_Animator;
    private void Start()
    {
        m_Player = GetComponent<TutorialPlayerController>();
        m_Animator = GetComponent<Animator>();
        //  AudioSourceを取得
        m_Source = GetComponent<AudioSource>();
        if (m_Source == null)
        {
            // なければ自動で追加（任意）
            m_Source = gameObject.AddComponent<AudioSource>();
        }

    }

    void Update()
    {
        // 左クリックで発射
        if (Input.GetMouseButtonDown(0))
        {

            m_Animator.SetTrigger("Attack");
            GameObject bullet = Instantiate(m_bulletPrefab, m_firePoint.position, m_firePoint.rotation);
            // 向きをセット
            int direction = m_Player.IsFacingRight() ? 1 : -1;
            bullet.GetComponent<Bullet>().SetDirection(direction);
            m_Source.PlayOneShot(m_Shoot);

        }
    }

}
