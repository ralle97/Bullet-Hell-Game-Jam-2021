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
    private GameObject upgradeMenuUI;

    public float powerupCooldown = 10f;
    private float powerupTimer;

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
        powerupTimer -= Time.deltaTime;

        if (powerupTimer <= 0f)
        {
            SpawnRandomPowerup();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Quiting application");
            Application.Quit();
        }
    }

    void SpawnRandomPowerup()
    {
        Debug.Log("Powerup spawned");

        powerupTimer = powerupCooldown;
    }

    public void EndGame()
    {
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
}
