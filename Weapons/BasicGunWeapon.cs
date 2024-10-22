
using UnityEngine;

public class BasicGunWeapon : Weapon
{
    public override void Shoot()
    {
        var transform1 = transform;
        GameObject bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");;
        GameObject bullet = Instantiate(bulletPrefab, transform1.position, transform1.rotation);
        bullet.transform.localScale = new Vector3(1, 1, 1);
        SpriteRenderer spriteRenderer = bullet.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = bulletSprite;
        spriteRenderer.sortingOrder = 6;
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.AddForce(transform.right * bulletSpeed, ForceMode2D.Impulse);
    }
}
