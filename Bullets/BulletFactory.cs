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
        CircleCollider2D circleCollider = bullet.gameObject.GetComponent<CircleCollider2D>();
        spriteRenderer.sprite = bulletSprite;
        spriteRenderer.sortingOrder = 3;

        Vector3 offset = parent.TransformDirection(new Vector3(spriteRenderer.bounds.extents.x, 0, 0));
        bullet.gameObject.transform.position = parent.position + offset * parent.localScale.x;
        bullet.gameObject.transform.rotation = parent.rotation;
        
        float spriteSize = Mathf.Min(spriteRenderer.sprite.bounds.size.x, spriteRenderer.sprite.bounds.size.y) / 2f;
        circleCollider.radius = spriteSize;
    }
    
    public Bullet CreateBullet(BulletType type, Transform parent)
    {
        Bullet bullet = CreateBulletGameObject("Prefabs/Bullet").GetComponent<Bullet>();;
        Sprite bulletSprite;
        switch (type)
        {
            case BulletType.Bullet03:
            {
                bulletSprite = AssetManager.Instance.GetSprite("Bullets", "Bullet 3");
                break;
            }
            case BulletType.Bullet04:
            {
                bulletSprite = AssetManager.Instance.GetSprite("Bullets", "Bullet 4");
                break;
            }
            case BulletType.Gatling:
            {
                bullet.transform.localScale = new Vector3(0.3f, 0.3f, 1);
                bulletSprite = AssetManager.Instance.GetSprite("Bullets", "BulletGatling");
                break;
            }
            case BulletType.Tail0:
            {
                bulletSprite = AssetManager.Instance.GetSprite("Bullets", "Tail0");
                break;
            }
            case BulletType.Tail1:
            {
                bulletSprite = AssetManager.Instance.GetSprite("Bullets", "Tail1");
                break;
            }
            default:
                throw new ArgumentException("Invalid weapon type.");
        }
        SetAttribute(bullet, parent, bulletSprite);
        return bullet;
    }
}
