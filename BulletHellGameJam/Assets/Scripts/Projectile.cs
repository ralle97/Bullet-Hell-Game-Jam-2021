using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private ObjectPooler objectPooler;

    private Rigidbody2D rigidBody;

    [HideInInspector]
    public Enemy owner;
    private int projectileDamage;
    public float timeToLive = 10.0f;
    private float timer;

    private float projectileForce;

    [HideInInspector]
    public bool isOwnerBoss = false;
    [HideInInspector]
    public int bossProjectileDamage;

    private void Awake()
    {
        objectPooler = ObjectPooler.instance;
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        timer = timeToLive;

        if (owner != null)
        {
            projectileDamage = owner.stats.damage;
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0.0f)
        {
            if (this.name.Equals("IceCreamProjectileBig") || this.name.Equals("IceCreamProjectileBig(Clone)"))
            {
                AllAroundAttack(18, 0.275f, "IceCreamProjectileSmall");
            }
            
            this.gameObject.SetActive(false);
        }
    }

    public void Launch(Vector2 dir, float force)
    {
        projectileForce = force;

        rigidBody.AddForce(dir * force);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Powerup"))
        {
            return;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            // TODO: Change to SetActive(false) in case you do enemies via object pooling
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.DamageEnemy(PlayerStats.instance.damage);
        }

        else if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (!isOwnerBoss)
            {
                player.DamagePlayer(projectileDamage);
            }
            else
            {
                player.DamagePlayer(bossProjectileDamage);
            }
        }

        else if (collision.gameObject.CompareTag("BossEnemy"))
        {
            BossEnemy boss = collision.GetComponent<BossEnemy>();
            boss.DamageBoss(PlayerStats.instance.damage);
        }

        if ((this.name.Equals("IceCreamProjectileBig") || this.name.Equals("IceCreamProjectileBig(Clone)")) && collision.CompareTag("Background"))
        {
            AllAroundAttack(18, 0.275f, "IceCreamProjectileSmall");
            this.gameObject.SetActive(false);
        }

        if (!(this.name.Equals("IceCreamProjectileBig") || this.name.Equals("IceCreamProjectileBig(Clone)")))
        {
            this.gameObject.SetActive(false);
        }
    }

    private void AllAroundAttack(int count, float firePointOffset, string projectileTag)
    {
        Vector2[] positions = new Vector2[count];

        float angleOffset = Random.Range(-10f, 10f);

        for (int i = 0; i < count; i++)
        {
            float angle = i * (360 / count) * Mathf.Deg2Rad;

            float posX = Mathf.Cos(angle + angleOffset) * firePointOffset;
            float posY = Mathf.Sin(angle + angleOffset) * firePointOffset;

            positions[i] = new Vector2(transform.position.x + posX, transform.position.y + posY);
        }

        for (int i = 0; i < positions.Length; i++)
        {
            GameObject projectileObject = objectPooler.SpawnFromPool(projectileTag);

            if (projectileObject != null)
            {
                projectileObject.transform.position = positions[i];
                projectileObject.transform.rotation = Quaternion.identity;
                projectileObject.SetActive(true);

                Projectile projectile = projectileObject.GetComponent<Projectile>();
                projectile.projectileDamage = this.projectileDamage;

                projectile.Launch((positions[i] - (Vector2)transform.position).normalized, projectileForce * 2);
            }
        }
    }
}
