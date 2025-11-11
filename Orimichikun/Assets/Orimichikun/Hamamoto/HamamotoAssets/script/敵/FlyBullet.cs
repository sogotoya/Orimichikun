using UnityEngine;

public class FlyBullet : MonoBehaviour
{
    public float m_Speed = 5f;
    private Vector2 m_Direction;

    public void SetDirection(Vector2 direction)
    {
        m_Direction = direction.normalized;
    }

    void Update()
    {
        transform.Translate(m_Direction * m_Speed * Time.deltaTime);
        Destroy(gameObject, 5f);
    }
    //GroundÇÃÉ^ÉOÇ…êGÇÍÇΩÇÁíeÇè¡Ç∑
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}

