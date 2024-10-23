using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public Sprite bulletSprite;
    public float bulletSpeed = 10f;
    public float fireRate = 1f;
    public bool isShooting;

    public virtual void StartShoot()
    {
        isShooting = true;
    }

    public virtual void StopShoot()
    {
        isShooting = false;
    }
}
