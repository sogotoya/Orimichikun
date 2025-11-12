//針を指定した方角へ飛ばす処理
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scatter_Shot : MonoBehaviour
{

    [SerializeField]
    [Header("針")]
    GameObject m_Hari;

    [SerializeField]
    [Header("通常状態の針を飛ばす方向")]
    float[] m_NormalNeed;

    [SerializeField]
    [Header("怒り状態の針を飛ばす方向")]
    float[] m_AngerNeed;

    [SerializeField]
    [Header("針のスピード")]
    float m_Speed;

    [SerializeField]
    [Header("発射位置ずらす距離")]
    float m_Offset = 0.5f;

    /// <summary>
    /// 針発射呼び出し関数(bool値セットする必要あり)
    /// </summary>
    /// <param name="flag"></param>
    public void ShotStart(bool flag, GameObject shotobj)
    {
        if (flag)
        {
            StartCoroutine(AngerShotStart(shotobj));
        }
        else if(!flag)
        {
            StartCoroutine(NormalShotStart(shotobj));
        }

    }

    /// <summary>
    /// 通常状態のショット
    /// </summary>
    IEnumerator NormalShotStart(GameObject shotobj)
    {
        yield return new WaitForSeconds(0.9f);
        foreach(float angle in m_NormalNeed)
        {
            //方向指定
            Quaternion rot = Quaternion.Euler(0, 0, angle);

            //発射位置作成
            Vector3 offset = rot * Vector3.up * m_Offset;
            //発射位置設定
            Vector3 spawnPos = shotobj.transform.position + offset;

            //生成
            GameObject obj = Instantiate(m_Hari, spawnPos, rot);
            //力を加える
            Rigidbody2D rb2 = obj.GetComponent<Rigidbody2D>();
            rb2.velocity = obj.transform.up * m_Speed;
        }
        yield return null;
    }

    /// <summary>
    /// 怒り状態のショット
    /// </summary>
    IEnumerator AngerShotStart(GameObject shotobj)
    {
        foreach (float angle in m_AngerNeed)
        {
            //方向指定
            Quaternion rot = Quaternion.Euler(0, 0, angle);

            //発射位置作成
            Vector3 offset = rot * Vector3.up * m_Offset;
            //発射位置設定
            Vector3 spawnPos= shotobj.transform.position+ offset;

            //生成
            GameObject obj = Instantiate(m_Hari, spawnPos, rot);
            //力を加える
            Rigidbody2D rb2 = obj.GetComponent<Rigidbody2D>();
            rb2.velocity = obj.transform.up * m_Speed;
        }
        yield return null;
    }
}
