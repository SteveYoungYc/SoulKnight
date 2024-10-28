using System;
using UnityEngine;
using Object = UnityEngine.Object;

public enum BulletType
{
    Bullet03,
    Bullet04,
    Gatling,
    Tail0,
    Tail1
}

public class BulletFactory
{
    private static BulletFactory _instance;

    // Private constructor for Singleton
    private BulletFactory()
    {
    }

    // Public static property to access the instance
    public static BulletFactory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BulletFactory();
            }
            return _instance;
        }
    }

    private GameObject CreateBulletGameObject(string path)
    {
        GameObject bulletPrefab = Resources.Load<GameObject>(path);
        GameObject bulletGameObject = Object.Instantiate(bulletPrefab);
        return bulletGameObject;
    }

    private void SetAttribute(Bullet bullet, Transform parent, Sprite bulletSprite)
    {

        SpriteRenderer spriteRenderer = bullet.gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = bulletSprite;
        spriteRenderer.sortingOrder = 3;
        
        CircleCollider2D circleCollider = bullet.gameObject.GetComponent<CircleCollider2D>();
        float spriteSize = Mathf.Min(spriteRenderer.sprite.bounds.size.x, spriteRenderer.sprite.bounds.size.y) / 2f;
        circleCollider.radius = spriteSize;
        
        Rigidbody2D rigidbody2D = bullet.gameObject.GetComponent<Rigidbody2D>();
        bullet.rb2d = rigidbody2D;
    }
    
    public Bullet CreateBullet(BulletType type, Transform parent)
    {
        Bullet bullet = CreateBulletGameObject("Prefabs/Bullet").GetComponent<Bullet>();;
        Sprite bulletSprite;
        switch (type)
        {
            case BulletType.Bullet03:
            {
                bulletSprite = GameManager.assetManager.GetSprite("Bullets", "Bullet 3");
                break;
            }
            case BulletType.Bullet04:
            {
                bulletSprite = GameManager.assetManager.GetSprite("Bullets", "Bullet 4");
                break;
            }
            case BulletType.Gatling:
            {
                bullet.transform.localScale = new Vector3(0.3f, 0.3f, 1);
                bulletSprite = GameManager.assetManager.GetSprite("Bullets", "BulletGatling");
                break;
            }
            case BulletType.Tail0:
            {
                bulletSprite = GameManager.assetManager.GetSprite("Bullets", "Tail0");
                break;
            }
            case BulletType.Tail1:
            {
                bulletSprite = GameManager.assetManager.GetSprite("Bullets", "Tail1");
                break;
            }
            default:
                throw new ArgumentException("Invalid weapon type.");
        }
        SetAttribute(bullet, parent, bulletSprite);
        return bullet;
    }
}
