using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachBoss : MonoBehaviour
{
    [SerializeField]
    GameObject m_Player;

    [SerializeField]
    CameraShake m_CameraShake;

    public bool m_ApFlag=false;

    // プレイヤーに近づくスピード
    float m_MoveSpeed = 1.0f;

    // プレイヤーにどこまで近づくか（停止距離）
    float m_StopDistance = 2f;

    public IEnumerator BossMoveStart()
    {
        //コルーチン止めるための保存
        Coroutine shakeCoroutine = StartCoroutine(m_CameraShake.Shake(0.5f, 0.1f,0.5f));

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
            yield return new WaitForSeconds(1f);
            m_ApFlag = true;
            Debug.Log("到着");
        }
    }
}
