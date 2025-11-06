//カメラの揺れの処理
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    /// <summary>
    /// カメラを揺らすコルーチン
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
        while (timer < magnitude)
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
}
