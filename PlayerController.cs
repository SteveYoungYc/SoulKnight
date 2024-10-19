﻿using System.Collections;
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

    public GameObject weaponPrefab;
    private Weapon[] weapons;
    private int currentWeaponIndex;
    private Vector3 mainWeaponPoint;
    private Vector3 otherWeaponPoint;
    private Quaternion otherWeaponRot;

    private bool isShooting = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainWeaponPoint = new Vector3(0.3f, -0.2f, 0);
        otherWeaponPoint = new Vector3(-0.2f, -0.35f, 0);
        otherWeaponRot = Quaternion.Euler(0f, 0f, -35f);
        currentWeaponIndex = 0;

        weapons = new Weapon[2];
        CreateWeapon(0, mainWeaponPoint, AssetManager.Instance.GetSprite("Weapons", "Weapon 3"));
        CreateWeapon(1, otherWeaponPoint, AssetManager.Instance.GetSprite("Weapons", "Weapon 4"));

        EquipWeapon(currentWeaponIndex);
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && !isShooting)
        {
            StartShooting();
        }

        if (Input.GetMouseButtonUp(0) && isShooting)
        {
            StopShooting();
        }

        HandleWeaponSwitch();
        HandleZoom();
    }

    private void StartShooting()
    {
        isShooting = true;
        InvokeRepeating(nameof(Shoot), 0f, weapons[currentWeaponIndex].fireRate);
    }

    private void StopShooting()
    {
        isShooting = false;
        CancelInvoke(nameof(Shoot));
    }

    private void Shoot()
    {
        weapons[currentWeaponIndex].Shoot();
    }

    private void CreateWeapon(int index, Vector3 localPosition, Sprite weaponSprite)
    {
        GameObject weaponObj = Instantiate(weaponPrefab, transform);
        weaponObj.transform.localPosition = localPosition;

        if (index != 0)
        {
            weaponObj.transform.rotation = otherWeaponRot;
        }

        SpriteRenderer spriteRenderer = weaponObj.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = weaponSprite;
        }

        weapons[index] = weaponObj.GetComponent<Weapon>();
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
        foreach (Weapon weapon in weapons)
        {
            StartCoroutine(MoveWeapon(weapon, otherWeaponPoint, otherWeaponRot, 0.3f));
        }

        StartCoroutine(MoveWeapon(weapons[weaponIndex], mainWeaponPoint, Quaternion.identity, 0.3f));
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

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));

        Vector2 lookDir = mousePos - (Vector2)weapons[currentWeaponIndex].transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        weapons[currentWeaponIndex].transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Vector3 desiredPosition = transform.position + cameraOffset;
        Vector3 smoothedPosition =
            Vector3.SmoothDamp(cam.transform.position, desiredPosition, ref velocity, smoothSpeed);
        cam.transform.position = smoothedPosition;
    }

    void HandleZoom()
    {
        float scrollData = Input.GetAxis("Mouse ScrollWheel");
        cam.orthographicSize -= scrollData * zoomSpeed;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
    }
}
