using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public Sprite bulletSprite;
    public float bulletSpeed = 10f;
    public float fireRate = 0.1f;
    public bool isShooting;

    public abstract void Shoot();
}
