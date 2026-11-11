//コインを取った時のBossの移動処理
//揺れは Cinemachine（CinemachineShake）で行う
using System.Collections;
using UnityEngine;

public class CoinMove : MonoBehaviour
{
    enum MoveState
    {
        Wait,       // コインが取られるまで待機
        Approach,   // プレイヤーに近づく
        Talk,       // 会話中
        Retreat,    // 後退して消える
    }

    [SerializeField]
    FastMessage m_FM;

    [SerializeField, Tooltip("Cinemachine で画面を揺らすコンポーネント")]
    CinemachineShake m_Shake;

    [SerializeField]
    GameObject m_Coin;

    [SerializeField]
    GameObject m_Player;

    [SerializeField]
    AudioSource m_MoveAS;

    [SerializeField, Tooltip("プレイヤーに近づくスピード")]
    float m_MoveSpeed = 1.0f;

    [SerializeField, Tooltip("プレイヤーにどこまで近づくか（停止距離）")]
    float m_StopDistance = 2f;

    [SerializeField, Tooltip("動き出してから揺れ始めるまでの時間")]
    float m_ShakeDelay = 0.5f;

    [SerializeField, Tooltip("会話後、後退し始めるまでの時間")]
    float m_RetreatDelay = 0.7f;

    [SerializeField, Tooltip("後退してから消えるまでの時間")]
    float m_RetreatTime = 4f;

    MoveState m_State = MoveState.Wait;

    void Start()
    {
        if (m_FM == null || m_Shake == null || m_Player == null || m_MoveAS == null)
            Debug.LogWarning($"{name}: CoinMove の参照が設定されていません", this);
        if (m_MoveAS != null) m_MoveAS.enabled = false;
    }

    void Update()
    {
        switch (m_State)
        {
            case MoveState.Wait:
                //コインが取られた
                if (m_Coin == null)
                {
                    m_State = MoveState.Approach;
                    SetMoving(true);
                }
                break;

            case MoveState.Approach:
                if (MoveTo(m_Player.transform.position))
                {
                    //指定した地点についた → コメント表示
                    SetMoving(false);
                    m_FM.m_MessageFlag = true;
                    m_State = MoveState.Talk;
                }
                break;

            case MoveState.Talk:
                //FastMessageのContact()が終わったら後退
                if (m_FM.m_ContactFlag)
                {
                    m_State = MoveState.Retreat;
                    StartCoroutine(Retreat());
                }
                break;
        }
    }

    /// <summary>
    /// 後退して削除
    /// </summary>
    IEnumerator Retreat()
    {
        SetMoving(true);
        yield return new WaitForSeconds(m_RetreatDelay);

        float timer = 0f;
        while (timer < m_RetreatTime)
        {
            MoveTo(new Vector3(Screen.width, 0, 0));
            timer += Time.deltaTime;
            yield return null;
        }

        SetMoving(false);
        Destroy(gameObject);
    }

    /// <summary>
    /// 目標へX方向に移動する。停止距離以内に着いたら true
    /// </summary>
    bool MoveTo(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        //座標が空中になるためYは0
        dir.y = 0f;
        dir.z = 0f;
        if (dir.magnitude <= m_StopDistance) return true;

        transform.position += dir.normalized * m_MoveSpeed * Time.deltaTime;
        return false;
    }

    /// <summary>
    /// 移動音と画面の揺れの ON/OFF
    /// </summary>
    void SetMoving(bool moving)
    {
        if (m_MoveAS != null) m_MoveAS.enabled = moving;
        if (m_Shake == null) return;
        if (moving) m_Shake.StartShake(m_ShakeDelay);
        else m_Shake.StopShake();
    }
}
