//プレイヤーを追従するカメラ
//CameraArea の範囲内にいる間は、その区間用の位置・サイズへ徐々に切り替える
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("追跡するターゲット")]
    public Transform m_Player;
    [Header("カメラとのオフセット")]
    public Vector3 offset = new Vector3(0f, 1f, -10f);
    [Header("追従のなめらかさ（大きいほど速く追従）")]
    [Range(0f, 1f)]
    public float m_smoothSpeed = 0.1f;
    [Header("カメラ制限")]
    public float minX, maxX;
    public float minY, maxY;

    Camera m_Camera;
    float m_DefaultSize;    //平行投影のときの元のサイズ
    float m_DefaultFov;     //透視投影のときの元の視野角

    //通常追従の位置（区間カメラとのブレンド元）
    Vector3 m_FollowPos;
    Vector3 m_LastPlayerPos;

    //区間カメラ
    CameraArea m_Area;
    float m_AreaWeight;          //0 = 通常, 1 = 区間カメラ
    float m_BlendTime = 1f;
    Vector3 m_AreaPos;
    float m_AreaZoom = 1f;       //ズームアウト倍率（1 = 通常）

    void Start()
    {
        m_Camera = GetComponent<Camera>();
        if (m_Camera != null)
        {
            m_DefaultSize = m_Camera.orthographicSize;
            m_DefaultFov = m_Camera.fieldOfView;
        }
        m_FollowPos = transform.position;
        if (m_Player != null) m_LastPlayerPos = m_Player.position;
    }

    /// <summary>
    /// 区間カメラに切り替える（CameraArea から呼ばれる）
    /// </summary>
    public void EnterArea(CameraArea area)
    {
        m_Area = area;
    }

    /// <summary>
    /// 区間カメラから通常カメラに戻す（CameraArea から呼ばれる）
    /// </summary>
    public void ExitArea(CameraArea area)
    {
        if (m_Area == area) m_Area = null;
    }

    /// <summary>
    /// カメラをプレイヤーの位置へ一気に合わせる（リスポーン直後に呼ぶ）
    /// ズームアウトの区間で死んだとき、引いたままリスポーン先へ動いて背景の外が見えるのを防ぐ
    /// </summary>
    public void SnapToPlayer()
    {
        if (m_Player == null) return;

        // リスポーン先が今の区間の外なら、区間カメラをやめる
        if (m_Area != null && !m_Area.Contains(m_Player.position)) m_Area = null;

        // 追従位置をリスポーン先にそろえる
        m_LastPlayerPos = m_Player.position;
        Vector3 desiredPosition = m_Player.position + offset;
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
        m_FollowPos = desiredPosition;

        // ズームの切り替え途中をなくして、すぐに反映する
        m_AreaWeight = m_Area != null ? 1f : 0f;
        LateUpdate();
    }

    /// <summary>
    /// 指定のズームアウト倍率のとき、z=0 の位置で映る高さの半分
    /// </summary>
    float GetHalfHeightAtGround(float cameraZ, float zoom)
    {
        if (m_Camera == null) return 0f;
        if (m_Camera.orthographic) return m_DefaultSize * zoom;
        return Mathf.Abs(cameraZ) * Mathf.Tan(m_DefaultFov * 0.5f * Mathf.Deg2Rad) * zoom;
    }

    void LateUpdate()
    {
        if (m_Player == null) return;

        // カメラはプレイヤーの子なので、プレイヤーが動いた分だけ追従位置も動かす
        m_FollowPos += m_Player.position - m_LastPlayerPos;
        m_LastPlayerPos = m_Player.position;

        // 目標位置（プレイヤーの位置 + オフセット）
        Vector3 desiredPosition = m_Player.position + offset;

        // 範囲内に制限
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);

        // 現在位置から目標位置へスムーズに移動
        m_FollowPos = Vector3.Lerp(m_FollowPos, desiredPosition, m_smoothSpeed);

        // 上の「プレイヤーが動いた分だけ追従位置も動かす」処理で制限を超えてしまうため、ここでも制限する
        // （超えたまま Lerp で押し戻されると、ジャンプの頂点付近でカメラが震え、背景の外が見えていた）
        m_FollowPos.x = Mathf.Clamp(m_FollowPos.x, minX, maxX);
        m_FollowPos.y = Mathf.Clamp(m_FollowPos.y, minY, maxY);

        // 区間カメラの目標を更新（範囲を出た後は最後の値を使って戻る）
        if (m_Area != null)
        {
            m_AreaPos = m_Area.GetCameraPosition(desiredPosition);
            m_AreaPos.z = desiredPosition.z;
            m_AreaZoom = m_Area.ZoomOut;

            // 引いたときに画面の上端・下端が背景の外に出ないよう高さを制限
            m_AreaPos.y = m_Area.ClampVertical(m_AreaPos.y, GetHalfHeightAtGround(desiredPosition.z, m_AreaZoom));
            m_BlendTime = m_Area.BlendTime;
        }

        float target = m_Area != null ? 1f : 0f;
        float speed = m_BlendTime > 0f ? Time.deltaTime / m_BlendTime : 1f;
        m_AreaWeight = Mathf.MoveTowards(m_AreaWeight, target, speed);
        float t = Mathf.SmoothStep(0f, 1f, m_AreaWeight);

        transform.position = Vector3.Lerp(m_FollowPos, m_AreaPos, t);
        // ズームアウト（映る範囲を倍率分だけ広げる）
        if (m_Camera != null)
        {
            float zoom = Mathf.Lerp(1f, m_AreaZoom, t);
            if (m_Camera.orthographic)
            {
                m_Camera.orthographicSize = m_DefaultSize * zoom;
            }
            else
            {
                float halfTan = Mathf.Tan(m_DefaultFov * 0.5f * Mathf.Deg2Rad) * zoom;
                m_Camera.fieldOfView = Mathf.Atan(halfTan) * 2f * Mathf.Rad2Deg;
            }
        }
    }
}