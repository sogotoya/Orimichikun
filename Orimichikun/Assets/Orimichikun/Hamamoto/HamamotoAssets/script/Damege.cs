using UnityEngine;

public class DamegeSystem : MonoBehaviour
{
    //攻撃力
    public int m_DamegePoint;
    //パラメータ
    public Parameta m_Parameta;


    //衝突
    private void OnTriggerEnter(Collider other)
    {

        //パラメータがついているか
        if (m_Parameta)
        {


            //パラメータを持っているか
            if (other.gameObject.GetComponent<Parameta>())
            {

                //パラメータを持っていて同じチームではないか
                if (other.gameObject.GetComponent<Parameta>().m_Team != m_Parameta.m_Team)
                {

                    //ダメージを与える
                    other.gameObject.GetComponent<Parameta>().TakeDamege(m_DamegePoint);
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

