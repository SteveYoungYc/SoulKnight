using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;       // 移动速度
    public Camera cam;                  // 主摄像机
    public Transform gunPoint;          // 枪口位置
    public Vector3 cameraOffset;        // 相机与玩家之间的偏移
    public float smoothSpeed = 0.125f;  // 相机平滑跟随速度
    public float zoomSpeed = 2f;        // 缩放速度
    public float minZoom = 5f;          // 最小缩放
    public float maxZoom = 20f;         // 最大缩放

    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 mousePos;
    private Vector3 velocity = Vector3.zero;  // 用于SmoothDamp的速度缓存

    public GameObject bulletPrefab;    // 子弹的预制体
    [SerializeField]
    private float bulletSpeed = 10f;
    [SerializeField]
    public float fireRate = 0.1f;      // 连发的射击间隔

    private bool isShooting = false;   // 是否在射击状态

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 处理WASD输入
        movement.x = Input.GetAxisRaw("Horizontal"); // 获取水平方向输入
        movement.y = Input.GetAxisRaw("Vertical");   // 获取垂直方向输入

        // 获取鼠标位置
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        // 检测左键按下并保持连发
        if (Input.GetMouseButtonDown(0) && !isShooting)  // 检测左键按下，且没有在射击
        {
            StartShooting();
        }

        if (Input.GetMouseButtonUp(0) && isShooting)  // 检测左键松开
        {
            StopShooting();
        }

        // 调用缩放函数，处理鼠标滚轮输入
        HandleZoom();
    }

    private void StartShooting()
    {
        isShooting = true;
        InvokeRepeating(nameof(Shoot), 0f, fireRate);  // 立即开始射击，并每隔fireRate秒连发
    }

    private void StopShooting()
    {
        isShooting = false;
        CancelInvoke(nameof(Shoot));  // 停止连发
    }

    private void Shoot()
    {
        // 从枪口位置实例化子弹
        GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);

        // 获取子弹的 Rigidbody2D，并使其向前运动
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.AddForce(gunPoint.right * bulletSpeed, ForceMode2D.Impulse);
    }

    void FixedUpdate()
    {
        // 角色移动
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));

        // 只让枪口朝向鼠标位置
        Vector2 lookDir = mousePos - (Vector2)gunPoint.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        gunPoint.rotation = Quaternion.Euler(0f, 0f, angle);

        // 获取相机的目标位置（玩家位置 + 偏移）
        Vector3 desiredPosition = transform.position + cameraOffset;

        // 使用SmoothDamp使相机位置平滑移动到目标位置
        Vector3 smoothedPosition = Vector3.SmoothDamp(cam.transform.position, desiredPosition, ref velocity, smoothSpeed);

        // 更新相机位置
        cam.transform.position = smoothedPosition;
    }

    // 处理鼠标滚轮缩放相机的视野
    void HandleZoom()
    {
        float scrollData = Input.GetAxis("Mouse ScrollWheel"); // 获取鼠标滚轮输入

        // 调整相机的orthographicSize，实现缩放
        cam.orthographicSize -= scrollData * zoomSpeed;
        
        // 限制缩放范围
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
    }
}
