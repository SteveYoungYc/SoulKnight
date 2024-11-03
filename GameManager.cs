using System;
using UnityEngine;

public enum GameMode
{
    Idle,
    Pause,
    Prepare,
    Fight
}

public class GameManager : MonoBehaviour
{
    public static GameMode mode;
    public static Camera cameraMain;
    public static AssetManager assetManager;
    public static EnemySpawner enemySpawner;
    public static LevelManager levelManager;
    public static UIManager uiManager;

    private void Awake()
    {
        mode = GameMode.Idle;
        cameraMain = Camera.main;
        if (cameraMain == null)
        {
            Debug.LogError("Main cam is not found!");
        }
        assetManager = gameObject.AddComponent<AssetManager>();
        enemySpawner = gameObject.AddComponent<EnemySpawner>();
        levelManager = gameObject.AddComponent<LevelManager>();
        uiManager = gameObject.AddComponent<UIManager>();
    }
}
