using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public float bulletSpeed = 10f;
    public float fireRate = 1f;
    public bool isShooting;
    public BulletType[] bulletTypes;

    public virtual void StartShoot()
    {
        isShooting = true;
    }

    public virtual void StopShoot()
    {
        isShooting = false;
    }
}
