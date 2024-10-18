using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f;    // 敌人的移动速度
    public int health = 3;           // 敌人的初始血量
    public Action OnEnemyDestroyed;  // 销毁事件

    void Update()
    {
        // 敌人向左移动
        transform.Translate(Vector2.left * (moveSpeed * Time.deltaTime));

        // 如果敌人移出屏幕，可以选择销毁敌人
        if (transform.position.x < -10f) // 这里的 -10f 根据场景设置调整
        {
            Destroy(gameObject);
        }
    }

    // 击中时掉血的函数
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    // 敌人死亡处理
    private void Die()
    {
        // 当敌人被消灭时触发事件
        Destroy(gameObject);
    }

    public void OnDestroy()
    {
        if (OnEnemyDestroyed != null)
        {
            OnEnemyDestroyed.Invoke();
        }
    }
}
