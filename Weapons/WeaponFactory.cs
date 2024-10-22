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

    private readonly Vector3 mainWeaponPoint;
    private readonly Vector3 otherWeaponPoint;
    private readonly Quaternion otherWeaponRot;

    // Private constructor for Singleton
    private WeaponFactory()
    {
        mainWeaponPoint = new Vector3(0.3f, -0.2f, 0);
        otherWeaponPoint = new Vector3(-0.2f, -0.35f, 0);
        otherWeaponRot = Quaternion.Euler(0f, 0f, -35f);
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
    
    private void SetWeaponTransform(Weapon weapon ,GameObject weaponObject, Transform parent, 
        Vector3 position, Sprite weaponSprite, Sprite bulletSprite)
    {
        weaponObject.transform.SetParent(parent);
        weaponObject.transform.position = position;
        
        SpriteRenderer spriteRenderer = weaponObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = weaponSprite;
        spriteRenderer.sortingOrder = 6;

        weapon.bulletSprite = bulletSprite;
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
                var bulletSprite = AssetManager.Instance.GetSprite("Bullets", "Bullet 3");
                SetWeaponTransform(weapon, weapon.gameObject, parent, mainWeaponPoint, weaponSprite, bulletSprite);
                return weapon;
            }
            case WeaponType.Weapon04:
            {
                var weapon = new GameObject("Weapon04").AddComponent<BasicGunWeapon>();
                var weaponSprite = AssetManager.Instance.GetSprite("Weapons", "Weapon 4");
                var bulletSprite = AssetManager.Instance.GetSprite("Bullets", "Bullet 4");
                SetWeaponTransform(weapon, weapon.gameObject, parent, mainWeaponPoint, weaponSprite, bulletSprite);
                return weapon;
            }
            case WeaponType.TailWeapon:
            {
                var weapon = new GameObject("Tail").AddComponent<TailWeapon>();
                var weaponSprite = AssetManager.Instance.GetSprite("Weapons", "Weapon 0");
                var bulletSprite = AssetManager.Instance.GetSprite("Bullets", "Bullet 4");
                SetWeaponTransform(weapon, weapon.gameObject, parent, mainWeaponPoint, weaponSprite, bulletSprite);
                return weapon;
            }
            default:
                throw new ArgumentException("Invalid weapon type.");
        }
    }
}
