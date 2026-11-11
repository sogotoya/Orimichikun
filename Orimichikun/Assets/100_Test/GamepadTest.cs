using UnityEngine;

public class GamepadTest : MonoBehaviour
{
    void Update()
    {
        // 左スティック
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
       

        // ボタンチェック（0～15ぐらい試す）
        for (int i = 0; i < 20; i++)
        {
            if (Input.GetKeyDown("joystick button " + i))
            {
              
            }
        }
        if (Input.GetKeyDown("joystick button 0"))
        {
           
        }
    }
}