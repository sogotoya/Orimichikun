//----------------
//画像をそのサイズ間でループ処理するクラス
//----------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

//自動的にImageをコンポーネント
[RequireComponent(typeof(Image))]
public class BackGroundLoop : MonoBehaviour
{
    [Tooltip("ループするときの上限変数")]
    private const float m_MaxLength = 1f;

    [Tooltip("どのテクスチャをずらすかを指示する変数")]
    private const string m_PropName = "_MainTex";

    [SerializeField]
    [Tooltip("どの方向、速さで動かすか決める変数")]
    Vector2 m_OffsetSpeed;

    [Tooltip("背景画像のスクロール位置を変更するためのコピーマテリアル")]
    Material m_CopiedMaterial;

    private void Start()
    {
        Image m_Image = GetComponent<Image>();
        m_CopiedMaterial = m_Image.material;
        //nullなら開発中に強制停止/エラー表示
        Assert.IsNotNull(m_CopiedMaterial);
    }

    private void Update()
    {
        //ゲームが止まっているなら( 0 なら)ストップ
        if (Time.timeScale == 0f)
            return;

        //ある値を指定した範囲で繰り返す
        float m_x = Mathf.Repeat(Time.time * m_OffsetSpeed.x, m_MaxLength);
        float m_y = Mathf.Repeat(Time.time * m_OffsetSpeed.y, m_MaxLength);

        //どれだけ横と縦にずらすかまとめる
        Vector2 m_Offset = new Vector2(m_x, m_y);

        //画面上で背景を動かす処理
        //(SetTextureOffsetでm_PropNameを指定し、m_Offset分動かす
        m_CopiedMaterial.SetTextureOffset(m_PropName, m_Offset);
    }

    //ゲームオブジェクトが消えたら、コピーしたマテリアルを消して、変数も空にして安全にする
    private void OnDestroy()
    {
        // ゲームオブジェクト破壊時にマテリアルのコピーも消しておく
        Destroy(m_CopiedMaterial);
        m_CopiedMaterial = null;
    }
}
