//特定の区間だけカメラを切り替えるスクリプト（プレハブ: 4_Prefabs/Camera/CameraArea）
//プレイヤーがトリガー範囲（BoxCollider2D）に入ると、子の CameraPoint の位置・指定サイズへ徐々に切り替える
//範囲から出ると、いつもの追従カメラへ徐々に戻る
//既定: 横はプレイヤーを追い、縦は CameraPoint の高さに固定してズームアウト（上下の足場と落下先を同時に見せる）
//使い方: プレハブを置く → BoxCollider2D の範囲と CameraPoint の高さを調整（シーンビューの水色の枠が映る範囲の目安）
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CameraArea : MonoBehaviour
{
    [SerializeField]
    [Header("カメラの中心位置（未設定ならこのオブジェクトの位置）")]
    Transform m_CameraPoint;

    [SerializeField]
    [Header("ズームアウト倍率（1 = 通常。2 なら上下左右 2 倍の範囲が映る）")]
    [Min(0.1f)]
    float m_ZoomOut = 1.75f;

    [SerializeField]
    [Header("切り替え・戻りにかける秒数")]
    float m_BlendTime = 1f;

    [SerializeField]
    [Header("横方向を CameraPoint に固定する（オフならプレイヤーを追う）")]
    bool m_LockX = false;

    [SerializeField]
    [Header("縦方向を CameraPoint に固定する（オフならプレイヤーを追う）")]
    bool m_LockY = true;

    [SerializeField]
    [Header("画面の上端を制限する（引いたときに背景の外＝空が見えないように）")]
    bool m_LimitTop = true;

    [SerializeField]
    [Tooltip("画面の上端がこの高さ（z=0 の位置で）より上に出ないよう、カメラを下げる。シーンビューの赤い線")]
    float m_TopLimitY = 4.45f;

    [SerializeField]
    [Header("画面の下端を制限する（背景の下の端が見えないように）")]
    bool m_LimitBottom = true;

    [SerializeField]
    [Tooltip("画面の下端がこの高さ（z=0 の位置で）より下に出ないよう、カメラを上げる。シーンビューの赤い線")]
    float m_BottomLimitY = -2.65f;

    PlayerCamera m_PlayerCamera;

    //プレイヤーのコライダーが複数あっても正しく判定するためのカウント
    int m_InsideCount;

    public float ZoomOut => m_ZoomOut;
    public float BlendTime => m_BlendTime;

    /// <summary>
    /// 区間内でのカメラ位置（固定しない軸は通常追従の位置を使う）
    /// </summary>
    public Vector3 GetCameraPosition(Vector3 followPosition)
    {
        Vector3 point = m_CameraPoint != null ? m_CameraPoint.position : transform.position;
        return new Vector3(
            m_LockX ? point.x : followPosition.x,
            m_LockY ? point.y : followPosition.y,
            followPosition.z);
    }

    /// <summary>
    /// 画面の上端・下端の制限をかけたカメラの高さ
    /// halfHeight は区間内（ズームアウト後）に z=0 の位置で映る高さの半分
    /// 上下の制限の間が画面より狭いときは、その真ん中に合わせる（倍率を下げて調整する）
    /// </summary>
    public float ClampVertical(float cameraY, float halfHeight)
    {
        float max = m_LimitTop ? m_TopLimitY - halfHeight : float.PositiveInfinity;
        float min = m_LimitBottom ? m_BottomLimitY + halfHeight : float.NegativeInfinity;
        if (min > max) return (m_TopLimitY + m_BottomLimitY) * 0.5f;
        return Mathf.Clamp(cameraY, min, max);
    }

    /// <summary>
    /// 指定の位置がこの区間の中にあるか（リスポーン直後のカメラ切り替えで使う）
    /// </summary>
    public bool Contains(Vector3 position)
    {
        return GetComponent<BoxCollider2D>().OverlapPoint(position);
    }

    private void Start()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (m_PlayerCamera == null) m_PlayerCamera = FindObjectOfType<PlayerCamera>();
        if (m_PlayerCamera == null) return;

        m_InsideCount++;
        m_PlayerCamera.EnterArea(this);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || m_PlayerCamera == null) return;

        m_InsideCount = Mathf.Max(0, m_InsideCount - 1);
        if (m_InsideCount == 0) m_PlayerCamera.ExitArea(this);
    }

    private void OnDisable()
    {
        m_InsideCount = 0;
        if (m_PlayerCamera != null) m_PlayerCamera.ExitArea(this);
    }

    //シーンビューで範囲（黄）と実際に映る範囲（水色, 16:9）を表示
    private void OnDrawGizmos()
    {
        var box = GetComponent<BoxCollider2D>();
        if (box != null)
        {
            Gizmos.color = new Color(1f, 0.9f, 0f, 0.8f);
            Vector3 center = transform.TransformPoint(box.offset);
            Vector3 size = Vector3.Scale(box.size, transform.lossyScale);
            Gizmos.DrawWireCube(center, size);
        }

        // メインカメラの設定から、区間内で地面（z=0）に映る範囲を計算
        // （停止中はカメラが別の位置にあるので、距離は PlayerCamera のオフセットから求める）
        Camera cam = Camera.main;
        if (cam == null) return;
        var playerCamera = cam.GetComponent<PlayerCamera>();
        float distance = playerCamera != null && playerCamera.m_Player != null
            ? Mathf.Abs(playerCamera.m_Player.position.z + playerCamera.offset.z)
            : Mathf.Abs(cam.transform.position.z);
        float halfHeight = cam.orthographic
            ? cam.orthographicSize
            : distance * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        halfHeight *= m_ZoomOut;

        Vector3 point = m_CameraPoint != null ? m_CameraPoint.position : transform.position;
        float viewY = ClampVertical(point.y, halfHeight);
        float halfWidth = halfHeight * 16f / 9f;
        Gizmos.color = new Color(0f, 0.9f, 1f, 0.9f);
        Gizmos.DrawWireCube(new Vector3(point.x, viewY, 0f),
            new Vector3(halfWidth * 2f, halfHeight * 2f, 0f));

        // 画面上端・下端の制限（赤い線）
        float left = box != null ? box.bounds.min.x : point.x - halfWidth;
        float right = box != null ? box.bounds.max.x : point.x + halfWidth;
        Gizmos.color = Color.red;
        if (m_LimitTop) Gizmos.DrawLine(new Vector3(left, m_TopLimitY, 0f), new Vector3(right, m_TopLimitY, 0f));
        if (m_LimitBottom) Gizmos.DrawLine(new Vector3(left, m_BottomLimitY, 0f), new Vector3(right, m_BottomLimitY, 0f));
    }
}
