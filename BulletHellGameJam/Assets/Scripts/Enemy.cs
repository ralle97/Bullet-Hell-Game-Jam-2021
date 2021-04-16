﻿using System.Collections;
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
        public float fireRate = 2f;
        public float projectileForce = 500f;

        public float speed = 2f;
        public float movementRange = 5f;

        public void Init()
        {
            currentHealth = maxHealth;
        }
    }

    private ObjectPooler objectPooler;

    public EnemyStats stats = new EnemyStats();

    public EnemyType enemyType;

    public PlayerController player;

    private Rigidbody2D rigidBody;
    private Animator animator;

    private float shotTimer;

    private float firePointOffset = 0.4f;

    public bool shoot = false;
    private bool waitToShoot;

    private Vector2 lookDir = new Vector2(0, -1);

    private int allAroundShots = 18;
    private int triangleShotCount = 5;
    private float triangleTotalAngle = 45;

    private void OnEnable()
    {
        stats.Init();
        player = FindObjectOfType<PlayerController>();
        shotTimer = stats.fireRate;
    }

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        objectPooler = ObjectPooler.instance;

        if (enemyType == EnemyType.DONUT)
        {
            firePointOffset *= 2;
        }
    }

    private void Update()
    {
        // TODO: Attacks

        shotTimer -= Time.deltaTime;

        if (shotTimer <= 0f && player != null)
        {
            if (enemyType == EnemyType.ICECREAM)
            {
                // TODO: BigProjectile that explodes in small ones
            }

            else if (enemyType == EnemyType.PIZZA)
            {
                // TODO: Triangle attack
                if (!waitToShoot && !shoot)
                {
                    animator.SetTrigger("Shoot");
                    waitToShoot = true;
                }
                else if (waitToShoot && shoot)
                {
                    TriangleAttack(triangleShotCount, triangleTotalAngle);
                    waitToShoot = false;
                }
            }

            else if (enemyType == EnemyType.DONUT)
            {
                // TODO: All around attack
                if (!waitToShoot && !shoot)
                {
                    animator.SetTrigger("Shoot");
                    waitToShoot = true;
                }
                else if (waitToShoot && shoot)
                {
                    AllAroundAttack(allAroundShots);
                    waitToShoot = false;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 movementDirection = Vector2.zero;
        if (player != null)
        {
            movementDirection = (player.transform.position - transform.position).normalized;
        }
        else
        {
            return;
        }

        if (!Mathf.Approximately(movementDirection.x, 0.0f) || !Mathf.Approximately(movementDirection.y, 0.0f))
        {
            lookDir.Set(movementDirection.x, movementDirection.y);
            lookDir.Normalize();
        }

        if (animator != null)
        {
            animator.SetFloat("Look X", lookDir.x);
            animator.SetFloat("Look Y", lookDir.y);
            Debug.Log(lookDir.y);
            animator.SetFloat("Speed", movementDirection.magnitude);
        }

        if ((player.transform.position - transform.position).magnitude > stats.movementRange || enemyType == EnemyType.BURGER)
        {
            Vector2 position = rigidBody.position;
            position.x += movementDirection.x * stats.speed * Time.fixedDeltaTime;
            position.y += movementDirection.y * stats.speed * Time.fixedDeltaTime;

            rigidBody.MovePosition(position);
        }
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
            }
        }
    }

    //==================== ATACKS ====================

    private void SetPosRotActivate(GameObject obj, Vector2 pos, Quaternion rot)
    {
        obj.transform.position = pos;
        obj.transform.rotation = rot;
        obj.SetActive(true);
    }

    public void TriangleAttack(int count, float angle)
    {
        if (player == null)
        {
            return;
        }

        Vector2[] positions = new Vector2[count];
        Vector2 startPosition = (player.transform.position - this.transform.position).normalized * firePointOffset;

        float startAngle = Mathf.Atan2(startPosition.y, startPosition.x);
        float fractionAngleRad = angle / count * Mathf.Deg2Rad;

        for (int i = 0; i < count; i++)
        {
            float offsetX = Random.Range(0.9f, 1.1f);
            float offsetY = Random.Range(0.9f, 1.1f);

            float offsetAngleRad = (i - count / 2) * fractionAngleRad;

            float posX = Mathf.Cos((startAngle + offsetAngleRad) * offsetX) * firePointOffset;
            float posY = Mathf.Sin((startAngle + offsetAngleRad) * offsetY) * firePointOffset;

            positions[i] = new Vector2(transform.position.x + posX, transform.position.y + posY);
        }

        Debug.Log(positions.Length);

        for (int i = 0; i < positions.Length; i++)
        {
            GameObject projectileObject = objectPooler.SpawnFromPool("PizzaProjectile");

            if (projectileObject != null)
            {
                SetPosRotActivate(projectileObject, positions[i], Quaternion.identity);

                Projectile projectile = projectileObject.GetComponent<Projectile>();
                projectile.owner = this;

                projectile.Launch((positions[i] - (Vector2)transform.position).normalized, stats.projectileForce);
            }
        }

        shotTimer = 1f / stats.fireRate;
    }

    public void AllAroundAttack(int count)
    {
        Vector2[] positions = new Vector2[count];

        for (int i = 0; i < count; i++)
        {
            float angle = i * (360 / count) * Mathf.Deg2Rad;

            float posX = Mathf.Cos(angle) * firePointOffset;
            float posY = Mathf.Sin(angle) * firePointOffset;

            positions[i] = new Vector2(transform.position.x + posX, transform.position.y + posY);
        }

        Debug.Log(positions.Length);

        for (int i = 0; i < positions.Length; i++)
        {
            GameObject projectileObject = objectPooler.SpawnFromPool("DonutProjectile");

            if (projectileObject != null)
            {
                SetPosRotActivate(projectileObject, positions[i], Quaternion.identity);

                Projectile projectile = projectileObject.GetComponent<Projectile>();
                projectile.owner = this;

                projectile.Launch((positions[i] - (Vector2)transform.position).normalized, stats.projectileForce);
            }
        }

        shotTimer = 1f / stats.fireRate;
    }
}
