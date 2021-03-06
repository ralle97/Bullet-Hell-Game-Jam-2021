using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private ObjectPooler objectPooler;
    private AudioManager audioManager;
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

    [SerializeField]
    private string iceCreamProjectileExplosionSound = "IceCreamProjectileExplosion";

    [SerializeField]
    private string bossFireSound = "BossFire";

    [SerializeField]
    private string enemyFireSound = "EnemyFire";

    private bool isIceCreamProjectile = false;

    private void Awake()
    {
        audioManager = AudioManager.instance;
        objectPooler = ObjectPooler.instance;
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        timer = timeToLive;

        if (this.name.Equals("IceCreamProjectileBig") || this.name.Equals("IceCreamProjectileBig(Clone)"))
        {
            isIceCreamProjectile = true;
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0.0f)
        {
            if (isIceCreamProjectile)
            {
                AllAroundAttack(18, 0.275f, "IceCreamProjectileSmall");
            }
            
            this.gameObject.SetActive(false);
        }
    }

    public void Launch(Vector2 dir, float force)
    {
        projectileForce = force;
        if (owner != null)
        {
            projectileDamage = owner.stats.damage;
        }

        if (isOwnerBoss)
        {
            audioManager.PlaySound(bossFireSound);
        }
        else if (owner != null)
        {
            audioManager.PlaySound(enemyFireSound);
        }

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
            IEnemy enemy = collision.GetComponent<IEnemy>();
            enemy.TakeHit(Mathf.RoundToInt((1 - enemy.GetArmorStat()) * PlayerStats.instance.damage));
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

        if (isIceCreamProjectile && collision.CompareTag("Background"))
        {
            AllAroundAttack(18, 0.275f, "IceCreamProjectileSmall");
            this.gameObject.SetActive(false);
        }

        if (!isIceCreamProjectile)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void AllAroundAttack(int count, float firePointOffset, string projectileTag)
    {
        audioManager.PlaySound(iceCreamProjectileExplosionSound);

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
