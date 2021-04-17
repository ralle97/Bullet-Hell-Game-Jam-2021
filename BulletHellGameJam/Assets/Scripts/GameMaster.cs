using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void Start()
    {
        cameraShake = CameraShake.instance;
    }

    /*  FOR DEBUGGING PURPOSE
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    */

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
