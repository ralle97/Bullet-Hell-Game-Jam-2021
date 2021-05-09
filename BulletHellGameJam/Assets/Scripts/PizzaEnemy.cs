using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaEnemy : Enemy
{
    private float shotTimer = 2f;
    private float firePointOffset = 0.4f;

    private readonly int triangleShotCount = 5;
    private readonly float triangleTotalAngle = 60;

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
                TriangleAttack(triangleShotCount, triangleTotalAngle, "PizzaProjectile");
                waitToShoot = false;
            }
        }
    }

    private void TriangleAttack(int count, float angle, string projectileTag)
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
