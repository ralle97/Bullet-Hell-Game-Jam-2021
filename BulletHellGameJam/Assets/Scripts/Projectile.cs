using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private ObjectPooler objectPooler;

    private Rigidbody2D rigidBody;

    public Enemy owner;
    public float timeToLive = 10.0f;
    private float timer;

    private void Awake()
    {
        objectPooler = ObjectPooler.instance;
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        timer = timeToLive;
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
            player.DamagePlayer(owner.stats.damage);
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

        for (int i = 0; i < count; i++)
        {
            float angle = i * (360 / count) * Mathf.Deg2Rad;

            float posX = Mathf.Cos(angle) * firePointOffset;
            float posY = Mathf.Sin(angle) * firePointOffset;

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
                projectile.owner = this.owner;

                projectile.Launch((positions[i] - (Vector2)transform.position).normalized, owner.stats.projectileForce * 2);
            }
        }
    }
}
