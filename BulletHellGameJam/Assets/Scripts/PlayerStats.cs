using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    #region Singleton

    public static PlayerStats instance;

    private void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            currentHealth = maxHealth;
        }
        else
        {
            instance = this;
            //DontDestroyOnLoad(this);
        }
    }

    #endregion

    public int maxHealth = 100;

    private int currentHealth;
    public int Health
    {
        get { return currentHealth; }
        set { currentHealth = Mathf.Clamp(value, 0, maxHealth); }
    }

    public int damage = 10;

    public float speed = 5f;
    public float fireRate = 5f;
    public float timeInvincible = 2f;
}
