using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static AssetManager assetManager;
    public static EnemySpawner enemySpawner;
    public static LevelManager levelManager;

    private void Awake()
    {
        assetManager = gameObject.AddComponent<AssetManager>();
        enemySpawner = gameObject.AddComponent<EnemySpawner>();
        levelManager = gameObject.AddComponent<LevelManager>();
    }
}
