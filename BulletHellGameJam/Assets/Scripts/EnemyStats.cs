using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyStats
{
    public int maxHealth = 100;

    private int currentHealth;
    public int CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = Mathf.Clamp(value, 0, maxHealth); }
    }

    public int damage = 40;
    public float speed = 2f;
    public float armorPct = 0f;
    
    [Space(10)]
    [Header("BOSS DOESN'T USE THOSE:")]
    
    public float fireRate = 2f;
    public float projectileForce = 500f;

    public float movementRange = 5f;

    public void Init()
    {
        currentHealth = maxHealth;
    }
}
