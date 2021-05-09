using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCreamEnemy : Enemy
{
    private float shotTimer = 2f;
    private float firePointOffset = 0.4f;

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
                IceCreamAttack("IceCreamProjectileBig");
                waitToShoot = false;
            }
        }
    }

    private void IceCreamAttack(string projectileTag)
    {
        Vector2 startPosition = (player.transform.position - this.transform.position).normalized * firePointOffset;

        float offsetX = Random.Range(0.9f, 1.1f);
        float offsetY = Random.Range(0.9f, 1.1f);

        float angle = Mathf.Atan2(startPosition.y, startPosition.x);

        float posX = Mathf.Cos(angle * offsetX) * firePointOffset;
        float posY = Mathf.Sin(angle * offsetY) * firePointOffset;

        Vector2 position = new Vector2(transform.position.x + posX, transform.position.y + posY);

        GameObject projectileObject = objectPooler.SpawnFromPool(projectileTag);

        if (projectileObject != null)
        {
            SetPosRotActivate(projectileObject, position, Quaternion.identity);

            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.owner = this;

            projectile.Launch((position - (Vector2)transform.position).normalized, stats.projectileForce);
        }

        float shotTimeOffset = Random.Range(0.9f, 1.1f);

        shotTimer = 1f / stats.fireRate * shotTimeOffset;
    }
}
