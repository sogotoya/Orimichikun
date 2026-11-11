using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImomushiMove : MonoBehaviour
{

    void Update()
    {
        transform.Translate(Vector2.right * 1 * 1 * Time.deltaTime);
    }
}
