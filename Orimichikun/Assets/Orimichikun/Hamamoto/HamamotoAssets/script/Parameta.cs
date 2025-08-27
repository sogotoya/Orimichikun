using UnityEngine;
using UnityEngine.UI;

public class Parameta : MonoBehaviour
{
    //チーム
    public string m_Team;
    //HP
    public int m_Hp = 100;
    public int m_MaxHp = 100;
    [Header("死亡アニメーター")]
    public Animator m_Die;
    public void TakeDamege(int DamegePoint)
    {
        //HPが0なら何もしない（二度死なないように）
        if (m_Hp <= 0)
            return;
        m_Hp -= DamegePoint;
        //HPが０以下なら
        if (m_Hp <= 0)
        {
            m_Hp = 0;
            //死亡アニメーション（五秒後消える）
            m_Die.SetTrigger("Die");
            Destroy(gameObject, 5f);
        }


    }

}
