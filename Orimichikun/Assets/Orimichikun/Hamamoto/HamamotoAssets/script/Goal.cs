using UnityEngine;

public class Goal : MonoBehaviour
{
    public AudioSource m_Bgm2;
    public AudioSource m_Bgm3;

    public GameObject m_Player;
    public GameObject  m_image;
    private bool m_IsActive = false;

    private void Start()
    {
        // 最初は非表示
        gameObject.SetActive(false);
        m_image.SetActive(false);
    }

    // Bossから呼ばれる
    public void OnBossDie()
    {
        if (m_IsActive) return; // 二重呼び出し防止
        m_IsActive = true;

        // ゴールを表示
        gameObject.SetActive(true);
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //プレイヤーがエリアに入ったら
        if (collision.gameObject == m_Player)
        {
            m_Bgm2.Stop();
            m_image.SetActive(true);
            m_Bgm3.Play();
        }
    }
}