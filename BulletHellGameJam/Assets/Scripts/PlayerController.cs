using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerStats stats;
    private ObjectPooler objectPooler;

    private Animator animator;
    private Camera mainCamera;

    private float horizontal;
    private float vertical;

    private Rigidbody2D rigidBody;

    private bool isInvincible = false;
    private float invincibleTimer;
    private bool shotCooldown = false;
    private float shotTimer;

    private Vector2 lookDir = new Vector2(0, -1);
    private Vector2 mousePos;

    public float projectileSpeed = 300f;

    private FirePoint firePoint;
    private float firePointOffset;

    private void Awake()
    {
        stats = PlayerStats.instance;
        stats.Health = stats.maxHealth;

        objectPooler = ObjectPooler.instance;

        mainCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
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

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;

            if (invincibleTimer <= 0.0f)
            {
                isInvincible = false;
            }
        }

        if (shotCooldown)
        {
            shotTimer -= Time.deltaTime;

            if (shotTimer <= 0.0f)
            {
                shotCooldown = false;
            }
        }

        if (Input.GetButton("Fire1") && !shotCooldown)
        {
            ChangeMousePos();
            ChangeFirePointPos();
            LaunchProjectile();
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidBody.position;
        position.x += horizontal * stats.speed * Time.deltaTime;
        position.y += vertical * stats.speed * Time.deltaTime;

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

    private void SetPosRotActivate(GameObject obj, Vector2 pos, Quaternion rot)
    {
        obj.transform.position = pos;
        obj.transform.rotation = rot;
        obj.SetActive(true);
    }
}
