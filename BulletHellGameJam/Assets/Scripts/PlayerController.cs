using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Controls controls;

    private GameMaster gm;
    private PlayerStats stats;
    private ObjectPooler objectPooler;
    private AudioManager audioManager;

    private Animator animator;
    private Camera mainCamera;

    private Vector2 move;

    private Rigidbody2D rigidBody;

    [SerializeField]
    private bool isInvincible = false;
    private float invincibleTimer;
    private bool shotCooldown = false;
    private float shotTimer;

    private Vector2 lookDir = new Vector2(0, -1);
    private Vector2 mousePos = Vector2.zero;

    public float projectileSpeed = 300f;

    private FirePoint firePoint;
    private float firePointOffset;

    public GameObject deathEffect;

    [SerializeField]
    private HPIndicatorUI hpBar;

    [SerializeField]
    private string fireProjectileSound = "FireProjectile";

    [SerializeField]
    private string playerGruntSound = "PlayerGrunt";

    private bool gamepadSupport = false;
    private Vector2 rightStickPos = Vector2.zero;

    [SerializeField]
    private GameObject gamepadCrosshair;
    private float gamepadCrosshairOffset = 2.5f;

    private void Awake()
    {
        controls = new Controls();

        controls.Master.Move.performed += ctx =>
        {
            move = ctx.ReadValue<Vector2>();
        };
        controls.Master.Move.canceled += ctx => move = Vector2.zero;

        controls.Master.Aim.performed += ctx => rightStickPos = ctx.ReadValue<Vector2>();
        controls.Master.Aim.canceled += ctx => rightStickPos = Vector2.zero;
    }

    private void OnEnable()
    {
        controls.Master.Enable();
    }

    private void OnDisable()
    {
        controls.Master.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        gm = GameMaster.instance;
        audioManager = AudioManager.instance;

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
        if (!gamepadSupport && Gamepad.all.Count > 0)
        {
            gamepadSupport = true;
            gamepadCrosshair.SetActive(true);
            Cursor.SetCursor(null, gm.crosshairHotspot, CursorMode.Auto);
        }

        if (gamepadSupport && Gamepad.all.Count == 0)
        {
            Cursor.SetCursor(gm.crosshairTexture, gm.crosshairHotspot, CursorMode.Auto);
            gamepadCrosshair.SetActive(false);
            gamepadSupport = false;
        }

        if (gamepadSupport)
        {
            float angle = Mathf.Atan2(rightStickPos.y, rightStickPos.x);

            rightStickPos.x = Mathf.Cos(angle);
            rightStickPos.y = Mathf.Sin(angle);

            gamepadCrosshair.transform.position = (transform.position + (Vector3)rightStickPos) * gamepadCrosshairOffset;
        }

        if (gm.upgradeMenuOpened || gm.isGameOver || gm.isPaused)
        {
            move = Vector2.zero;

            animator.SetFloat("Look X", 0);
            animator.SetFloat("Look Y", 0);
            animator.SetFloat("Speed", 0);

            return;
        }
        
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
                animator.SetBool("Hit", false);
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

        if (controls.Master.Shoot.IsPressed() && !shotCooldown && !gm.isPaused && !gm.isGameOver && !gm.upgradeMenuOpened)
        {
            ChangeMousePos();
            ChangeFirePointPos();

            audioManager.PlaySound(fireProjectileSound);

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

        position.x += move.x * stats.speed * Time.fixedDeltaTime / Time.timeScale;
        position.y += move.y * stats.speed * Time.fixedDeltaTime / Time.timeScale;

        rigidBody.MovePosition(position);
    }

    private void ChangeMousePos()
    {
        if (!gamepadSupport)
        {
            float mousePosX = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()).x;
            float mousePosY = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()).y;

            mousePos = new Vector2(mousePosX, mousePosY);
        }
        else
        {
            mousePos = gamepadCrosshair.transform.position;
        }
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
        if (stats.Shield)
        {
            stats.DisableShield();
        }
        else if (!isInvincible)
        {
            stats.Health -= damage;

            hpBar.SetHealth(stats.Health, stats.maxHealth);

            audioManager.PlaySound(playerGruntSound);

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
