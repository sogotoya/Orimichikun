// Cinemachine の Impulse を使ったカメラの揺れ
// 揺れを受け取るには、仮想カメラに CinemachineImpulseListener を付けておくこと
using System.Collections;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineImpulseSource))]
public class CinemachineShake : MonoBehaviour
{
    [SerializeField, Tooltip("揺れの強さ")]
    float m_Force = 0.1f;

    [SerializeField, Tooltip("1回の揺れの長さ（秒）")]
    float m_Duration = 0.5f;

    CinemachineImpulseSource m_Source;
    Coroutine m_LoopCoroutine;

    void Awake()
    {
        m_Source = GetComponent<CinemachineImpulseSource>();
        var def = m_Source.m_ImpulseDefinition;
        def.m_ImpulseDuration = m_Duration;            // Uniform 等の場合の長さ
        def.m_TimeEnvelope.m_AttackTime = 0f;          // Legacy（ノイズ信号）の場合の長さ
        def.m_TimeEnvelope.m_SustainTime = m_Duration;
        def.m_TimeEnvelope.m_DecayTime = 0.05f;
    }

    /// <summary>
    /// 1回だけ揺らす
    /// </summary>
    public void Shake()
    {
        m_Source.GenerateImpulseWithForce(m_Force);
    }

    /// <summary>
    /// StopShake() が呼ばれるまで揺らし続ける
    /// delay 秒待ってから揺れ始める
    /// </summary>
    public void StartShake(float delay = 0f)
    {
        if (m_LoopCoroutine != null) return; // 二重起動を防ぐ
        m_LoopCoroutine = StartCoroutine(ShakeLoop(delay));
    }

    /// <summary>
    /// 揺らし続けるのを止める（揺れ途中の分は自然に収まる）
    /// </summary>
    public void StopShake()
    {
        if (m_LoopCoroutine == null) return;
        StopCoroutine(m_LoopCoroutine);
        m_LoopCoroutine = null;
    }

    /// <summary>
    /// delay 秒後から duration 秒間揺らし続ける
    /// </summary>
    public void ShakeFor(float duration, float delay = 0f)
    {
        StopShake();
        m_LoopCoroutine = StartCoroutine(ShakeForRoutine(duration, delay));
    }

    IEnumerator ShakeForRoutine(float duration, float delay)
    {
        if (delay > 0f) yield return new WaitForSeconds(delay);
        float end = Time.time + duration;
        var wait = new WaitForSeconds(m_Duration);
        while (Time.time < end)
        {
            Shake();
            yield return wait;
        }
        m_LoopCoroutine = null;
    }

    IEnumerator ShakeLoop(float delay)
    {
        if (delay > 0f) yield return new WaitForSeconds(delay);
        var wait = new WaitForSeconds(m_Duration);
        while (true)
        {
            Shake();
            yield return wait;
        }
    }
}
