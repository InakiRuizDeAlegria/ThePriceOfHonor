using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float armor = 0;
    public float currentHealth;
    public bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void Damage(float damage)
    {
        if (isDead) return;

        if (armor != 0) 
        {
            currentHealth -= damage * armor;
        } else
        {
            currentHealth -= damage;
        }

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
