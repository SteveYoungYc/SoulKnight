using System;
using UnityEngine;

public enum WeaponType
{
    Weapon03,
    Weapon04,
    TailWeapon,
    SwordWeapon
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
        spriteRenderer.sortingOrder = 6;

        BoxCollider2D boxCollider2D = weaponObject.AddComponent<BoxCollider2D>();
        boxCollider2D.size = spriteRenderer.sprite.bounds.size;
        boxCollider2D.isTrigger = true;
        
        GameObject gunPointObj = new GameObject("GunPoint");
        gunPointObj.transform.SetParent(weaponObject.transform);
        gunPointObj.transform.localPosition = gunPointPosition;
        weapon.gunPoint = gunPointObj.transform;
    }

    // Method to create weapons
    public Weapon CreateWeapon(WeaponType type, Transform parent)
    {
        Weapon weapon;
        Sprite weaponSprite;
        BulletType[] bulletTypes;
        Vector3 gunPointPosition;
        switch (type)
        {
            case WeaponType.Weapon03:
            {
                weapon = new GameObject("Weapon03").AddComponent<BasicGunWeapon>();
                weaponSprite = AssetManager.Instance.GetSprite("Weapons", "Weapon 3");
                bulletTypes = new[] { BulletType.BulletTail };
                gunPointPosition = Vector3.up;
                break;
            }
            case WeaponType.Weapon04:
            {
                weapon = new GameObject("Weapon04").AddComponent<BasicGunWeapon>();
                weaponSprite = AssetManager.Instance.GetSprite("Weapons", "Gatling");
                bulletTypes = new[] { BulletType.Bullet04 };
                gunPointPosition = new Vector3(1.5f, 0.05f, 0);
                break;
            }
            case WeaponType.TailWeapon:
            {
                weapon = new GameObject("TailWeapon").AddComponent<TailWeapon>();
                weaponSprite = AssetManager.Instance.GetSprite("Weapons", "Knife");
                bulletTypes = new[] { BulletType.BulletTail };
                gunPointPosition = Vector3.up;
                break;
            }
            case WeaponType.SwordWeapon:
            {
                weapon = new GameObject("SwordWeapon").AddComponent<SwordWeapon>();
                weaponSprite = AssetManager.Instance.GetSprite("Weapons", "Knife");
                bulletTypes = null;
                gunPointPosition = Vector3.up;
                break;
            }
            default:
                throw new ArgumentException("Invalid weapon type.");
        }
        weapon.type = type;
        SetAttribute(weapon, weapon.gameObject, parent, gunPointPosition, weaponSprite, bulletTypes);
        return weapon;
    }
}
