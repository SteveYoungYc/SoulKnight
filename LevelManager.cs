using UnityEngine;
using UnityEngine.UI;  // 如果你想显示倒计时，可以使用UI元素
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    public Text countdownText;  // 用于显示倒计时的UI文字（可选）
    
    private int currentLevel = 1;      // 当前关卡
    private float levelInterval = 2f; // 关卡间隔时间
    
    void Start()
    {
        // 确保 enemySpawner 被正确赋值
        if (enemySpawner == null)
        {
            enemySpawner = FindObjectOfType<EnemySpawner>();  // 尝试自动查找场景中的 EnemySpawner
        }

        if (enemySpawner != null)
        {
            StartCoroutine(StartNextLevel());  // 开始游戏，启动第一关
        }
        else
        {
            Debug.LogError("EnemySpawner not found or not assigned!");
        }
    }

    // 启动下一关的协程
    private IEnumerator StartNextLevel()
    {
        while (true)  // 无限关卡循环
        {
            OnLevelStart();  // 关卡开始
            
            while (enemySpawner.enemies.Count == 0)  // 保证至少有一个敌人生成后才继续
            {
                yield return null;
            }
            
            while (enemySpawner.enemies.Count > 0)
            {
                yield return null;
            }
            
            OnLevelEnd();  // 关卡结束逻辑，比如统计分数或奖励等

            currentLevel++;  // 增加关卡数

            // 倒计时显示，如果不需要显示可以跳过
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
                countdownText.text = "";  // 清空倒计时
            }
        }
    }

    // 关卡开始时触发的事件
    private void OnLevelStart()
    {
        // 在这里添加每一关的启动逻辑，比如生成敌人、设置目标等
        Debug.Log("Level " + currentLevel + " has started!");
        // 举例: 生成敌人、生成道具、设定关卡目标等
        enemySpawner.Init();
        InvokeRepeating(nameof(InvokeSpawnEnemy), 0f, enemySpawner.spawnInterval);
    }
    
    private void InvokeSpawnEnemy()
    {
        enemySpawner.SpawnEnemy();
        if (enemySpawner.generated >= enemySpawner.maxEnemies) // 检查列表中是否有超过最大数量的敌人
        {
            CancelInvoke(nameof(InvokeSpawnEnemy)); // 停止生成敌人
        }
    }

    // 关卡结束时触发的事件
    private void OnLevelEnd()
    {
        // 在这里处理关卡结束时的逻辑，比如结算分数、触发奖励等
        Debug.Log("Level " + currentLevel + " has ended!");
        // 举例: 清理敌人、保存数据等
    }
}
