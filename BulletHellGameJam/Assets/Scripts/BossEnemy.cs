using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossEnemy : MonoBehaviour
{
    [System.Serializable]
    public class Pattern
    {
        public string name;
        
        public float patternCooldown;
        public float patternFireRate;
        public float patternDuration = 10f;
        
        public string projectileName;
        public float projectileForce;
        public int projectileDamage;
    }

    [System.Serializable]
    public class BossStats
    {
        public int maxHealth = 2500;

        private int currentHealth;
        public int CurrentHealth
        {
            get { return currentHealth; }
            set { currentHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public int damage = 40;

        public float speed = 2f;

        public float armorPct = 0.25f;

        public void Init()
        {
            currentHealth = maxHealth;
        }
    }

    private GameMaster gm;

    public BossStats stats;

    [SerializeField]
    private HPIndicatorUI hpIndicator;

    [SerializeField]
    private Pattern[] patterns;
    private float[] patternCooldownTimers;
    private int currentPatternIndex;

    private ObjectPooler objectPooler;

    [HideInInspector]
    public PlayerController player;

    private Rigidbody2D rigidBody;
    private Animator animator;

    private float firePointOffset = 2.5f;
    
    [HideInInspector]
    public float patternChangeTimer;
    public bool shoot = false;
    private bool waitToShoot = false;

    private Vector2 lookDir = new Vector2(0, -1);

    // Triangle attack parameters
    [SerializeField]
    private int triangleShotCount = 5;
    [SerializeField]
    private float triangleTotalAngle = 60;

    // All around attack parameters
    [SerializeField]
    private int allAroundShots = 24;

    // Rotate circle attack parameters
    [SerializeField]
    private float rotateSpeed = 5f;
    [SerializeField]
    private int numOfFirepoints = 4;

    // Cone rotate attack parameters
    [SerializeField]
    private float totalAngleCoverage = 60f;

    public GameObject[] deathParticles;

    private void OnEnable()
    {
        stats.Init();
        player = FindObjectOfType<PlayerController>();

        if (hpIndicator != null)
        {
            hpIndicator.SetHealth(stats.CurrentHealth, stats.maxHealth);
        }

        patternChangeTimer = 0;
        currentPatternIndex = 0;

        patternCooldownTimers = new float[patterns.Length];
        for (int i = 0; i < patternCooldownTimers.Length; i++)
        {
            patternCooldownTimers[i] = 0f;
        }
    }

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        objectPooler = ObjectPooler.instance;
        
        gm = GameMaster.instance;
        gm.bossPatternChangeText.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (gm.isPaused || gm.isGameOver)
        {
            return;
        }

        for (int i = 0; i < patternCooldownTimers.Length; i++)
        {
            patternCooldownTimers[i] -= Time.deltaTime;
        }

        patternChangeTimer -= Time.deltaTime;

        gm.ChangeBossPatternText(patternChangeTimer);
        
        if (patternChangeTimer < 0f && player != null)
        {
            if (!waitToShoot)
            {
                while (true)
                {
                    currentPatternIndex = Random.Range(0, patterns.Length);

                    if (patternCooldownTimers[currentPatternIndex] <= 0f)
                    {
                        break;
                    }
                }
            }

            switch (currentPatternIndex)
            {
                case 0:
                    if (!waitToShoot && !shoot)
                    {
                        animator.SetTrigger("Shoot");
                        waitToShoot = true;
                    }
                    else if (waitToShoot && shoot)
                    {
                        StartCoroutine(ScatterAttack("IceCreamProjectileBig"));
                        waitToShoot = false;
                    }
                    break;
                
                case 1:
                    if (!waitToShoot && !shoot)
                    {
                        animator.SetTrigger("Shoot");
                        waitToShoot = true;
                    }
                    else if (waitToShoot && shoot)
                    {
                        StartCoroutine(TriangleAttack(triangleShotCount, triangleTotalAngle, "PizzaProjectile"));
                        waitToShoot = false;
                    }
                    break;

                case 2:
                    if (!waitToShoot && !shoot)
                    {
                        animator.SetTrigger("Shoot");
                        waitToShoot = true;
                    }
                    else if (waitToShoot && shoot)
                    {
                        StartCoroutine(AllAroundAttack(allAroundShots, "DonutProjectile"));
                        waitToShoot = false;
                    }
                    break;

                case 3:
                    if (!waitToShoot && !shoot)
                    {
                        animator.SetTrigger("Shoot");
                        waitToShoot = true;
                    }
                    else if (waitToShoot && shoot)
                    {
                        StartCoroutine(RotateAttack(numOfFirepoints, "BossProjectileBlack"));
                        waitToShoot = false;
                    }
                    break;

                case 4:
                    if (!waitToShoot && !shoot)
                    {
                        animator.SetTrigger("Shoot");
                        waitToShoot = true;
                    }
                    else if (waitToShoot && shoot)
                    {
                        StartCoroutine(ConeRotateAttack(totalAngleCoverage, "BossProjectileBlue"));
                        waitToShoot = false;
                    }
                    break;
            }
            
            if (!waitToShoot)
            {
                patternChangeTimer = patterns[currentPatternIndex].patternDuration;
            }
            
        }
    }

    private void FixedUpdate()
    {
        Vector2 temp;
        Vector2 movementDirection = Vector2.zero;

        if (player != null)
        {
            temp = (player.transform.position - transform.position);
            movementDirection = temp.normalized;
        }
        else
        {
            return;
        }

        if (!Mathf.Approximately(movementDirection.x, 0.0f) || !Mathf.Approximately(movementDirection.y, 0.0f))
        {
            lookDir.Set(movementDirection.x, movementDirection.y);
            lookDir.Normalize();
        }

        if (animator != null)
        {
            animator.SetFloat("Look X", lookDir.x);
            animator.SetFloat("Look Y", lookDir.y);
            animator.SetFloat("Speed", movementDirection.magnitude);
        }

        Vector2 position = rigidBody.position;

        position.x += movementDirection.x * stats.speed * Time.fixedDeltaTime;
        position.y += movementDirection.y * stats.speed * Time.fixedDeltaTime;

        rigidBody.MovePosition(position);
    }

    public void DamageBoss(int damage)
    {
        stats.CurrentHealth -= damage;

        if (stats.CurrentHealth <= 0)
        {
            GameMaster.KillBoss(this);
        }

        if (hpIndicator != null)
        {
            hpIndicator.SetHealth(stats.CurrentHealth, stats.maxHealth);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.collider.GetComponent<PlayerController>();

            if (player != null && !player.IsInvincible())
            {
                player.DamagePlayer(stats.damage);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.collider.GetComponent<PlayerController>();

            if (player != null && !player.IsInvincible())
            {
                player.DamagePlayer(stats.damage);
            }
        }
    }

    //======================== ATTACKS ========================

    private void SetPosRotActivate(GameObject obj, Vector2 pos, Quaternion rot)
    {
        obj.transform.position = pos;
        obj.transform.rotation = rot;
        obj.SetActive(true);
    }

    IEnumerator ScatterAttack(string projectileTag)
    {
        int cachedIndex = currentPatternIndex;
        float scatterTimer = patterns[currentPatternIndex].patternDuration;
        
        while (scatterTimer > 0f)
        {
            if (player == null)
            {
                yield break;
            }    

            Vector2 startPosition = (player.transform.position - this.transform.position).normalized * firePointOffset;

            float offsetX = Random.Range(0.92f, 1.08f);
            float offsetY = Random.Range(0.92f, 1.08f);

            float angle = Mathf.Atan2(startPosition.y, startPosition.x);

            float posX = Mathf.Cos(angle * offsetX) * firePointOffset;
            float posY = Mathf.Sin(angle * offsetY) * firePointOffset;

            Vector2 position = new Vector2(transform.position.x + posX, transform.position.y + posY);

            GameObject projectileObject = objectPooler.SpawnFromPool(projectileTag);

            if (projectileObject != null)
            {
                SetPosRotActivate(projectileObject, position, Quaternion.identity);

                Projectile projectile = projectileObject.GetComponent<Projectile>();
                projectile.isOwnerBoss = true;
                projectile.bossProjectileDamage = patterns[currentPatternIndex].projectileDamage;

                projectile.Launch((position - (Vector2)transform.position).normalized, patterns[currentPatternIndex].projectileForce);
            }

            float shotTimeOffset = Random.Range(0.9f, 1.1f);

            float waitTime = 1f / patterns[currentPatternIndex].patternFireRate * shotTimeOffset;

            yield return new WaitForSeconds(waitTime);

            scatterTimer -= waitTime;
        }

        patternCooldownTimers[cachedIndex] = patterns[cachedIndex].patternCooldown;
    }

    IEnumerator TriangleAttack(int count, float angle, string projectileTag)
    {
        int cachedIndex = currentPatternIndex;
        float triangleAttackTimer = patterns[currentPatternIndex].patternDuration;

        Vector2[] positions = new Vector2[count];
        float fractionAngleRad = angle / count * Mathf.Deg2Rad;

        while (triangleAttackTimer > 0f)
        {
            if (player == null)
            {
                yield break;
            }

            Vector2 startPosition = (player.transform.position - this.transform.position).normalized * firePointOffset;

            float startAngle = Mathf.Atan2(startPosition.y, startPosition.x);


            for (int i = 0; i < count; i++)
            {
                float offsetX = Random.Range(0.92f, 1.08f);
                float offsetY = Random.Range(0.92f, 1.08f);

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
                    projectile.isOwnerBoss = true;
                    projectile.bossProjectileDamage = patterns[currentPatternIndex].projectileDamage;

                    projectile.Launch((positions[i] - (Vector2)transform.position).normalized, patterns[currentPatternIndex].projectileForce);
                }
            }

            float shotTimeOffset = Random.Range(0.9f, 1.1f);

            float waitTime = 1f / patterns[currentPatternIndex].patternFireRate * shotTimeOffset;

            yield return new WaitForSeconds(waitTime);

            triangleAttackTimer -= waitTime;
        }

        patternCooldownTimers[cachedIndex] = patterns[cachedIndex].patternCooldown;
    }

    IEnumerator AllAroundAttack(int count, string projectileTag)
    {
        int cachedIndex = currentPatternIndex;
        float allAroundTimer = patterns[currentPatternIndex].patternDuration;

        Vector2[] positions = new Vector2[count];

        while (allAroundTimer > 0f)
        {
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
                    SetPosRotActivate(projectileObject, positions[i], Quaternion.identity);

                    Projectile projectile = projectileObject.GetComponent<Projectile>();
                    projectile.isOwnerBoss = true;
                    projectile.bossProjectileDamage = patterns[currentPatternIndex].projectileDamage;

                    projectile.Launch((positions[i] - (Vector2)transform.position).normalized, patterns[currentPatternIndex].projectileForce);
                }
            }

            float shotTimeOffset = Random.Range(0.9f, 1.1f);

            float waitTime = 1f / patterns[currentPatternIndex].patternFireRate * shotTimeOffset;

            yield return new WaitForSeconds(waitTime);

            allAroundTimer -= waitTime;
        }

        patternCooldownTimers[cachedIndex] = patterns[cachedIndex].patternCooldown;
    }

    IEnumerator RotateAttack(int numOfFirepoints, string projectileTag)
    {
        int cachedIndex = currentPatternIndex;
        float rotateAttackTimer = patterns[currentPatternIndex].patternDuration;
        float waitTime = 1f / patterns[currentPatternIndex].patternFireRate;

        Vector2[] positions = new Vector2[numOfFirepoints];

        float angleBetweenFirepoints = (360 / numOfFirepoints) * Mathf.Deg2Rad;

        while (rotateAttackTimer > 0f)
        {
            float angle = rotateSpeed * Time.time;

            for (int i = 0; i < numOfFirepoints; i++)
            {
                float posX = Mathf.Cos(angle + i * angleBetweenFirepoints) * firePointOffset;
                float posY = Mathf.Sin(angle + i * angleBetweenFirepoints) * firePointOffset;

                positions[i] = new Vector2(transform.position.x + posX, transform.position.y + posY);
            }

            for (int i = 0; i < positions.Length; i++)
            {
                GameObject projectileObject = objectPooler.SpawnFromPool(projectileTag);

                if (projectileObject != null)
                {
                    SetPosRotActivate(projectileObject, positions[i], Quaternion.identity);

                    Projectile projectile = projectileObject.GetComponent<Projectile>();
                    projectile.isOwnerBoss = true;
                    projectile.bossProjectileDamage = patterns[currentPatternIndex].projectileDamage;

                    projectile.Launch((positions[i] - (Vector2)transform.position).normalized, patterns[currentPatternIndex].projectileForce);
                }
            }

            yield return new WaitForSeconds(waitTime);

            rotateAttackTimer -= waitTime;
        }

        patternCooldownTimers[cachedIndex] = patterns[cachedIndex].patternCooldown;
    }

    IEnumerator ConeRotateAttack(float offsetAngle, string projectileTag)
    {
        int cachedIndex = currentPatternIndex;
        float rotateAttackTimer = patterns[currentPatternIndex].patternDuration;
        float waitTime = 1f / patterns[currentPatternIndex].patternFireRate;

        float offsetAngleRad = offsetAngle * Mathf.Deg2Rad;

        Vector2 position;

        while (rotateAttackTimer > 0f)
        {
            if (player == null)
            {
                yield break;
            }

            float startAngle = Mathf.Atan2(player.transform.position.y, player.transform.position.x);

            float angle = rotateSpeed * Time.time;

            float posX = Mathf.Cos(startAngle + Mathf.Sin(angle) * offsetAngleRad) * firePointOffset;
            float posY = Mathf.Sin(startAngle + Mathf.Sin(angle) * offsetAngleRad) * firePointOffset;

            position = new Vector2(transform.position.x + posX, transform.position.y + posY);

            GameObject projectileObject = objectPooler.SpawnFromPool(projectileTag);

            if (projectileObject != null)
            {
                SetPosRotActivate(projectileObject, position, Quaternion.identity);

                Projectile projectile = projectileObject.GetComponent<Projectile>();
                projectile.isOwnerBoss = true;
                projectile.bossProjectileDamage = patterns[currentPatternIndex].projectileDamage;

                projectile.Launch((position - (Vector2)transform.position).normalized, patterns[currentPatternIndex].projectileForce);
            }

            yield return new WaitForSeconds(waitTime);

            rotateAttackTimer -= waitTime;
        }

        patternCooldownTimers[cachedIndex] = patterns[cachedIndex].patternCooldown;
    }
}
