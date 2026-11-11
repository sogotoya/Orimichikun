using UnityEngine;

public class ChangeSenseHP : MonoBehaviour
{
    //ŠO•”‚©‚ç‚Å‚à
    public static ChangeSenseHP m_Instance;
    public int PlayerHP = 10;

    private void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
