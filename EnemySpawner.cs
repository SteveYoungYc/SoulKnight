using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 1f;
    public int maxEnemies = 5;
    public List<GameObject> enemies;
    public int generated;
    public bool startSpawn;
    private float interval;

    private void Start()
    {
        enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemy");
        enemies = new List<GameObject>();
        startSpawn = false;
        interval = 0f;
    }

    public void Update()
    {
        if (!startSpawn)
        {
            return;
        }
        
        if (generated == maxEnemies)
        {
            startSpawn = false;
            return;
        }
        
        interval += Time.deltaTime;
        if (interval >= spawnInterval)
        {
            SpawnEnemy();
            interval = 0f;
        }
    }

    private void SpawnEnemy()
    {
        Vector2 spawnPosition = new Vector2(10f, Random.Range(-3f, 3f));
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        enemies.Add(newEnemy);
        generated++;
        newEnemy.GetComponent<Enemy>().OnEnemyDestroyed += () => HandleEnemyDestroyed(newEnemy);
    }

    private void HandleEnemyDestroyed(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }

    public void StartSpawnEnemy()
    {
        startSpawn = true;
        interval = 0f;
        generated = 0;
        SpawnEnemy();
    }

    public bool isFinished()
    {
        return enemies.Count == 0 && generated == maxEnemies;
    }
}
