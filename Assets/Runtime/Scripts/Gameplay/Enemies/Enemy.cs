using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamageable
{
    private EnemyPoolManager manager;
    public float maxHealth = 100f;
    public Slider healthSlider;
    private float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
        currentHealth = maxHealth;
    }
    
    public void SetManager(EnemyPoolManager mngr)
    {
        manager = mngr;
    }

    private void Die()
    {
        manager.DisableEnemy(this);
    }

    public void Damage(float amount)
    {
        currentHealth -= amount;
        healthSlider.value = currentHealth;

        if(currentHealth <= 0)
        {
            Die();
        }
    }
}