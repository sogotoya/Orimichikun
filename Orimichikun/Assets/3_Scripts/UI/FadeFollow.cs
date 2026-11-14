//ほかの UI（文字など）の透明度に合わせて、まとめてフェードさせる
//例: タイトルの「をおしてスタート！」が点滅するのに合わせて、E キー・コントローラーの絵も点滅させる
//このオブジェクトに CanvasGroup を付け、その alpha を元の UI の透明度に毎フレーム合わせる
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class FadeFollow : MonoBehaviour
{
    [SerializeField, Tooltip("透明度を合わせる元の UI（アニメーションで透明度が変わるもの）")]
    Graphic m_Source;

    CanvasGroup m_Group;

    private void Awake()
    {
        m_Group = GetComponent<CanvasGroup>();
    }

    //アニメーションが色を変えた後に合わせる
    private void LateUpdate()
    {
        if (m_Source == null) return;
        m_Group.alpha = m_Source.color.a;
    }
}
