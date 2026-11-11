using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_B_Move_v2 : MonoBehaviour
{
    [Header("待機時間(秒)の最小値")]
    public float m_WaitMin = 1f;
    [Header("待機時間(秒)の最大値")]
    public float m_WaitMax = 3f;

    [Header("移動距離(ユニット)の最小値")]
    public float m_DistanceMin = 1f;
    [Header("移動距離(ユニット)の最大値")]
    public float m_DistanceMax = 3f;

    [Header("移動にかける時間(秒)")]
    public float m_MoveDuration = 2f;

    [Header("移動ベクトル(方向)向き変えたかったらこれを変更する")]
    public Vector3 m_MoveDirection = Vector3.right;

    [Header("方向を毎回ランダム反転する")]
    public bool m_RandomizeDirection = true;

    [Header("イージングを適用する(自然な減速)")]
    public bool m_UseEaseOut = true;

    private enum State { Waiting, Moving }
    [Header("行動ステート")]
    private State m_State = State.Waiting;


    [Header("現在の状態に入ってからの経過時間(秒)")]
    private float m_Timer = 0f;
    [Header("現在の状態を継続する合計時間(秒)")]
    private float m_TargetTime = 0f;

    [Header("今回の移動の開始位置(ワールド座標)")]
    private Vector3 m_StartPos;
    [Header("今回の移動の目標位置(ワールド座標)")]
    private Vector3 m_TargetPos;

    void Start()
    {
        // 最初は待機状態に遷移(待機時間を抽選する)
        BeginWait();
    }

    void Update()
    {
        //時間加算
        m_Timer += Time.deltaTime;

        if (m_State == State.Waiting)
        {
            // 待機ステートの場合
            // 待機時間を経過したら
            if (m_Timer >= m_TargetTime)
            {
                // 移動状態へ切り替える
                BeginMove();
            }
        }
        else if (m_State == State.Moving)
        {
            // 移動ステートの場合
            // 進行度(0→1)を計算(0割防止)
            float Dummy = (m_TargetTime <= 0f) ? 1f : m_Timer / m_TargetTime;
            // 進行度を0～1にクランプする
            Dummy = Mathf.Clamp01(Dummy);

            // 見た目を自然にするためにEaseOutを適用
            float Dummy2 = m_UseEaseOut ? EaseOutCubic(Dummy) : Dummy;

            // 始点→目標へ補間でリープする
            Vector3 pos = Vector3.Lerp(m_StartPos, m_TargetPos, Dummy2);
            // 実際のワールド座標に位置確定
            transform.position = pos;

            // 進行度が1で、移動終了になったら
            if (Dummy >= 1f)
            {
                // 最終位置へスナップして誤差を除去
                transform.position = m_TargetPos;
                // 次のサイクルのため待機状態へ戻る(原点へは戻さない)
                BeginWait();
            }
        }
    }

    /// <summary>
    /// 待機開始処理
    /// </summary>
    void BeginWait()
    {
        //待機ステートにする
        m_State = State.Waiting;
        // 経過時間をリセット
        m_Timer = 0f;
        // 万一Min>Maxだった場合に備えて下限を正規化する。まぁ、やる奴いないだろうが…。
        float waitMin = Mathf.Min(m_WaitMin, m_WaitMax);
        // 上限を正規化
        float waitMax = Mathf.Max(m_WaitMin, m_WaitMax);
        // 次の待機時間(秒)ランダムセット
        m_TargetTime = Random.Range(waitMin, waitMax);
    }

    /// <summary>
    /// 移動開始処理
    /// </summary>
    void BeginMove()
    {
        // 移動ステートにする
        m_State = State.Moving;
        // 経過時間をリセット
        m_Timer = 0f;

        // 距離下限(負値を0に補正＋正規化)
        float dMin = Mathf.Max(0f, Mathf.Min(m_DistanceMin, m_DistanceMax));
        // 距離上限(正規化)
        float dMax = Mathf.Max(m_DistanceMin, m_DistanceMax);
        // 今回の移動距離(ユニット)をランダムでセット
        float distance = Random.Range(dMin, dMax);

        // 基本方向ベクトルがゼロの場合
        Vector3 baseDir = (m_MoveDirection == Vector3.zero)
            ? Vector3.right         // デフォルトで+X方向を採用
            : m_MoveDirection;      // 指定された方向を使用

        // 方向ベクトルを正規化(大きさ1にする)
        baseDir.Normalize();

        // 反転設定ONなら50%で反転(−1)にする、同じ方向は癪だから
        int sign = (m_RandomizeDirection && Random.value < 0.5f) ? -1 : 1;
        // 実際に使う今回の方向(正規化済み＋反転反映)
        Vector3 dir = baseDir * sign;

        // 今回の移動の始点を記録
        m_StartPos = transform.position;
        // 始点から方向×距離ぶん先を目標地点とする(原点へは戻さない、そうしないと移動にならない)
        m_TargetPos = m_StartPos + dir * distance;
        // まぁ、何秒間で移動するかを設定するにはこれいるよな?
        m_TargetTime = Mathf.Max(0.0001f, m_MoveDuration);
    }

    /// <summary>
    /// 終盤で減速する三次曲線イージング
    /// </summary>
    /// <param name="Dummy">0→1</param>
    /// <returns></returns>
    float EaseOutCubic(float Dummy)
    {
        // 数式: 1 - (1 - Dummy)^3
        return 1f - Mathf.Pow(1f - Dummy, 3f);
    }
}