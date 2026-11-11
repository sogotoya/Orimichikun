// プレイヤーの弾（炎）が当たった草を焦げた色にする
// 草には当たり判定を付けず、子の SpriteRenderer と弾の見た目の範囲が重なったかで判定する
// （弾の進み方や他のトリガー処理に影響しないようにするため）
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnableGrass : MonoBehaviour
{
    [SerializeField, Tooltip("焦げた後の色")]
    Color m_BurntColor = new Color(0.30f, 0.20f, 0.12f, 1f);

    [SerializeField, Tooltip("焦げ色に変わるまでの時間（秒）")]
    float m_BurnTime = 0.3f;

    [SerializeField, Tooltip("草の上どこまでの高さを弾が通ったら焦げるか（足元の小さな草にも届くように）")]
    float m_ReachHeight = 1.2f;

    readonly List<SpriteRenderer> m_Grass = new List<SpriteRenderer>();
    readonly HashSet<SpriteRenderer> m_Burnt = new HashSet<SpriteRenderer>();

    void Start()
    {
        // 子にある草をすべて対象にする
        GetComponentsInChildren(true, m_Grass);
    }

    void Update()
    {
        var bullets = FindObjectsOfType<Bullet>();
        if (bullets.Length == 0) return;

        foreach (var bullet in bullets)
        {
            var r = bullet.GetComponentInChildren<Renderer>();
            if (r == null) continue;
            Bounds b = r.bounds;

            foreach (var grass in m_Grass)
            {
                if (grass == null || m_Burnt.Contains(grass)) continue;
                // 草の範囲を上に m_ReachHeight だけ伸ばして判定（弾が真上を通れば焦げる）
                Bounds g = grass.bounds;
                g.max = new Vector3(g.max.x, g.max.y + m_ReachHeight, g.max.z);
                if (Overlap2D(b, g))
                {
                    m_Burnt.Add(grass);
                    StartCoroutine(Burn(grass));
                }
            }
        }
    }

    // Z（奥行き）は無視して XY だけで重なりを見る
    static bool Overlap2D(Bounds a, Bounds b)
    {
        return a.min.x < b.max.x && a.max.x > b.min.x
            && a.min.y < b.max.y && a.max.y > b.min.y;
    }

    IEnumerator Burn(SpriteRenderer grass)
    {
        Color from = grass.color;
        float t = 0f;
        while (t < m_BurnTime)
        {
            t += Time.deltaTime;
            grass.color = Color.Lerp(from, m_BurntColor, t / m_BurnTime);
            yield return null;
        }
        grass.color = m_BurntColor;
    }
}
