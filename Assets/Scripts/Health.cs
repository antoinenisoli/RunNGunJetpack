using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Health
{
    [SerializeField] float currentHealth;
    [SerializeField] float maxHealth = 50;
    public bool isDead => CurrentHealth <= 0;
    public float value => CurrentHealth / MaxHealth;

    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            if (value < 0)
                value = 0;

            if (value > maxHealth)
                value = maxHealth;

            currentHealth = value;
        }
    }

    public float MaxHealth { get => maxHealth; set => maxHealth = value; }

    public void Initialize()
    {
        CurrentHealth = MaxHealth;
    }

    public void ModifyValue(float amount)
    {
        CurrentHealth += amount;
    }
}
