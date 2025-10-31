using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    [Header("敵のprefab ※一種類共通")]
    public GameObject m_Enemy;

    [Header("スポーン位置（複数登録してください）")]
    public Transform[] m_SpawnPoints;

    // スポーンされた敵を保存しておく
    private GameObject[] m_CurrentEnemies;

    void Start()
    {
        //配列の長さにあわせて確保
        m_CurrentEnemies = new GameObject[m_SpawnPoints.Length];

        //最初に全てスポーン
        SpawnAllEnemies();
    }

    // プレイヤーから呼ばれる
    public void Respawn()
    {
        Debug.Log("Respawn All Enemies!");

        //全部消す
        for (int i = 0; i < m_CurrentEnemies.Length; i++)
        {
            if (m_CurrentEnemies[i] != null)
            {
                Destroy(m_CurrentEnemies[i]);
            }
        }

        //再生成
        SpawnAllEnemies();
    }

    private void SpawnAllEnemies()
    {
        for (int i = 0; i < m_SpawnPoints.Length; i++)
        {
            //指定位置に敵生成
            m_CurrentEnemies[i] = Instantiate(m_Enemy, m_SpawnPoints[i].position, Quaternion.identity);
        }
    }
}
