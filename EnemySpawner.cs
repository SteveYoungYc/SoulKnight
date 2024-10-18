using System.Collections.Generic; // 用于使用 List
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // 敌人的预制体
    public float spawnInterval = 2f; // 生成间隔时间
    public int maxEnemies = 10;      // 最大敌人数
    public List<GameObject> enemies; // 用于记录生成的敌人
    public int generated;

    private void Start()
    {
        enemies = new List<GameObject>(); // 初始化敌人列表
    }

    public void Init()
    {
        generated = 0;
    }

    public void SpawnEnemy()
    {
        Debug.Log("SpawnEnemy");
        Vector2 spawnPosition = new Vector2(10f, Random.Range(-3f, 3f)); // 随机生成位置
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity); // 实例化敌人
        enemies.Add(newEnemy); // 添加敌人到列表中
        generated++;

        // 注册敌人销毁事件，以便从列表中移除
        newEnemy.GetComponent<Enemy>().OnEnemyDestroyed += () => HandleEnemyDestroyed(newEnemy);
    }

    // 当敌人被销毁时调用的处理函数
    private void HandleEnemyDestroyed(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy); // 从列表中移除被销毁的敌人
        }
    }
}
