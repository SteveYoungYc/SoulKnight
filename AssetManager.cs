using UnityEngine;
using System.Collections.Generic;

public class AssetManager : MonoBehaviour
{
    private Dictionary<string, Sprite> weaponSprites;
    private Dictionary<string, Dictionary<string, Sprite>> sprites;

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
        LoadSprites("Weapons");
    }

    private void LoadSprites(string category)
    {
        // Initialize the outer dictionary if it's null
        if (sprites == null)
        {
            sprites = new Dictionary<string, Dictionary<string, Sprite>>();
        }

        // Check if the category already exists in the dictionary
        if (!sprites.ContainsKey(category))
        {
            sprites[category] = new Dictionary<string, Sprite>();
        }

        // Load sprites from the specified path
        Sprite[] loadedSprites = Resources.LoadAll<Sprite>(category);

        // Check if the loaded sprites are valid
        if (loadedSprites == null || loadedSprites.Length == 0)
        {
            Debug.LogError(
                $"No sprites found at the path: {category}. Please check the path and ensure that sprites are present.");
            return;
        }

        // Add the sprites to the dictionary under the specified category
        foreach (Sprite sprite in loadedSprites)
        {
            if (sprites[category].ContainsKey(sprite.name))
            {
                Debug.LogWarning(
                    $"Duplicate sprite found in category '{category}': {sprite.name}. Skipping the duplicate.");
                continue;
            }

            sprites[category][sprite.name] = sprite;
        }

        Debug.Log($"Successfully loaded {sprites[category].Count} sprites into the category '{category}'.");
    }

    public Sprite GetSprite(string category, string spriteName)
    {
        // Check if the category exists in the outer dictionary
        if (!sprites.ContainsKey(category))
        {
            Debug.LogError($"Category '{category}' not found.");
            return null;
        }

        // Check if the sprite exists in the inner dictionary for the given category
        if (!sprites[category].ContainsKey(spriteName))
        {
            Debug.LogError($"Sprite '{spriteName}' not found in category '{category}'.");
            return null;
        }

        // Return the sprite if found
        return sprites[category][spriteName];
    }
}
