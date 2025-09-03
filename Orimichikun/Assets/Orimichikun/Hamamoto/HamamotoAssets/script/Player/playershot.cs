using UnityEngine;

public class playershoot : MonoBehaviour
{
    // 弾プレハブ
    public GameObject m_bulletPrefab;
    // 発射位置（空オブジェクトを子として置く）
    public Transform m_firePoint;
    private PlayScript m_Player;
    private Animator m_Animator;
    private void Start()
    {
        m_Player = GetComponent<PlayScript>();
        m_Animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 左クリックで発射
        if (Input.GetMouseButtonDown(0)) 
        {
            m_Animator.SetTrigger("Attack");
            GameObject bullet=Instantiate(m_bulletPrefab, m_firePoint.position, m_firePoint.rotation);
            // 向きをセット
            int direction = m_Player.IsFacingRight() ? 1 : -1;
            bullet.GetComponent<Bullet>().SetDirection(direction);

        }
    }

}
