//コントローラーが接続されているかを調べる処理
//キー表示の切り替え（KeyGuideIcon）や、LT ボタンの入力で使う
using UnityEngine;

public static class ControllerCheck
{
    //接続を調べ直す間隔（秒）。GetJoystickNames は毎フレーム呼ぶと少し重いため
    const float k_CheckInterval = 0.5f;

    //LT を押したと判定する深さ（0〜1）
    const float k_TriggerPress = 0.5f;

    //InputManager に追加した LT 用の軸の名前
    //LT … 9番目の軸（Windows の Xbox 型コントローラー）
    static readonly string[] k_LTAxisNames = { "LT" };

    static bool s_IsConnected;
    static float s_NextCheckTime = -1f;

    //LT の押した瞬間の判定用
    static bool s_LTWasPressed;
    static int s_LTCheckedFrame = -1;
    static bool s_LTDown;
    //InputManager に無い軸は、何度も警告しないように覚えておく
    static readonly System.Collections.Generic.HashSet<string> s_MissingAxes = new System.Collections.Generic.HashSet<string>();

    /// <summary>
    /// コントローラーが1台でも接続されていれば true
    /// </summary>
    public static bool IsConnected
    {
        get
        {
            //一定時間ごとに調べ直す
            if (Time.unscaledTime >= s_NextCheckTime)
            {
                s_NextCheckTime = Time.unscaledTime + k_CheckInterval;
                s_IsConnected = false;
                foreach (string name in Input.GetJoystickNames())
                {
                    //一度つないで抜いたコントローラーは空の名前で残るので除外
                    if (!string.IsNullOrEmpty(name))
                    {
                        s_IsConnected = true;
                        break;
                    }
                }
            }
            return s_IsConnected;
        }
    }

    /// <summary>
    /// LT を押した瞬間だけ true（同じフレームなら何回呼んでも同じ結果）
    /// </summary>
    public static bool IsLTDown()
    {
        //同じフレームで2回目以降は前の結果を返す
        if (s_LTCheckedFrame == Time.frameCount) return s_LTDown;
        s_LTCheckedFrame = Time.frameCount;

        bool pressed = GetLT() > k_TriggerPress;
        s_LTDown = pressed && !s_LTWasPressed;
        s_LTWasPressed = pressed;
        return s_LTDown;
    }

    /// <summary>
    /// LT の押し込み量（0〜1）を取る
    /// </summary>
    static float GetLT()
    {
        //どれか1つでも押されていれば押したことにする
        float value = 0f;
        foreach (string axis in k_LTAxisNames)
        {
            if (s_MissingAxes.Contains(axis)) continue;
            try
            {
                value = Mathf.Max(value, Input.GetAxisRaw(axis));
            }
            catch (System.ArgumentException)
            {
                //InputManager に軸が無いとき（設定を書き換えた後は Unity の再起動が必要なことがある）
                s_MissingAxes.Add(axis);
                Debug.LogWarning($"InputManager に「{axis}」の軸がありません。Unity を再起動するか、Project Settings > Input Manager を確認してください。");
            }
        }
        return value;
    }
}
