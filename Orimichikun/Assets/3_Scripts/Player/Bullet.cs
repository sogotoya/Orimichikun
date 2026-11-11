using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 弾の速さ
    public float m_speed = 10f;
    // 弾削除
    public float m_lifeTime = 2f;
    //向き
    private int m_direction = 1;

    void Start()
    {
        // 寿命が来たら自動で消える
        Destroy(gameObject, m_lifeTime);
    }

    void Update()
    {
        // 発射位置の向いている方向に進む
        transform.Translate(Vector2.right * m_direction * m_speed * Time.deltaTime, Space.World);
    }
    // 外部から呼ばれる「向きセット用」
    public void SetDirection(int dir)
    {
        m_direction = dir;

        // 見た目も反転
        if (dir == -1)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
    //Groundのタグに触れたら弾を消す
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
