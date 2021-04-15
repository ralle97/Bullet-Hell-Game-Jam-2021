using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public class EnemyStats
    {
        public int maxHealth = 100;

        private int currentHealth;
        public int CurrentHealth
        {
            get { return currentHealth; }
            set { currentHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public void Init()
        {
            currentHealth = maxHealth;
        }
    }

    public EnemyStats stats = new EnemyStats();

    private void OnEnable()
    {
        stats.Init();
    }

    public void DamageEnemy(int damage)
    {
        stats.CurrentHealth -= damage;

        if (stats.CurrentHealth <= 0)
        {
            GameMaster.KillEnemy(this);
        }
    }
}
