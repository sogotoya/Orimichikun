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

        obj.transform.position = Vector3.MoveTowards(current, target, 4.0f * Time.deltaTime);

        // 完了判定
        return Vector3.Distance(obj.transform.position, target) < 0.05f;

    }
}
