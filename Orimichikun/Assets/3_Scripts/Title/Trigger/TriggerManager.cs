using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// チュートリアルの表示やスポーン管理
/// </summary>
public class TriggerManager : MonoBehaviour
{
    //ジャンプ表示
    public bool m_IsJump;


    //ダブルジャンプ表示
    public bool m_IsDoubleJump;
    //弾表示
    public bool m_IsAttackTama;
    //背景動き開始
    public bool m_IsMoveTitle;
    //敵表示
    public bool m_IsEnemy;
    //タイトルからの移行フラグ
    public bool m_IsTitle=false;
    [Header("敵の出現位置とプレハブ")]
    [SerializeField]
    GameObject m_Enemy;
    [SerializeField]
    Transform m_Tf;
    bool m_IsSpown;//1回のみ生成


    [SerializeField]
    [Header("ゴーストジャンプアニメーションが入っているオブジェクト")]
    GameObject m_GhostJump;
    [SerializeField]
    [Header("ゴーストダブルジャンプアニメーションが入っているオブジェクト")]
    GameObject m_GhostDoubleJump;
    [Header("ゴーストダブルジャンプアニメーションが入っているオブジェクト")]
    [SerializeField]
    GameObject m_GhostAttackObj;
    [SerializeField]
    GameObject m_GhostTama;

    [Header("ゴーストと一緒に出すボタン説明（ButtonBlink、未設定なら出さない）")]
    [SerializeField]
    GameObject m_JumpGuide;
    [SerializeField]
    GameObject m_DoubleJumpGuide;
    [SerializeField]
    GameObject m_AttackGuide;
    [Header("背景を動かすオブジェクト")]
    [SerializeField]
    GameObject m_HaikeiMoveObj;
    [SerializeField]
    PlayScript m_PS;
    [SerializeField]
    playershoot m_PST;
    [SerializeField]
    Animator m_Animator;
    [SerializeField]
    GameObject m_Title;
    [SerializeField]
    SmartPhoneUI m_SmartPhoneUI;
    private void Update()
    {
        //範囲内にいるなら表示・範囲内にいなければ非表示
        if (m_IsJump)
        {
            m_GhostJump.SetActive(true);
        }
        else
        {
            m_GhostJump.SetActive(false);
        }
        if (m_IsDoubleJump)
        {
            m_GhostDoubleJump.SetActive(true);
        }
        else
        {
            m_GhostDoubleJump.SetActive(false);
        }
        if (m_IsAttackTama)
        {
            m_GhostAttackObj.SetActive(true);
            m_GhostTama.SetActive(true);
        }
        else
        {
            m_GhostAttackObj.SetActive(false);
            m_GhostTama.SetActive(false);
        }

        //ゴーストのお手本と一緒に、押すボタンを光らせて見せる
        SetGuide(m_JumpGuide, m_IsJump);
        SetGuide(m_DoubleJumpGuide, m_IsDoubleJump);
        SetGuide(m_AttackGuide, m_IsAttackTama);

        if (m_IsEnemy)
        {
            if (!m_IsSpown)
            {
                EnemySpown();
            }
        }

        if(m_IsMoveTitle)
        {
            m_HaikeiMoveObj.SetActive(true);
            m_PS.enabled = false;
            m_PST.enabled = false;
            m_Animator.SetTrigger("TitleMove");
            m_Title.SetActive(true);
            m_IsTitle = true;
            
            // スマホUIに「次のステージへ」ボタンを表示させる
            if (m_SmartPhoneUI != null)
            {
                m_SmartPhoneUI.ShowNextStageUI(true);
            }
        }
       
    }


    /// <summary>
    /// ボタン説明の表示切り替え（状態が変わったときだけ切り替えて、点滅を最初からやり直さないようにする）
    /// </summary>
    void SetGuide(GameObject guide, bool show)
    {
        if (guide != null && guide.activeSelf != show) guide.SetActive(show);
    }

    /// <summary>
    /// 敵の生成
    /// </summary>
    void EnemySpown()
    {
        Instantiate(m_Enemy, m_Tf.position, m_Enemy.transform.rotation);
        m_IsSpown = true;
    }
}
