using UnityEngine;

public class Damege2D : MonoBehaviour
{
    // 攻撃力
    public int m_DamegePoint;
    // パラメータ（自分のParameta2D）
    public Parameta2D m_Parameta;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!m_Parameta) return;

        Parameta2D otherParam = other.gameObject.GetComponent<Parameta2D>();
        if (otherParam)
        {
            // チームが違う場合のみダメージ
            if (otherParam.m_Team != m_Parameta.m_Team)
            {
                otherParam.TakeDamage(m_DamegePoint);
    
            }
        }
    }
}
