using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void Damage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Destroy(gameObject);
    }
}
