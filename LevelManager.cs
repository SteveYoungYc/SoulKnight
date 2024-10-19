using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    public Text countdownText;
    
    private int currentLevel = 1;
    private float levelInterval = 2f;
    
    void Start()
    {
        if (enemySpawner == null)
        {
            enemySpawner = FindObjectOfType<EnemySpawner>();
        }

        if (enemySpawner != null)
        {
            StartCoroutine(StartNextLevel());
        }
        else
        {
            Debug.LogError("EnemySpawner not found or not assigned!");
        }
    }
    
    private IEnumerator StartNextLevel()
    {
        while (true)
        {
            OnLevelStart();
            
            while (enemySpawner.enemies.Count == 0)
            {
                yield return null;
            }
            
            while (enemySpawner.enemies.Count > 0)
            {
                yield return null;
            }
            
            OnLevelEnd();
            currentLevel++;
            
            float countdown = levelInterval;
            while (countdown > 0)
            {
                if (countdownText != null)
                {
                    countdownText.text = "Next Level in: " + countdown.ToString("F0") + "s";
                }
                yield return new WaitForSeconds(1f);
                countdown--;
            }

            if (countdownText != null)
            {
                countdownText.text = "";
            }
        }
    }
    
    private void OnLevelStart()
    {
        Debug.Log("Level " + currentLevel + " has started!");
        enemySpawner.Init();
        InvokeRepeating(nameof(InvokeSpawnEnemy), 0f, enemySpawner.spawnInterval);
    }
    
    private void InvokeSpawnEnemy()
    {
        enemySpawner.SpawnEnemy();
        if (enemySpawner.generated >= enemySpawner.maxEnemies)
        {
            CancelInvoke(nameof(InvokeSpawnEnemy));
        }
    }
    
    private void OnLevelEnd()
    {
        Debug.Log("Level " + currentLevel + " has ended!");
    }
}
