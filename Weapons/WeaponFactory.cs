using System;
using UnityEngine;

public enum WeaponType
{
    Weapon03,
    Weapon04,
    TailWeapon
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
        
        SpriteRenderer spriteRenderer = weaponObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = weaponSprite;
        spriteRenderer.sortingOrder = 6;

        weapon.bulletTypes = bulletTypes;
    }

    // Method to create weapons
    public Weapon CreateWeapon(WeaponType type, Transform parent)
    {
        switch (type)
        {
            case WeaponType.Weapon03:
            {
                var weapon = new GameObject("Weapon03").AddComponent<BasicGunWeapon>();
                var weaponSprite = AssetManager.Instance.GetSprite("Weapons", "Weapon 3");
                BulletType[] bulletTypes = new[] { BulletType.Bullet03 };
                SetAttribute(weapon, weapon.gameObject, parent, weaponSprite, bulletTypes);
                return weapon;
            }
            case WeaponType.Weapon04:
            {
                var weapon = new GameObject("Weapon04").AddComponent<BasicGunWeapon>();
                var weaponSprite = AssetManager.Instance.GetSprite("Weapons", "Weapon 4");
                BulletType[] bulletTypes = new[] { BulletType.Bullet04 };
                SetAttribute(weapon, weapon.gameObject, parent, weaponSprite, bulletTypes);
                return weapon;
            }
            case WeaponType.TailWeapon:
            {
                var weapon = new GameObject("Tail").AddComponent<TailWeapon>();
                var weaponSprite = AssetManager.Instance.GetSprite("Weapons", "Weapon 0");
                BulletType[] bulletTypes = new[] { BulletType.Bullet04 };
                SetAttribute(weapon, weapon.gameObject, parent, weaponSprite, bulletTypes);
                return weapon;
            }
            default:
                throw new ArgumentException("Invalid weapon type.");
        }
    }
}
