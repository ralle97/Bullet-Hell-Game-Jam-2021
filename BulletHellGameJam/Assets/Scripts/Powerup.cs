using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public enum PowerupType { HPREGEN, STOPWATCH, FIRERATE, TRIANGLE };

    public PowerupType powerupType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (powerupType == PowerupType.HPREGEN)
            {
                PlayerStats stats = PlayerStats.instance;

                if (stats.Health >= stats.maxHealth)
                {
                    return;
                }
            }

            // TODO: What to do with multiple stopwatches?

            GameMaster.instance.PowerupPicked(powerupType);
            Destroy(gameObject);
        }
    }
}
