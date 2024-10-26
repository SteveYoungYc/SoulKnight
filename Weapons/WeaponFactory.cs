using System;
using UnityEngine;

public enum WeaponType
{
    Weapon03,
    Weapon04,
    Gatling,
    Tail,
    Sword
}

public class WeaponFactory
{
    private static WeaponFactory _instance;

    // Private constructor for Singleton
    private WeaponFactory()
    {
    }

    // Public static property to access the instance
    public static WeaponFactory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new WeaponFactory();
            }
            return _instance;
        }
    }
    
    private void SetAttribute(Weapon weapon ,GameObject weaponObject, Transform parent, 
        Vector3 gunPointPosition, Sprite weaponSprite, BulletType[] bulletTypes)
    {
        weaponObject.transform.SetParent(parent);
        weapon.bulletTypes = bulletTypes;
        
        SpriteRenderer spriteRenderer = weaponObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = weaponSprite;
        spriteRenderer.sortingOrder = 7;
        weapon.SpriteRenderer = spriteRenderer;

        BoxCollider2D boxCollider2D = weaponObject.AddComponent<BoxCollider2D>();
        boxCollider2D.size = spriteRenderer.sprite.bounds.size;
        boxCollider2D.isTrigger = true;
        weapon.boxCollider2D = boxCollider2D;
        
        GameObject gunPointObj = new GameObject("GunPoint");
        gunPointObj.transform.SetParent(weaponObject.transform);
        gunPointObj.transform.localPosition = gunPointPosition;
        weapon.gunPoint = gunPointObj.transform;
    }

    // Method to create weapons
    public Weapon CreateWeapon(WeaponType type, Transform parent)
    {
        GameObject weaponObject = new GameObject(type.ToString());
        Weapon weapon;
        Sprite weaponSprite;
        BulletType[] bulletTypes;
        Vector3 gunPointPosition = Vector3.zero;
        switch (type)
        {
            case WeaponType.Weapon03:
            {
                weapon = weaponObject.AddComponent<BasicGunWeapon>();
                weaponSprite = AssetManager.Instance.GetSprite("Weapons", "Weapon 3");
                bulletTypes = new[] { BulletType.Bullet03 };
                break;
            }
            case WeaponType.Weapon04:
            {
                weapon = weaponObject.AddComponent<BasicGunWeapon>();
                weaponSprite = AssetManager.Instance.GetSprite("Weapons", "Weapon 4");
                bulletTypes = new[] { BulletType.Bullet04 };
                break;
            }
            case WeaponType.Gatling:
            {
                weapon = weaponObject.AddComponent<BasicGunWeapon>();
                weaponSprite = AssetManager.Instance.GetSprite("Weapons", "Gatling");
                bulletTypes = new[] { BulletType.Gatling };
                gunPointPosition = new Vector3(1.5f, 0.05f, 0);
                break;
            }
            case WeaponType.Tail:
            {
                weapon = weaponObject.AddComponent<TailWeapon>();
                weaponSprite = AssetManager.Instance.GetSprite("Weapons", "WeaponTail_0");
                bulletTypes = new[] { BulletType.Tail0, BulletType.Tail1 };
                gunPointPosition = new Vector3(0.5f, 0, 0);
                break;
            }
            case WeaponType.Sword:
            {
                weapon = weaponObject.AddComponent<SwordWeapon>();
                weaponSprite = AssetManager.Instance.GetSprite("Weapons", "Knife");
                bulletTypes = null;
                break;
            }
            default:
                throw new ArgumentException("Invalid weapon type.");
        }
        weapon.type = type;
        SetAttribute(weapon, weaponObject, parent, gunPointPosition, weaponSprite, bulletTypes);
        return weapon;
    }
}
