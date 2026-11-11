using UnityEngine;
using UnityEngine.UI;
public class SavePoint : MonoBehaviour
{
    [Header("復活位置")]
    public Transform m_SavePosition;
    [Header("光るクリスタル")]
    public GameObject m_ShiningCrystal;
    [Header("黒いクリスタル")]
    public GameObject m_BlackCrystal;
    [Header("SavePointのUI")]
    public Image m_SavePointUI;
    public AudioClip m_SavePointSE;
    public AudioSource m_AudioSource;

    [Header("クリスタルに触れたかどうか")]
    [SerializeField] private bool m_TriggerCrystal = false;

    private void Start()
    {
        //非表示
        m_SavePointUI.gameObject.SetActive(false);
        m_ShiningCrystal.SetActive(false);
    }
    private void Update()
    {
        //エリアに入ったらクリスタルが表示してアニメーションが動き出す
        if (m_TriggerCrystal)
        {
            m_ShiningCrystal.SetActive(true);
            m_BlackCrystal.SetActive(false);
           
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !m_TriggerCrystal)
        {
            //表示
            m_SavePointUI.gameObject.SetActive(true);
            m_AudioSource.PlayOneShot(m_SavePointSE);
            //クリスタルに触れた
            m_TriggerCrystal = true;
            //PlayScriptをプレイヤーから探しセーブ地点を変更
            collision.GetComponent<PlayScript>().UpdateSavePoint(m_SavePosition.position);
            Debug.Log("セーブ地点更新!");
        }
    }
}
