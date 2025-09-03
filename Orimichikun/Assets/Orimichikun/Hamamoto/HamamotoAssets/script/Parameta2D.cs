using UnityEngine;
using UnityEngine.UI;

public class Parameta2D : MonoBehaviour
{
    [Header("チーム名 (例: Player / Enemy)")]
    public string m_Team = "Enemy";

    [Header("HP")]
    public int m_Hp = 100;
    public int m_MaxHp = 100;

    [Header("無敵時間 (秒)")]
    public float m_InvincibleTime = 1f;

    private float m_InvincibleTimer = 0f;

    private Animator m_Animator;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (m_InvincibleTimer > 0)
        {
            m_InvincibleTimer -= Time.deltaTime;
        }
    }

    public void TakeDamage(int damage)
    {
        // 無敵中はダメージ無効
        if (m_InvincibleTimer > 0) return;
        // すでに死んでいる
        if (m_Hp <= 0) return; 

        m_Hp -= damage;
       

        // 無敵時間をセット
        m_InvincibleTimer = m_InvincibleTime;
        // ダメージアニメーションを再生
        if (m_Animator)
        {
            m_Animator.SetTrigger("Damage");
        }
        if (m_Hp <= 0)
        {
            m_Hp = 0;
            Die();
        }
    }

    private void Die()
    {
        if (m_Animator)
        {
            m_Animator.SetTrigger("Die");
        }
        Destroy(gameObject, 2f);
    }

}
