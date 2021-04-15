using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void EndGame()
    {
        Debug.Log("GAME OVER!");
    }

    public void KillEnemy(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }
}
