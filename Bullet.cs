using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnBecameInvisible()
    {
        Destroy(gameObject); // 子弹离开屏幕视野时销毁
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) // 确保敌人有设置 Tag 为 "Enemy"
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(50); // 调用敌人掉血的方法
                Destroy(gameObject); // 子弹击中敌人后销毁
            }
        }
    }
}