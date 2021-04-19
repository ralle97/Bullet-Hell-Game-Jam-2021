using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameMaster gm;
    private PlayerStats stats;
    private ObjectPooler objectPooler;

    private Animator animator;
    private Camera mainCamera;

    [HideInInspector]
    public float horizontal;
    [HideInInspector]
    public float vertical;

    private Rigidbody2D rigidBody;

    [SerializeField]
    private bool isInvincible = false;
    private float invincibleTimer;
    private bool shotCooldown = false;
    private float shotTimer;

    private Vector2 lookDir = new Vector2(0, -1);
    private Vector2 mousePos;

    public float projectileSpeed = 300f;

    private FirePoint firePoint;
    private float firePointOffset;

    public GameObject deathEffect;

    [SerializeField]
    private HPIndicatorUI hpBar;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameMaster.instance;

        stats = PlayerStats.instance;
        stats.Health = stats.maxHealth;

        objectPooler = ObjectPooler.instance;

        mainCamera = Camera.main;

        animator = GetComponent<Animator>();

        rigidBody = GetComponent<Rigidbody2D>();
        if (rigidBody == null)
        {
            Debug.LogError("No rigidBody in Player");
        }

        firePoint = GetComponentInChildren<FirePoint>();
        if (firePoint == null)
        {
            Debug.LogError("No firePoint in Player");
        }

        firePointOffset = Vector2.Distance((Vector2)firePoint.transform.position, (Vector2)transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.upgradeMenuOpened || gm.isGameOver || gm.isPaused)
        {
            return;
        }

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDir.Set(move.x, move.y);
            lookDir.Normalize();
        }

        animator.SetFloat("Look X", lookDir.x);
        animator.SetFloat("Look Y", lookDir.y);
        animator.SetFloat("Speed", move.magnitude);
        
        // TODO: Enable after testing
        /*
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;

            if (invincibleTimer <= 0.0f)
            {
                animator.SetBool("Hit", false);
                isInvincible = false;
            }
        }
        */
        if (shotCooldown)
        {
            shotTimer -= Time.deltaTime;

            if (shotTimer <= 0.0f)
            {
                shotCooldown = false;
            }
        }

        if (Input.GetButton("Fire1") && !shotCooldown && !gm.isPaused)
        {
            ChangeMousePos();
            ChangeFirePointPos();

            if (!stats.isTriangleAttack)
            {
                LaunchProjectile();
            }
            else
            {
                TriangleProjectile(3, 30f);
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidBody.position;
        position.x += horizontal * stats.speed * Time.fixedDeltaTime / Time.timeScale;
        position.y += vertical * stats.speed * Time.fixedDeltaTime / Time.timeScale;

        rigidBody.MovePosition(position);
    }

    private void ChangeMousePos()
    {
        float mousePosX = mainCamera.ScreenToWorldPoint(Input.mousePosition).x;
        float mousePosY = mainCamera.ScreenToWorldPoint(Input.mousePosition).y;

        mousePos = new Vector2(mousePosX, mousePosY);
    }

    private void ChangeFirePointPos()
    {
        float angle = Mathf.Atan2(mousePos.y - transform.position.y,
                                mousePos.x - transform.position.x);

        float firePointX = Mathf.Cos(angle) * firePointOffset;
        float firePointY = Mathf.Sin(angle) * firePointOffset;

        firePoint.ChangePosition(new Vector2(firePointX, firePointY));
    }

    private void LaunchProjectile()
    {
        Vector2 firepoint = firePoint.transform.position;

        GameObject projectileObject = objectPooler.SpawnFromPool("PlayerProjectile");

        if (projectileObject != null)
        {
            SetPosRotActivate(projectileObject, firepoint, Quaternion.identity);

            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.Launch((mousePos - firepoint).normalized, projectileSpeed);

            shotTimer = 1 / stats.fireRate;
            shotCooldown = true;
        }
    }

    private void TriangleProjectile(int count, float angle)
    {
        Vector2[] positions = new Vector2[count];
        Vector2 startPosition = (mousePos - (Vector2)this.transform.position).normalized * firePointOffset;

        float startAngle = Mathf.Atan2(startPosition.y, startPosition.x);
        float fractionAngleRad = angle / count * Mathf.Deg2Rad;

        for (int i = 0; i < count; i++)
        {
            float offsetAngleRad = (i - count / 2) * fractionAngleRad;

            float posX = Mathf.Cos((startAngle + offsetAngleRad)) * firePointOffset;
            float posY = Mathf.Sin((startAngle + offsetAngleRad)) * firePointOffset;

            positions[i] = new Vector2(transform.position.x + posX, transform.position.y + posY);
        }

        for (int i = 0; i < positions.Length; i++)
        {
            GameObject projectileObject = objectPooler.SpawnFromPool("PlayerProjectile");

            if (projectileObject != null)
            {
                SetPosRotActivate(projectileObject, positions[i], Quaternion.identity);

                Projectile projectile = projectileObject.GetComponent<Projectile>();

                projectile.Launch((positions[i] - (Vector2)transform.position).normalized, projectileSpeed);
            }
        }

        shotTimer = 1f / stats.fireRate;
        shotCooldown = true;
    }

    private void SetPosRotActivate(GameObject obj, Vector2 pos, Quaternion rot)
    {
        obj.transform.position = pos;
        obj.transform.rotation = rot;
        obj.SetActive(true);
    }

    public void DamagePlayer(int damage)
    {
        if (!isInvincible)
        {
            stats.Health -= damage;

            hpBar.SetHealth(stats.Health, stats.maxHealth);

            if (stats.Health <= 0)
            {
                GameMaster.KillPlayer(this);
            }
            else
            {
                CameraShake.instance.Shake(2f, 0.15f);
                isInvincible = true;
                invincibleTimer = stats.timeInvincible;
                animator.SetBool("Hit", true);
            }
        }
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }
}
