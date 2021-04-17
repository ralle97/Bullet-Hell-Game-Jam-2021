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
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.LogError("MORE THAN ONE GameMaster INSTANCE");
        }
    }

    #endregion

    public CameraShake cameraShake;
    private Bounds bounds;

    public float powerupCooldown = 10f;
    private float powerupTimer;

    private void Start()
    {
        cameraShake = CameraShake.instance;

        CinemachineConfiner confiner = FindObjectOfType<CinemachineConfiner>();

        bounds.center = confiner.m_BoundingShape2D.bounds.center * 3 / 5;
        bounds.extents = confiner.m_BoundingShape2D.bounds.extents * 3 / 5;

        powerupTimer = powerupCooldown;
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
        // TODO: Show GAME OVER Screen
        Debug.Log("GAME OVER!");
    }

    public static void KillEnemy(Enemy enemy)
    {
        Instantiate(enemy.deathEffect, enemy.transform.position, Quaternion.identity);
        instance.cameraShake.Shake(2f, 0.15f);

        // TODO: If object pooling for enemies, then change active to false
        //enemy.gameObject.SetActive(false);
        Destroy(enemy.gameObject);
    }

    public static void KillPlayer(PlayerController player)
    {
        Instantiate(player.deathEffect, player.transform.position, Quaternion.identity);
        Destroy(player.gameObject);
        
        instance.EndGame();
    }
}
