//アタッチした順番にオブジェクトを表示させる処理
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder_Position : MonoBehaviour
{
    [Tooltip("雷終了イベント")]
    public System.Action OnThunderEnd; 


    [SerializeField]
    [Header("雷を降らしたい順番")]
    GameObject[] m_Thunder_Obj;

    [SerializeField]
    ThunderWarning m_TW;

    /// <summary>
    /// 落雷攻撃スタート処理
    /// </summary>
    public void ThunderAttackStart()
    {
        StartCoroutine(ThunderAttack());
    }

    IEnumerator ThunderAttack()
    {
        //警告が全部点灯し終わるまで待つ
        yield return StartCoroutine(m_TW.OnThunderWarningCoroutine());

        foreach (GameObject obj in m_Thunder_Obj)
        {
            obj.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            obj.SetActive(false);
        }
        //すべて終わったら通知
        OnThunderEnd?.Invoke();

    }

}
