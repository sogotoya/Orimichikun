using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_B_Scope : MonoBehaviour
{
    /// <summary>
    /// 活動範囲
    /// </summary>
    public BoxCollider2D area = null;
    private void Start()
    {
        if(area == null)
        {
            Debug.Log("E_B_ScopeクラスのBoxCollider2D（行動範囲）がアタッチされていません");
        }

    }

    void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, area.bounds.min.x + 1, area.bounds.max.x - 1),
           Mathf.Clamp(transform.position.y, area.bounds.min.y + 1, area.bounds.max.y - 1), transform.position.z);
    }
}
