using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public enum PowerupType { HPREGEN };

    public PowerupType powerupType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // TODO: If health full don't allow pickup; Same for other pickups, if already in use, forbid now one

            GameMaster.instance.PowerupPicked(powerupType);
            Destroy(gameObject);
        }
    }
}
