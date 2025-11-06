//コインを取った時のBossの移動処理
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class CoinMove : MonoBehaviour
{
    [SerializeField]
    FastMessage m_FM;
    //何回呼ばれたかのカウント
    int m_FlagCount = 0;

    [SerializeField]
    CameraShake m_CameraShake;
    // プレイヤーに近づくスピード
    float m_MoveSpeed = 1.0f;

    // プレイヤーにどこまで近づくか（停止距離）
    float m_StopDistance = 2f;

    [SerializeField]
    GameObject m_Coin;

    [SerializeField]
    GameObject m_Player;

    //繰り返し呼ばれる対策
    bool m_ShakeFlag;

    void Start()
    {
        if (m_Coin == null) return;
        if (m_CameraShake == null) return;
        if (m_FM == null) return;
    }

    void Update()
    {
        //コインをとられた、メッセージ表示される前まで
        if (m_Coin == null && !m_FM.m_MessageFlag)
        {
            MoveStart();
        }
        //FastMessageのContact()が終わったら起動
        if (m_FM.m_ContactFlag)
        {
            StartCoroutine(BackMoveStart());
        }
    }

    /// <summary>
    /// 最初の動き
    /// </summary>
    void MoveStart()
    {
        //コルーチン止めるための保存
        Coroutine shakeCoroutine = StartCoroutine(m_CameraShake.Shake(0.5f, 0.1f, 0.5f));

        Vector2 dir = m_Player.transform.position - transform.position;
        //座標が空中になるためYは0
        dir.y = 0f;
        float distance = dir.magnitude;
        //Debug.Log($"距離: {m_Distance}, 停止距離: {m_StopDistance}");

        if (distance > m_StopDistance)
        {
            // 移動
            Vector3 move = dir.normalized * m_MoveSpeed * Time.deltaTime;
            transform.position += move;
        }
        else//指定した地点についた
        {
            //対策
            StopCoroutine(shakeCoroutine);

            //コメント表示
            m_FM.m_MessageFlag = true;
        }
    }

    /// <summary>
    /// 後退する
    /// </summary>
    IEnumerator BackMoveStart()
    {
        //コルーチン止めるための保存
        Coroutine shakeCoroutine = StartCoroutine(m_CameraShake.Shake(0.5f, 0.1f,0.5f));
        yield return new WaitForSeconds(0.7f);
        Vector2 dir = new Vector3(Screen.width, 0, 0) - transform.position;
        //座標が空中になるためYは0
        dir.y = 0f;
        float distance = dir.magnitude;

        if (distance > m_StopDistance)
        {
            // 移動
            Vector3 move = dir.normalized * m_MoveSpeed * Time.deltaTime;
            transform.position += move;
        }
        //4秒たったら削除
        yield return new WaitForSeconds(4f);
        //対策
        StopCoroutine(shakeCoroutine);
        Destroy(gameObject);
    }
}
