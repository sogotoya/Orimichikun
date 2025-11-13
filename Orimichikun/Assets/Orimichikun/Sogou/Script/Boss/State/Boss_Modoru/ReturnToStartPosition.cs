//指定した位置に戻すスクリプト
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.UI.GridLayoutGroup;

public class ReturnToStartPosition : MonoBehaviour
{
    [SerializeField]
    Transform m_Tf;


    public bool ReturnPosition(GameObject obj)
    {
        Vector3 current = obj.transform.position;
        Vector3 target = m_Tf.position;
        target.y = current.y;

        float distance = Vector3.Distance(obj.transform.position, target);

        if (distance > 0.5f)
        {
            // まだ離れてたら近づける
            obj.transform.position = Vector3.MoveTowards(current, target, 4.0f * Time.deltaTime);
            Debug.Log("指定された場所に近づく中");
            return false;
        }
        else
        {
            // ほぼ同じ位置なら完全に固定する（誤差リセット）
            obj.transform.position = target;
            return true;
        }

    }
}
