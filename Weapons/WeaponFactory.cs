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
        Sprite weaponSprite, BulletType[] bulletTypes)
    {
        weaponObject.transform.SetParent(parent);
        weapon.bulletTypes = bulletTypes;
        
        SpriteRenderer spriteRenderer = weaponObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = weaponSprite;
        spriteRenderer.sortingOrder = 6;

        BoxCollider2D boxCollider2D = weaponObject.AddComponent<BoxCollider2D>();
        boxCollider2D.size = spriteRenderer.sprite.bounds.size;
        boxCollider2D.isTrigger = true;
    }

    // Method to create weapons
    public Weapon CreateWeapon(WeaponType type, Transform parent)
    {
        Weapon weapon;
        switch (type)
        {
            case WeaponType.Weapon03:
            {
                weapon = new GameObject("Weapon03").AddComponent<BasicGunWeapon>();
                var weaponSprite = AssetManager.Instance.GetSprite("Weapons", "Weapon 3");
                BulletType[] bulletTypes = new[] { BulletType.BulletTail };
                SetAttribute(weapon, weapon.gameObject, parent, weaponSprite, bulletTypes);
                break;
            }
            case WeaponType.Weapon04:
            {
                weapon = new GameObject("Weapon04").AddComponent<BasicGunWeapon>();
                var weaponSprite = AssetManager.Instance.GetSprite("Weapons", "Gatling");
                BulletType[] bulletTypes = new[] { BulletType.Bullet04 };
                SetAttribute(weapon, weapon.gameObject, parent, weaponSprite, bulletTypes);
                break;
            }
            case WeaponType.TailWeapon:
            {
                weapon = new GameObject("TailWeapon").AddComponent<TailWeapon>();
                var weaponSprite = AssetManager.Instance.GetSprite("Weapons", "Knife");
                BulletType[] bulletTypes = new[] { BulletType.BulletTail };
                SetAttribute(weapon, weapon.gameObject, parent, weaponSprite, bulletTypes);
                break;
            }
            case WeaponType.SwordWeapon:
            {
                weapon = new GameObject("SwordWeapon").AddComponent<SwordWeapon>();
                var weaponSprite = AssetManager.Instance.GetSprite("Weapons", "Knife");
                SetAttribute(weapon, weapon.gameObject, parent, weaponSprite, null);
                break;
            }
            default:
                throw new ArgumentException("Invalid weapon type.");
        }
        weapon.type = type;
        return weapon;
    }
}
