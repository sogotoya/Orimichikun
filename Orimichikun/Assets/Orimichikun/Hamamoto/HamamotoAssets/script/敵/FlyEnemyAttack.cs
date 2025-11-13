using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemyAttack : MonoBehaviour
{
    [Header("プレイヤー")]
    public GameObject m_Player;
    [Header("敵が放出する弾")]
    public GameObject m_EnemyBullet;
    [Header("攻撃する時間（クールタイム）")]
    public float m_AttackCoolingtime=0.5f;
    [Header("アニメーター")]
    public Animator m_Animator;
    //攻撃エリアに入ったかどうか
    private bool m_AttackTrigger = false;
    private bool m_HasAttackedOnce=false;
    private float m_Count;
    private FlySound m_Sound;
    private Parameta2D m_Parameta;
    private void Start()
    {
        //ないなら親からアニメーターを探す
        if (m_Animator==null)
        {
            m_Animator=GetComponentInParent<Animator>();
        }
        if (m_Parameta == null)
        {
            m_Parameta = GetComponentInParent<Parameta2D>();
        }
        //プレイヤーが入ってない場合Tagで登録
        if (m_Player==null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            if (obj!=null)
            {
                m_Player = obj;
            }
        }
        m_Sound = GetComponent<FlySound>();
    }
    private void Update()
    {
        if (m_Parameta.m_Hp <= 0)
        {
            m_AttackTrigger = false;
            return;
        }

        if (m_AttackTrigger)
        {
            m_Count += Time.deltaTime;

            if (m_Count >= m_AttackCoolingtime)
            {
                AttackMode();
                m_Count = 0f;
            }
        }

    }
    //エリアにプレイヤーが入ってきたら攻撃開始
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            m_AttackTrigger=true;
            if (!m_HasAttackedOnce) // 初回のみ
            {
                AttackMode();
                m_HasAttackedOnce = true;
                m_Count = 0f;
            }
        }
    }
    //エリアからプレイヤが出たら攻撃終わり
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (m_Parameta.m_Hp != 0)
            {
                m_AttackTrigger = true;
              
                m_Count = 0f;
            }
        }
    }
    void AttackMode()
    {
        //攻撃アニメーション
        m_Animator.SetTrigger("Attack");
        //攻撃SE
        m_Sound.m_Source.PlayOneShot(m_Sound.m_FlyAttack);
        // 弾生成
        GameObject bullet = Instantiate(m_EnemyBullet, transform.position, Quaternion.identity);

        // プレイヤー方向を計算
        Vector2 direction = (m_Player.transform.position - transform.position);

        // 弾に方向を渡す
        bullet.GetComponent<FlyBullet>().SetDirection(direction);
    }
}
