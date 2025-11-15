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
    public Goal m_Goal;

   
    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        //ChangeSenseHPがあれば
        if (m_Team == "Player"&&ChangeSenseHP.m_Instance != null)
        {
            m_Hp = ChangeSenseHP.m_Instance.PlayerHP;

            // もし保存された HP が Max を超えていたら補正
            if (m_Hp > m_MaxHp) m_Hp = m_MaxHp;
        }
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

        // プレイヤーだけ HP を保存
        if (m_Team == "Player" && ChangeSenseHP.m_Instance != null)
        {
            ChangeSenseHP.m_Instance.PlayerHP = m_Hp;
        }

        // 無敵時間をセット
        m_InvincibleTimer = m_InvincibleTime;

        if (m_Animator)
            m_Animator.SetTrigger("Damage");

        if (m_Hp <= 0)
        {
            m_Hp = 0;

            // 死亡時もプレイヤーだけ HP を保存
            if (m_Team == "Player" && ChangeSenseHP.m_Instance != null)
            {
                ChangeSenseHP.m_Instance.PlayerHP = m_Hp;
            }

            Die();
        }
    }

    private void Die()
    {
        if (m_Animator)
        {
            m_Animator.SetTrigger("Die");
           
        }


        if (m_Goal != null)
        {
            m_Goal.OnBossDie();
        }



        // 敵だけ Destroy
        if (m_Team != "Player")
        {
            Destroy(gameObject, 2f); 
        }
        else
        {
           
        }
    }

}
