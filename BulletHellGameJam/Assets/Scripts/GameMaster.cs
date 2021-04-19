using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using TMPro;

public class GameMaster : MonoBehaviour
{
    #region Singleton

    public static GameMaster instance;

    private void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            instance = this;
        }
    }

    #endregion

    private Bounds bounds;

    [SerializeField]
    private GameObject gameOverUI;

    [SerializeField]
    private GameObject pauseGameUI;

    [SerializeField]
    private GameObject gameFinishedUI;

    [SerializeField]
    private GameObject upgradeMenuUI;

    [SerializeField]
    private GameObject waveCountdownUI;

    [HideInInspector]
    public bool isPaused;

    public float powerupCooldown = 10f;
    private float powerupTimer;

    private float prevTimeScale = 1f;

    public bool isGameOver = false;

    private AudioManager audioManager;
    private PlayerStats playerStats;

    public GameObject[] powerupPrefabs;

    private bool isTimeSlowed;
    public float slowTimeDuration = 2f;
    private float slowTimer;
    public float timeSlowAmount = 0.5f;

    private bool isFireRateEnhanced;
    public float fireRateDuration = 3f;
    private float fireRateTimer;
    public float fireRateAmount = 2f;
    private float prevFireRate;

    private bool isTriangleAttack;
    public float triangleAttackDuration = 5f;
    private float triangleTimer;

    [SerializeField]
    private TextMeshProUGUI powerupTimerText;

    [HideInInspector]
    public bool gameFinished;

    [SerializeField]
    private HPIndicatorUI hpBar;

    [SerializeField]
    private int startingUpgradePoints = 0;
    [SerializeField]
    private int upgradePointsAward = 2;
    [HideInInspector]
    public bool canUpgrade = false;
    public bool upgradeMenuOpened = false;
    public static int UpgradePoints;
    [SerializeField]
    private TextMeshProUGUI upgradePointsText;

    private void Start()
    {
        playerStats = PlayerStats.instance;
        if (playerStats == null)
        {
            Debug.LogError("No PlayerStats found in the scene.");
        }

        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("No AudioManager found in the scene.");
        }

        CinemachineConfiner confiner = FindObjectOfType<CinemachineConfiner>();

        bounds.center = confiner.m_BoundingShape2D.bounds.center * 3 / 5;
        bounds.extents = confiner.m_BoundingShape2D.bounds.extents * 3 / 5;

        powerupTimer = powerupCooldown;

        if (gameOverUI == null)
        {
            Debug.LogError("GameOverUI not found!");
        }

        UpgradePoints = startingUpgradePoints;
        upgradePointsText.text = "Upgrade points: " + UpgradePoints.ToString() + "UP";
    }

    private void Update()
    {
        if (isTimeSlowed)
        {
            slowTimer -= Time.deltaTime;

            if (slowTimer < 0f)
            {
                isTimeSlowed = false;
                Time.timeScale = 1f;
            }
        }

        if (isFireRateEnhanced)
        {
            fireRateTimer -= Time.deltaTime;

            if (fireRateTimer < 0f)
            {
                isFireRateEnhanced = false;
                playerStats.fireRate = prevFireRate;
            }
        }

        if (isTriangleAttack)
        {
            triangleTimer -= Time.deltaTime;

            if (triangleTimer < 0f)
            {
                isTriangleAttack = false;
                playerStats.isTriangleAttack = false;
            }
        }

        if (gameFinished && !isGameOver)
        {
            FinishGame();
        }

        if (!isGameOver)
        {
            powerupTimer -= Time.deltaTime;

            powerupTimerText.text = "Powerup in: " + powerupTimer.ToString("F2") + "s";

            if (powerupTimer <= 0f)
            {
                SpawnRandomPowerup();
            }

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
            {
                if (!upgradeMenuOpened)
                {
                    TogglePauseMenu(!pauseGameUI.activeSelf);
                }
                else
                {
                    ToggleUpgradeMenu(!upgradeMenuUI.activeSelf);
                }
            }

            if (canUpgrade && Input.GetKeyDown(KeyCode.U))
            {
                ToggleUpgradeMenu(!upgradeMenuUI.activeSelf);
            }
        }
    }

    public void TogglePauseMenu(bool active)
    {
        pauseGameUI.SetActive(active);
        
        if (active)
        {
            isPaused = true;
            prevTimeScale = Time.timeScale;
            Time.timeScale = 0f;    
        }
        else
        {
            isPaused = false;
            Time.timeScale = prevTimeScale;
        }
    }

    public void ToggleUpgradeMenu(bool active)
    {
        upgradeMenuUI.SetActive(active);

        if (active)
        {
            upgradeMenuOpened = true;
            upgradePointsText.gameObject.SetActive(false);
            prevTimeScale = Time.timeScale;
            Time.timeScale = 0.5f;
        }
        else
        {
            upgradeMenuOpened = false;
            upgradePointsText.gameObject.SetActive(true);
            Time.timeScale = prevTimeScale;
        }

        upgradePointsText.text = "Upgrade points: " + UpgradePoints.ToString() + "UP";
    }

    void SpawnRandomPowerup()
    {
        int index = Random.Range(0, powerupPrefabs.Length);

        float spawnPosX = Random.Range((float)-bounds.extents.x, (float)bounds.extents.x);
        float spawnPosY = Random.Range((float)-bounds.extents.y, (float)bounds.extents.y);

        Vector2 spawnPos = new Vector2(spawnPosX, spawnPosY);

        Instantiate(powerupPrefabs[index], spawnPos, Quaternion.identity);

        Debug.Log("Powerup spawned");

        powerupTimer = powerupCooldown;
    }

    public void EndGame()
    {
        Time.timeScale = 1f;
        isGameOver = true;

        waveCountdownUI.SetActive(false);
        gameOverUI.SetActive(true);
    }

    private void FinishGame()
    {
        Time.timeScale = 1f;
        isGameOver = true;

        waveCountdownUI.SetActive(false);
        gameFinishedUI.SetActive(true);
    }

    public static void KillEnemy(Enemy enemy)
    {
        Instantiate(enemy.deathEffect, enemy.transform.position, Quaternion.identity);
        CameraShake.instance.Shake(2f, 0.15f);

        // TODO: If object pooling for enemies, then change active to false
        //enemy.gameObject.SetActive(false);
        Destroy(enemy.gameObject);
    }

    public static void KillPlayer(PlayerController player)
    {
        Instantiate(player.deathEffect, player.transform.position, Quaternion.identity);
        CameraShake.instance.Shake(5f, 0.6f);
        
        Destroy(player.gameObject);
        
        if (!instance.isGameOver)
        {
            instance.EndGame();
        }
    }

    public void PowerupPicked(Powerup.PowerupType type)
    {
        audioManager.PlaySound("Pickup");

        if (type == Powerup.PowerupType.HPREGEN)
        {
            playerStats.Health = playerStats.maxHealth;

            hpBar.SetHealth(playerStats.Health, playerStats.maxHealth);
        }
        else if (type == Powerup.PowerupType.STOPWATCH)
        {
            // TODO: What to do when already active?
            prevTimeScale = Time.timeScale;
            Time.timeScale = timeSlowAmount;
            isTimeSlowed = true;
            slowTimer = slowTimeDuration;
        }
        else if (type == Powerup.PowerupType.FIRERATE)
        {
            if (!isFireRateEnhanced)
            {
                prevFireRate = playerStats.fireRate;
                playerStats.fireRate *= fireRateAmount;
                isFireRateEnhanced = true;
                fireRateTimer = fireRateDuration;
            }
            else
            {
                fireRateTimer += fireRateDuration;
            }
        }
        else if (type == Powerup.PowerupType.TRIANGLE)
        {
            if (!isTriangleAttack)
            {
                playerStats.isTriangleAttack = true;
                triangleTimer = triangleAttackDuration;
            }
            else
            {
                triangleTimer += triangleAttackDuration;
            }
        }

        Debug.Log("Powerup " + type + " picked.");
    }

    public void WaveFinished()
    {
        UpgradePoints += upgradePointsAward;
        upgradePointsText.text = "Upgrade points: " + UpgradePoints.ToString() + "UP";
        canUpgrade = true;
    }
}
