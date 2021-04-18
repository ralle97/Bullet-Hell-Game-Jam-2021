using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

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

    public float powerupCooldown = 10f;
    private float powerupTimer;

    private float prevTimeScale = 1f;

    private bool isGameOver = false;

    private void Start()
    {
        CinemachineConfiner confiner = FindObjectOfType<CinemachineConfiner>();

        bounds.center = confiner.m_BoundingShape2D.bounds.center * 3 / 5;
        bounds.extents = confiner.m_BoundingShape2D.bounds.extents * 3 / 5;

        powerupTimer = powerupCooldown;

        if (gameOverUI == null)
        {
            Debug.LogError("GameOverUI not found!");
        }
    }

    private void Update()
    {
        if (!isGameOver)
        {
            powerupTimer -= Time.deltaTime;

            if (powerupTimer <= 0f)
            {
                SpawnRandomPowerup();
            }

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
            {
                TogglePauseMenu(!pauseGameUI.activeSelf);
            }
        }
    }

    public void TogglePauseMenu(bool active)
    {
        pauseGameUI.SetActive(active);
        
        if (active)
        {
            prevTimeScale = Time.timeScale;
            Time.timeScale = 0f;    
        }
        else
        {
            Time.timeScale = prevTimeScale;
        }
    }

    void SpawnRandomPowerup()
    {
        Debug.Log("Powerup spawned");

        powerupTimer = powerupCooldown;
    }

    public void EndGame()
    {
        isGameOver = true;

        gameOverUI.SetActive(true);

        Debug.Log("GAME OVER!");
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
        
        instance.EndGame();
    }

    public void PowerupPicked(Powerup.PowerupType type)
    {
        Debug.Log("Powerup " + type + " picked.");
    }
}
