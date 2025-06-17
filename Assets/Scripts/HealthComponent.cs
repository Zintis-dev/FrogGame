using UnityEngine;
using System;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    private bool isDead = false;

    public event Action OnDeath;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log($"{gameObject.name} health after damage: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log($"{gameObject.name} is dying.");
        OnDeath?.Invoke();
    }

    public float GetHealth() => currentHealth;
    public float GetMaxHealth() => maxHealth;

    public void SetMaxHealth(float value)
    {
        maxHealth = value;
        currentHealth = maxHealth;
        isDead = false;
    }
}
