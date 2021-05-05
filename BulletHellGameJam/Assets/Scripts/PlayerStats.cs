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

    public int maxHealth = 150;

    private int currentHealth;
    public int Health
    {
        get { return currentHealth; }
        set { currentHealth = Mathf.Clamp(value, 0, maxHealth); }
    }

    public int damage = 20;

    public float speed = 5f;
    public float fireRate = 4f;
    public float timeInvincible = 2f;

    [HideInInspector]
    public bool isTriangleAttack = false;
    
    public bool Shield
    {
        get;
        private set;
    }

    [SerializeField]
    private SpriteRenderer shieldEffectSprite;

    public void EnableShield()
    {
        shieldEffectSprite.gameObject.SetActive(true);
        Shield = true;
    }

    public void DisableShield()
    {
        shieldEffectSprite.gameObject.SetActive(false);
        Shield = false;
    }
}
