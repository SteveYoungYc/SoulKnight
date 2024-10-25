using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public bool isShooting;
    public bool isTakeControl;
    public float bulletSpeed = 10f;
    public float fireRate = 1f;
    public int damage;
    public WeaponType type;
    public BulletType[] bulletTypes;
    public Transform gunPoint;

    public virtual void StartShoot()
    {
        isShooting = true;
    }

    public virtual void StopShoot()
    {
        isShooting = false;
    }
}
