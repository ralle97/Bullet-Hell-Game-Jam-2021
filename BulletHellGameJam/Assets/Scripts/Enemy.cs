using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyType { ICECREAM, PIZZA, BURGER, DONUT };

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

        public float shotCooldown = 3f;
        private float shotTimer;
        
        public float speed = 2f;

        public void Init()
        {
            currentHealth = maxHealth;
        }
    }

    public EnemyStats stats = new EnemyStats();

    public EnemyType enemyType;

    public PlayerController player;

    private Rigidbody2D rigidBody;
    private Animator animator;

    private Vector2 lookDir = new Vector2(0, -1);

    private void OnEnable()
    {
        stats.Init();
        player = FindObjectOfType<PlayerController>();
    }

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 movementDirection = Vector2.zero;
        if (player != null)
        {
            movementDirection = (player.transform.position - transform.position).normalized;
        }

        if (!Mathf.Approximately(movementDirection.x, 0.0f) || !Mathf.Approximately(movementDirection.y, 0.0f))
        {
            lookDir.Set(movementDirection.x, movementDirection.y);
            lookDir.Normalize();
        }

        animator.SetFloat("Look X", lookDir.x);
        animator.SetFloat("Look Y", lookDir.y);
        animator.SetFloat("Speed", movementDirection.magnitude);

        // Movement until fixed range

        Vector2 position = rigidBody.position;
        position.x += movementDirection.x * stats.speed * Time.fixedDeltaTime;
        position.y += movementDirection.y * stats.speed * Time.fixedDeltaTime;

        rigidBody.MovePosition(position);
    }

    public void DamageEnemy(int damage)
    {
        stats.CurrentHealth -= damage;

        if (stats.CurrentHealth <= 0)
        {
            GameMaster.KillEnemy(this);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.collider.GetComponent<PlayerController>();

            if (player != null && !player.IsInvincible())
            {
                player.DamagePlayer(stats.damage);
                Debug.Log(stats.damage);
            }
        }
    }
}
