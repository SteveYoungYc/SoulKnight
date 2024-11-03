using System;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    public int health = 3;
    public Action OnEnemyDestroyed;

    private int maxHealth;
    private SlideBar healthBar;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        maxHealth = health;
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        Bounds bounds = spriteRenderer.bounds;
        Vector3 offset = new Vector3(-bounds.size.x / 2, bounds.size.y / 2 + 0.2f, 0);
        healthBar = UIFactory.Instance.CreateSlideBar(UIType.EnemyHealthBar, transform, offset);
        healthBar.rectTransform.sizeDelta = new Vector2(spriteRenderer.bounds.size.x, 0.2f);
        healthBar.width = bounds.size.x;
    }

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
        UpdateHealthBar();

        if (health <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {
        healthBar.ratio = (float)health / maxHealth;
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (OnEnemyDestroyed != null)
        {
            OnEnemyDestroyed.Invoke();
        }

        if (healthBar != null)
        {
            Destroy(healthBar.gameObject);
        }
    }
}
