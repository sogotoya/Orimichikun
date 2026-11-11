using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public AudioSource m_BGM;
    void Update()
    {
        // Enterキーが押されたら
        if (Input.GetKeyDown(KeyCode.Return))
        {
            m_BGM.Stop();
            SceneManager.LoadScene("Stage"); // シーン名を指定
        }
    }
}
