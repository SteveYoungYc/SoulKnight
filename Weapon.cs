using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Sprite bulletSprite;
    public float bulletSpeed = 10f;
    public float fireRate = 0.1f;

    public void Shoot()
    {
        var transform1 = transform;
        // GameObject bullet = new GameObject(bulletSprite.name)
        // {
        //     transform =
        //     {
        //         position = transform1.position,
        //         rotation = transform1.rotation
        //     }
        // };
        //
        // SpriteRenderer spriteRenderer = bullet.AddComponent<SpriteRenderer>();
        // spriteRenderer.sprite = bulletSprite;
        // spriteRenderer.sortingOrder = 6;
        //
        //
        // bullet.AddComponent<Bullet>();
        //
        // var circleCollider2D = bullet.AddComponent<CircleCollider2D>();
        //
        // Rigidbody2D bulletRb = bullet.AddComponent<Rigidbody2D>();
        // bulletRb.gravityScale = 0;
        // bulletRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        // bulletRb.AddForce(transform.right * bulletSpeed, ForceMode2D.Impulse);

        GameObject bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");;
        if (bulletPrefab == null)
        {
            Debug.LogError("Prefabs/Bullet not found");
            return;
        }
        GameObject bullet = Instantiate(bulletPrefab, transform1.position, transform1.rotation);
        bullet.transform.localScale = new Vector3(1, 1, 1);
        SpriteRenderer spriteRenderer = bullet.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = bulletSprite;
        spriteRenderer.sortingOrder = 6;
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.AddForce(transform.right * bulletSpeed, ForceMode2D.Impulse);
    }

    public void Start()
    {
        
    }
    
    public void Update()
    {
        
    }
}
