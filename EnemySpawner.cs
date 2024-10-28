using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 1f;
    public int maxEnemies = 10;
    public List<GameObject> enemies;
    public int generated;

    private void Start()
    {
        enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemy");
        enemies = new List<GameObject>();
    }

    public void Init()
    {
        generated = 0;
    }

    public void SpawnEnemy()
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
}
