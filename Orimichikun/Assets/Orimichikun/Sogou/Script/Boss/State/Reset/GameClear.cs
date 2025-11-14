using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClear : MonoBehaviour
{
    public bool m_BossDie = false;

    // Stay ’†‚ÉŒ©‚Â‚¯‚½“G‚ðˆêŽž“I‚É•Û‘¶‚·‚é
    List<GameObject> removeList = new List<GameObject>();

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!m_BossDie) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            removeList.Add(collision.gameObject);
        }
    }

    private void LateUpdate()
    {
        // LateUpdate ‚ÅˆÀ‘S‚Éíœ
        if (removeList.Count > 0)
        {
            foreach (var obj in removeList)
            {
                if (obj != null)
                {
                    Destroy(obj);
                }
            }

            removeList.Clear();
        }
    }
}
