using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    public int health = 3;
    public Action OnEnemyDestroyed;

    void Update()
    {
        transform.Translate(Vector2.left * (moveSpeed * Time.deltaTime));
        
        if (transform.position.x < -10f)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
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
