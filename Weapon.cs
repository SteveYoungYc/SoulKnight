using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float fireRate = 0.1f;

    public void Shoot()
    {
        var transform1 = transform;
        GameObject bullet = Instantiate(bulletPrefab, transform1.position, transform1.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.AddForce(transform.right * bulletSpeed, ForceMode2D.Impulse);
    }
}
