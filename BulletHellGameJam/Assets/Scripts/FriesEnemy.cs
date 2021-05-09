using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriesEnemy : Enemy
{
    private float shotTimer = 2f;
    private float firePointOffset = 0.4f;

    private readonly int friesLineShots = 5;

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
                StartCoroutine(LineAttack(friesLineShots, "FriesProjectile"));
                waitToShoot = false;
            }
        }
    }

    IEnumerator LineAttack(int count, string projectileTag)
    {
        shotTimer = 1f / stats.fireRate;

        for (int i = 0; i < count; i++)
        {
            if (player == null)
            {
                yield break;
            }

            Vector2 startPosition = (player.transform.position - this.transform.position).normalized * firePointOffset;

            float offsetX = Random.Range(0.97f, 1.03f);
            float offsetY = Random.Range(0.97f, 1.03f);

            float angle = Mathf.Atan2(startPosition.y, startPosition.x);

            float posX = Mathf.Cos(angle * offsetX) * firePointOffset;
            float posY = Mathf.Sin(angle * offsetY) * firePointOffset;

            Vector2 position = new Vector2(transform.position.x + posX, transform.position.y + posY);

            GameObject projectileObject = objectPooler.SpawnFromPool(projectileTag);

            if (projectileObject != null)
            {
                SetPosRotActivate(projectileObject, position, Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg + 90f));

                Projectile projectile = projectileObject.GetComponent<Projectile>();
                projectile.owner = this;

                projectile.Launch((position - (Vector2)transform.position).normalized, stats.projectileForce);
            }

            yield return new WaitForSeconds((1 / stats.fireRate) / (count * 2));
        }
    }
}
