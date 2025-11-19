//タイトルのボスの処理
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFastManager : MonoBehaviour
{
    [SerializeField]
    CameraShake m_CameraShake;
    // プレイヤーに近づくスピード
    float m_MoveSpeed = 1.0f;

    // プレイヤーにどこまで近づくか（停止距離）
    float m_StopDistance = 2.0f;

    [SerializeField]
    GameObject m_Player;

    [SerializeField]
    GameObject m_Boss;

    [SerializeField]
    AudioSource m_MoveAS;
    //繰り返し呼ばれる対策
    bool m_ShakeFlag;

    [Tooltip("最初にプレイヤーが取得するコイン")]
    public bool m_IsCoin=false;

    void Start()
    {
        m_MoveAS.enabled = false;
    }

    void Update()
    {
        if(m_IsCoin)
        {
            MoveStart();
        }
    }

    /// <summary>
    /// 最初の動き
    /// </summary>
    void MoveStart()
    {

        //コルーチン止めるための保存
        Coroutine shakeCoroutine = StartCoroutine(m_CameraShake.Shake(0.5f, 0.1f, 0.5f));

        Vector2 dir = m_Player.transform.position - m_Boss.transform.position;
        //座標が空中になるためYは0
        dir.y = 0f;
        float distance = dir.magnitude;
        //Debug.Log($"距離: {m_Distance}, 停止距離: {m_StopDistance}");

        if (distance > m_StopDistance)
        {
            // 移動
            Vector3 move = dir.normalized * m_MoveSpeed * Time.deltaTime;
            m_Boss.transform.position += move;
        }
        else//指定した地点についた
        {
            //対策
            StopCoroutine(shakeCoroutine);
            m_MoveAS.enabled = false;
            //呼び出し停止
            m_IsCoin = false;
        }
    }

    /// <summary>
    /// 後退する
    /// </summary>
    IEnumerator BackMoveStart()
    {
        //コルーチン止めるための保存
        Coroutine shakeCoroutine = StartCoroutine(m_CameraShake.Shake(0.5f, 0.1f, 0.5f));
        yield return new WaitForSeconds(0.7f);
        Vector2 dir = new Vector3(Screen.width, 0, 0) - m_Boss.transform.position;
        //座標が空中になるためYは0
        dir.y = 0f;
        float distance = dir.magnitude;

        if (distance > m_StopDistance)
        {
            // 移動
            Vector3 move = dir.normalized * m_MoveSpeed * Time.deltaTime;
            m_Boss.transform.position += move;
        }
        //4秒たったら削除
        yield return new WaitForSeconds(4f);
        m_MoveAS.enabled = false;
        //対策
        StopCoroutine(shakeCoroutine);
        Destroy(gameObject);
    }
}
