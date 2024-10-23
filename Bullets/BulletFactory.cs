﻿using System;
using UnityEngine;
using Object = UnityEngine.Object;

public enum BulletType
{
    Bullet03,
    Bullet04
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
        bulletGameObject.transform.localScale = new Vector3(1, 1, 1);
        return bulletGameObject;
    }

    private void SetAttribute(Bullet bullet, Transform parent, Sprite bulletSprite)
    {
        bullet.gameObject.transform.position = parent.position;
        bullet.gameObject.transform.rotation = parent.rotation;
        SpriteRenderer spriteRenderer = bullet.gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = bulletSprite;
        spriteRenderer.sortingOrder = 6;
    }
    
    public Bullet CreateBullet(BulletType type, Transform parent)
    {
        switch (type)
        {
            case BulletType.Bullet03:
            {
                Bullet bullet = CreateBulletGameObject("Prefabs/Bullet").GetComponent<Bullet>();
                SetAttribute(bullet, parent, AssetManager.Instance.GetSprite("Bullets", "Bullet 3"));
                return bullet;
            }
            case BulletType.Bullet04:
            {
                Bullet bullet = CreateBulletGameObject("Prefabs/Bullet").GetComponent<Bullet>();
                SetAttribute(bullet, parent, AssetManager.Instance.GetSprite("Bullets", "Bullet 4"));
                return bullet;
            }
            default:
                throw new ArgumentException("Invalid weapon type.");
        }
    }
}