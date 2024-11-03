using System;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    public int health = 3;
    public Action OnEnemyDestroyed;

    private int maxHealth;
    private GameObject healthBar;
    private RectTransform healthBarTransform;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        maxHealth = health;
        
        healthBar = new GameObject("HealthBar");
        healthBar.transform.SetParent(GameManager.uiManager.worldCanvas.transform);
        
        Image healthImage = healthBar.AddComponent<Image>();
        healthImage.color = Color.red;
        
        healthBarTransform = healthBar.GetComponent<RectTransform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        float width = spriteRenderer.bounds.size.x;
        healthBarTransform.sizeDelta = new Vector2(width, 0.2f);
        healthBarTransform.pivot = new Vector2(0, 0.5f);

        UpdateHealthBarPosition();
    }

    void Update()
    {
        transform.Translate(Vector2.left * (moveSpeed * Time.deltaTime));
        
        if (transform.position.x < -10f)
        {
            Destroy(gameObject);
        }
        
        UpdateHealthBarPosition();
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
        if (healthBarTransform != null)
        {
            float healthRatio = (float)health / maxHealth;
            healthBarTransform.sizeDelta = new Vector2(spriteRenderer.bounds.size.x * healthRatio, healthBarTransform.sizeDelta.y);
        }
    }

    private void UpdateHealthBarPosition()
    {
        Vector3 offset = new Vector3(-spriteRenderer.bounds.size.x / 2, spriteRenderer.bounds.size.y / 2 + 0.2f, 0);
        healthBarTransform.position = transform.position + offset;
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
            Destroy(healthBar);
        }
    }
}
