//会話（コメント）の表示時間をまとめて管理する
//FastMessage / StartTextManager / EndTextManager がこの値を使う
public static class MessageTiming
{
    /// <summary>
    /// 1 つの会話を表示してから次に切り替わるまでの秒数
    /// </summary>
    public const float DisplayTime = 3f;

    /// <summary>
    /// 会話が始まる前の待ち時間
    /// </summary>
    public const float StartDelay = 1f;
}
