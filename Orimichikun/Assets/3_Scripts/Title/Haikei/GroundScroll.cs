//タイトルが流れるとき、背景（HaikeiMove）と一緒に床も左へ流してループさせる
//床の上に置いた草（m_GrassRoot の子）も、乗っているブロックと一緒に動かす
using System.Collections.Generic;
using UnityEngine;

public class GroundScroll : MonoBehaviour
{
    [SerializeField]
    [Header("流す床ブロック（左から右の順に並べる）")]
    Transform[] m_Blocks;

    [SerializeField]
    [Header("床ブロック1つ分の幅")]
    float m_BlockWidth = 3f;

    [SerializeField]
    [Header("速さを合わせる背景（未設定なら m_Speed を使う）")]
    HaikeiMove m_SyncHaikei;

    [SerializeField]
    float m_Speed = 1.5f;

    [SerializeField]
    [Header("床の上の草をまとめた親（一緒に動かす）")]
    Transform m_GrassRoot;

    Transform m_Last;      //一番右のブロック
    float m_LoopX;         //これより左に行ったら右端へ回す

    //草と、乗っているブロック・ブロックからの位置
    readonly List<(Transform grass, Transform block, Vector3 offset)> m_Grass = new List<(Transform, Transform, Vector3)>();

    void Start()
    {
        if (m_Blocks == null || m_Blocks.Length == 0)
        {
            Debug.LogError("床ブロックがセットされていません", this);
            enabled = false;
            return;
        }
        m_Last = m_Blocks[m_Blocks.Length - 1];
        m_LoopX = m_Blocks[0].position.x - m_BlockWidth;

        //各ブロックの上にある草を覚える
        if (m_GrassRoot != null)
        {
            foreach (Transform g in m_GrassRoot)
            {
                foreach (var b in m_Blocks)
                {
                    var col = b.GetComponent<Collider2D>();
                    if (col == null) continue;
                    Bounds bd = col.bounds;
                    bool onTop = g.position.x >= bd.min.x && g.position.x < bd.max.x
                              && g.position.y >= bd.max.y - 0.3f && g.position.y <= bd.max.y + 1.0f;
                    if (onTop)
                    {
                        m_Grass.Add((g, b, g.position - b.position));
                        break;
                    }
                }
            }
        }
    }

    void Update()
    {
        float speed = m_SyncHaikei != null ? m_SyncHaikei.m_WalkSpeed : m_Speed;

        foreach (var b in m_Blocks)
        {
            if (b == null) continue;
            b.position += Vector3.left * speed * Time.deltaTime;

            //左へ流れきったら右端の後ろへつなげる
            if (b.position.x < m_LoopX)
            {
                b.position = m_Last.position + Vector3.right * m_BlockWidth;
                m_Last = b;
            }
        }

        //草をブロックに合わせて動かす
        foreach (var (grass, block, offset) in m_Grass)
        {
            if (grass != null) grass.position = block.position + offset;
        }
    }
}
