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
        }
        else
        {
            Debug.LogError("MORE THAN ONE GameMaster INSTANCE");
        }
    }

    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void EndGame()
    {
        // TODO: Show GAME OVER Screen
        Debug.Log("GAME OVER!");
    }

    public static void KillEnemy(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    public static void KillPlayer(PlayerController player)
    {
        Destroy(player.gameObject);
        instance.EndGame();
    }
}
