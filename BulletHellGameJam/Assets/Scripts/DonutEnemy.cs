using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonutEnemy : Enemy
{
    private float shotTimer = 2f;
    private float firePointOffset = 0.8f;

    private readonly int allAroundShots = 24;

    // Update is called once per frame
    void Update()
    {
        shotTimer -= Time.deltaTime;

        if (shotTimer <= 0f && player != null)
        {
            if (!waitToShoot && !shoot)
            {
                animator.SetTrigger("Shoot");
                waitToShoot = true;
            }
            else if (waitToShoot && shoot)
            {
                AllAroundAttack(allAroundShots, "DonutProjectile");
                waitToShoot = false;
            }
        }
    }

    private void AllAroundAttack(int count, string projectileTag)
    {
        Vector2[] positions = new Vector2[count];

        float angleBetweenTwoProjectiles = 360 / count;
        float angleOffset = Random.Range(-angleBetweenTwoProjectiles, angleBetweenTwoProjectiles);

        for (int i = 0; i < count; i++)
        {
            float angle = i * angleBetweenTwoProjectiles * Mathf.Deg2Rad;

            float posX = Mathf.Cos(angle + angleOffset) * firePointOffset;
            float posY = Mathf.Sin(angle + angleOffset) * firePointOffset;

            positions[i] = new Vector2(transform.position.x + posX, transform.position.y + posY);
        }

        for (int i = 0; i < positions.Length; i++)
        {
            GameObject projectileObject = objectPooler.SpawnFromPool(projectileTag);

            if (projectileObject != null)
            {
                SetPosRotActivate(projectileObject, positions[i], Quaternion.identity);

                Projectile projectile = projectileObject.GetComponent<Projectile>();
                projectile.owner = this;

                projectile.Launch((positions[i] - (Vector2)transform.position).normalized, stats.projectileForce);
            }
        }

        float shotTimeOffset = Random.Range(0.9f, 1.1f);

        shotTimer = 1f / stats.fireRate * shotTimeOffset;
    }
}
