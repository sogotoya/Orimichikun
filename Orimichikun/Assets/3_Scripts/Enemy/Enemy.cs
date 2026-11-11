using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("ˆÚ“®‘¬“x")]
    public float m_EnemySpeed;
    private Rigidbody2D m_Rigidbody;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        Move();
    }
    private void Move()
    {
        m_Rigidbody.velocity = new Vector2(Vector2.left.x * m_EnemySpeed, m_Rigidbody.velocity.y); 
    }
}
