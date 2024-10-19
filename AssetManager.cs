using UnityEngine;
using System.Collections.Generic;

public class AssetManager : MonoBehaviour
{
    private Dictionary<string, Sprite> weaponSprites;

    public static AssetManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        LoadWeaponSprites();
    }

    private void LoadWeaponSprites()
    {
        weaponSprites = new Dictionary<string, Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Weapons");

        foreach (Sprite sprite in sprites)
        {
            Debug.Log(sprite.name);
            weaponSprites[sprite.name] = sprite;
        }

        Debug.Log($"Loaded {weaponSprites.Count} weapon sprites.");
    }

    public Sprite GetWeaponSprite(string weaponName)
    {
        if (weaponSprites.TryGetValue(weaponName, out Sprite sprite))
        {
            return sprite;
        }

        Debug.LogWarning($"Weapon sprite {weaponName} not found.");
        return null;
    }
}
