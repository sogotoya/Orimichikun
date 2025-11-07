using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public enum State
    {
        Patrol,
        Attack,
        Damage,
    }
  
    [Header("プレイヤー")]
    public GameObject m_Player;
    [Header("アニメーター")]
    public Animator m_Animator;
    [Header("敵の速さ")]
    public float m_EnemySpeed;
    [Header("地面判定用レイヤー")]
    public LayerMask m_Layer;
    [Header("地面チェック位置")]
    public Transform m_Ground;
    private State CurrentState = State.Patrol;
    //右に行くか左に行くか？
    private int m_Direction = 1;
    private Parameta2D m_Parameta;
    private Damege2D m_Damege;
    //ストップしているかどうか
    private bool m_MoveStop=false;
    

    private void Start()
    {
        if (m_Player==null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            if (obj != null)
            {
               m_Player = obj;
            }
        }
        m_Animator=GetComponent<Animator>();
        m_Parameta=GetComponent<Parameta2D>();
        m_Damege=GetComponent<Damege2D>();
    }

    private void Update()
    {
        if (m_Parameta.m_Hp <= 0)
        {
            Destroy(GetComponent<BoxCollider2D>());
            Destroy(GetComponent<Rigidbody2D>());
            return;
            // 死んでたら何もしない
        }

        switch (CurrentState)
        {
            case State.Patrol: ModePatrol(); break;

        }
    }


    void ModePatrol()
    {
        //プレイヤーがあたっていなければ
        if (!m_MoveStop)
        {
            //移動処理
            transform.Translate(Vector2.right * m_Direction * m_EnemySpeed * Time.deltaTime);
        }
        else
        {
            Stop();
        }
        
        RaycastHit2D hit = Physics2D.Raycast(m_Ground.position, Vector2.down, 0.5f, m_Layer);
        // 地面がなかったら方向転換
        if (hit.collider == null)
        {
            TurnAround();
        }
    }
    private void ModeDie()
    {
        if (m_Animator)
        {
            m_Animator.SetTrigger("Die");
            
        }


   
    }

    void TurnAround()
    {
        //行く方向を反転
        m_Direction *= -1;
        // 見た目も反転
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * m_Direction;
        transform.localScale = scale;
    }

    private void Stop()
    {
        m_MoveStop = true;
    }
    //プレイヤーが当たったらストップ
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") ){
            m_MoveStop=true;
        }
    }
    //プレイヤーがいなければ動く
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            m_MoveStop = false;
        }
    }
}
