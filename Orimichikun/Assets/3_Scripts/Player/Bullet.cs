using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 弾の速さ
    public float m_speed = 10f;
    // 弾が消えるまでの時間
    public float m_lifeTime = 2f;
    // 向き（1:右 / -1:左）
    private int m_direction = 1;

    void Start()
    {
        // 一定時間たったら自動で消える
        Destroy(gameObject, m_lifeTime);

        // 弾は Kinematic なので、そのままだと地面・壁（Rigidbody2D なしの静的コライダー）と
        // 当たり判定が起きない。静的コライダーとも判定するようにする
        var rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.useFullKinematicContacts = true;
    }

    void Update()
    {
        // 発射した向きに進む
        transform.Translate(Vector2.right * m_direction * m_speed * Time.deltaTime, Space.World);
    }

    // 外から呼ばれる「向きセット用」
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

    // 地面・壁に当たったら弾を消す（壁の貫通防止）
    // ブロックは Ground タグではなく Ground レイヤーなので、両方で判定する
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger) return; // トリガー（判定用の範囲）では消えない

        if (collision.gameObject.CompareTag("Ground") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
