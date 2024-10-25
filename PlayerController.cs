using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public Camera cam;
    public Vector3 cameraOffset;
    public float smoothSpeed = 0.125f;
    public float zoomSpeed = 2f;
    public float minZoom = 5f;
    public float maxZoom = 20f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 mousePos;
    private Vector3 velocity = Vector3.zero;
    
    private Weapon[] weapons;
    private int currentWeaponIndex;
    private static readonly Vector3 mainWeaponPoint = new (0.1f, -0.2f, 0);
    private static readonly Vector3 otherWeaponPoint = new (-0.2f, -0.35f, 0);
    private static readonly Quaternion otherWeaponRot = Quaternion.Euler(0f, 0f, -35f);

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentWeaponIndex = 0;

        weapons = new Weapon[2];
        weapons[0] = WeaponFactory.Instance.CreateWeapon(WeaponType.Gatling, transform);
        weapons[1] = WeaponFactory.Instance.CreateWeapon(WeaponType.Sword, transform);

        EquipWeapon(currentWeaponIndex);
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        
        if (Input.GetMouseButtonDown(0))
        {
            weapons[currentWeaponIndex].StartShoot();
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            weapons[currentWeaponIndex].StopShoot();
        }
        
        HandleWeaponSwitch();
        HandleZoom();
        HandleWeaponAim();
    }
    
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
        Vector3 desiredPosition = transform.position + cameraOffset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(cam.transform.position, desiredPosition, ref velocity, smoothSpeed);
        cam.transform.position = smoothedPosition;
    }

    private void FastMoveWeapon(Weapon weapon, Vector3 targetPosition, Quaternion targetRotation)
    {
        var _transform = weapon.transform;
        _transform.localPosition = targetPosition;
        _transform.localRotation = targetRotation;
    }

    private IEnumerator MoveWeapon(Weapon weapon, Vector3 targetPosition, Quaternion targetRotation, float duration)
    {
        Vector3 initialPosition = weapon.transform.localPosition;
        Quaternion initialRotation = weapon.transform.localRotation;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            weapon.transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            weapon.transform.localRotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        weapon.transform.localPosition = targetPosition;
        weapon.transform.localRotation = targetRotation;
    }

    private void EquipWeapon(int weaponIndex)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].isActive = false;
            weapons[i].SpriteRenderer.sortingOrder = 2;
            if (i == weaponIndex)
                continue;
            FastMoveWeapon(weapons[i], otherWeaponPoint, otherWeaponRot);
        }

        Quaternion targetQuaternion;
        if (weapons[weaponIndex].type == WeaponType.Sword)
        {
            targetQuaternion = Quaternion.Euler(0, 0, 60);
        }
        else
        {
            targetQuaternion = Quaternion.identity;
        }
        FastMoveWeapon(weapons[weaponIndex], mainWeaponPoint, targetQuaternion);
        weapons[weaponIndex].isActive = true;
        weapons[weaponIndex].SpriteRenderer.sortingOrder = 7;
    }

    private void HandleWeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeaponIndex = 0;
            EquipWeapon(currentWeaponIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeaponIndex = 1;
            EquipWeapon(currentWeaponIndex);
        }
    }

    void HandleZoom()
    {
        float scrollData = Input.GetAxis("Mouse ScrollWheel");
        cam.orthographicSize -= scrollData * zoomSpeed;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
    }

    void HandleWeaponAim()
    {
        Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        bool isFacingLeft = mousePosition.x < transform.position.x;
        weapons[currentWeaponIndex].isFacingLeft = isFacingLeft;
        transform.localScale = new Vector3(isFacingLeft ? -1 : 1, 1, 1);
        
        if (!weapons[currentWeaponIndex].isTakeControl)
        {
            Vector2 lookDir = (Vector2)mousePosition - (Vector2)weapons[currentWeaponIndex].transform.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            weapons[currentWeaponIndex].transform.rotation = Quaternion.Euler(0f, 0f, isFacingLeft ? angle - 180 : angle);
        }
    }
}
