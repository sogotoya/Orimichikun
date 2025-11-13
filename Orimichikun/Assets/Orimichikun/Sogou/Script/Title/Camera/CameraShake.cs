//カメラの揺れの処理
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    /// <summary>
    /// カメラを揺らすコルーチン(カメラがローカルの場合）
    /// duration は揺れている継続時間
    /// magnitude は揺れの大きさ
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="magnitude"></param>
    /// <returns></returns>
    public IEnumerator Shake(float duration, float magnitude,float time)
    {
        Debug.Log("グラグラ");
        yield return new WaitForSeconds(time);
        //カメラの元位置を保存
        //親オブジェクトを基準にしないのでローカル
        Vector3 camepos = transform.localPosition;

        float timer = 0f;

        //指定時間が終わるまでループ
        while (timer < duration)
        {
            //それぞれランダムに揺らす
            //乱数を生成してmagnitudeを掛ける
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            //カメラ位置をランダムに少しずらす
            transform.localPosition=new Vector3(x,y,camepos.z);

            timer += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = camepos;
    }

    /// <summary>
    /// カメラを揺らすコルーチン（カメラがローカルじゃない場合）
    /// duration は揺れている継続時間
    /// magnitude は揺れの大きさ
    /// tf　は座標
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="magnitude"></param>
    /// <returns></returns>
    public IEnumerator BossShake(float duration, float magnitude, float time,Transform tf)
    {
        Debug.Log("グラグラ");
        yield return new WaitForSeconds(time);
        //カメラの元位置を保存
        //親オブジェクトを基準にしないのでローカル
        Vector3 camepos = tf.transform.position;

        float timer = 0f;

        //指定時間が終わるまでループ
        while (timer < duration)
        {
            //それぞれランダムに揺らす
            //乱数を生成してmagnitudeを掛ける
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            //カメラ位置をランダムに少しずらす
            tf.position = camepos+new Vector3(x, y, 0f);

            timer += Time.deltaTime;
            yield return null;
        }
       tf.position = camepos;
    }
}
