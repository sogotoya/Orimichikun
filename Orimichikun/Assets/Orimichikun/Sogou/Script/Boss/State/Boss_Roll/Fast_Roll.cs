//1段階目(怒っていない)時の回転処理
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fast_Roll : MonoBehaviour
{
    [SerializeField]
    [Header("右の移動ポジション(Pointオブジェクトの子をアタッチ)")]
    Transform m_Right;
    [SerializeField]
    [Header("左の移動ポジション(Pointオブジェクトの子をアタッチ)")]
    Transform m_Left;
    [SerializeField]
    [Header("移動スピード")]
    float m_MoveSpeed = 1f;
    [SerializeField]
    [Header("左右の方向")]
    int m_Direction = -1;

    //何回右の地点に返ってきたかのカウント
    int m_Cnt = 0;
    //判定フラグ
    bool m_Flag = false;

    /// <summary>
    /// ボスの回転移動攻撃(処理を実行させたいオブジェクト、何回させるか)
    /// </summary>
    public int NormalRoll(GameObject obj, int cnt)
    {

        if (m_Cnt != cnt)
        {
            //現在の場所の更新
            Vector3 pos = obj.transform.position;
            //移動処理
            pos.x += m_MoveSpeed * Time.deltaTime * m_Direction;

            if (pos.x >= m_Right.position.x)
            {
                m_Direction = -1;
                m_Cnt++;
            }
            if (pos.x <= m_Left.position.x)
            {
                m_Direction = 1;
            }
            //座標更新
            obj.transform.position = pos;

            return 0;
        }
        else
        {
            //初期化
            m_Cnt = 0;
            m_Flag = false;
            return 100;
        }

    }


}
