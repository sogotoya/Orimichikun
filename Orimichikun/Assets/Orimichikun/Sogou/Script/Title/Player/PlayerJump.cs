//ジャンプ処理
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D m_Rigidbody2D;
    bool m_Jumping = false;
    private void Start()
    {
        if(m_Rigidbody2D == null)
        {
            Debug.LogError("Rigidbodyがアタッチされていません");
        }
    }
    private void Update()
    {
        if (!m_Jumping)
        {
            //spaceキーを押したらジャンプする
            if (Input.GetKeyDown(KeyCode.Space)||Input.GetKeyDown("joystick button 2") || Input.GetKeyDown("joystick button 0") )
            {
                float jumpPower = 4.0f;
                m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, jumpPower);

                m_Jumping = true;
            }
        }
    }
}
