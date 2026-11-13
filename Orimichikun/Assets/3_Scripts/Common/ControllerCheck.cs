//コントローラーが接続されているか・最後に触ったのがコントローラーかを調べる処理
//キー表示の切り替え（KeyHintUI / KeyGuideIcon）や、LT ボタンの入力で使う
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

    //最後に触ったのがコントローラーか（null = まだ何も触っていない）
    static bool? s_UsingPad;
    static int s_UsingPadCheckedFrame = -1;
    //スティックを倒したと判定する量
    const float k_StickPress = 0.5f;

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
    /// コントローラー用の表示にするか
    /// 最後に触ったのがコントローラーなら true、キーボードかマウスなら false
    /// まだ何も触っていないときは、コントローラーがつながっていれば true
    /// </summary>
    public static bool IsUsingPad
    {
        get
        {
            //1フレームに1回だけ調べる
            if (s_UsingPadCheckedFrame != Time.frameCount)
            {
                s_UsingPadCheckedFrame = Time.frameCount;
                CheckLastInput();
            }

            //コントローラーが抜かれたら PC 用にする
            if (!IsConnected) return false;
            return s_UsingPad ?? true;
        }
    }

    /// <summary>
    /// 今のフレームで触ったのがコントローラーかキーボード・マウスかを調べる処理
    /// </summary>
    static void CheckLastInput()
    {
        //コントローラーのボタン
        for (KeyCode k = KeyCode.JoystickButton0; k <= KeyCode.JoystickButton19; k++)
        {
            if (Input.GetKeyDown(k))
            {
                s_UsingPad = true;
                return;
            }
        }
        //LT とスティック（キーボードの左右キーを押していないのに左右に入っていればスティック）
        bool keyboardMove = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);
        if (GetLT() > k_TriggerPress || (!keyboardMove && Mathf.Abs(Input.GetAxisRaw("Horizontal")) > k_StickPress))
        {
            s_UsingPad = true;
            return;
        }
        //キーボードかマウスのボタン（コントローラーのボタンは上で調べ済み）
        if (Input.anyKeyDown)
        {
            s_UsingPad = false;
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
