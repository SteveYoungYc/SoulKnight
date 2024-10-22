using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hit" + collision.gameObject.name);
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Hit");
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(50);
                Destroy(gameObject);
            }
        }
    }
}