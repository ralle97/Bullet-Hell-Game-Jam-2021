using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeMenuUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI healthText;
    [SerializeField]
    private TextMeshProUGUI speedText;
    [SerializeField]
    private TextMeshProUGUI fireRateText;
    [SerializeField]
    private TextMeshProUGUI damageText;

    [SerializeField]
    private int healthAddition = 50;
    [SerializeField]
    private float speedAddition = 1f;
    [SerializeField]
    private float fireRateAddition = 1f;
    [SerializeField]
    private int damageAddition = 5;

    [SerializeField]
    private int upgradeCost = 1;

    private PlayerStats stats;

    [SerializeField]
    private HPIndicatorUI hpBar;

    private AudioManager audioManager;

    [SerializeField]
    private string upgradeSound = "Upgrade";
    [SerializeField]
    private string noUpgradePointsSound = "NoUpgradePoints";

    private void Awake()
    {
        stats = PlayerStats.instance;

        healthAddition = stats.maxHealth / 3;
        damageAddition = stats.damage / 4;

        audioManager = AudioManager.instance;
    }

    private void OnEnable()
    {
        UpdateValues();
    }

    private void UpdateValues()
    {
        healthText.text = "Health: " + stats.maxHealth.ToString();
        speedText.text = "Speed: " + stats.speed.ToString();
        fireRateText.text = "Fire Rate: " + stats.fireRate.ToString() + "/s";
        damageText.text = "Damage: " + stats.damage.ToString();
    }

    private bool CheckUpgradePoints()
    {
        if (GameMaster.UpgradePoints < upgradeCost)
        {
            audioManager.PlaySound(noUpgradePointsSound);
            return false;
        }
        else
        {
            return true;
        }
    }

    private void Upgrade()
    {
        GameMaster.UpgradePoints -= upgradeCost;
        audioManager.PlaySound(upgradeSound);

        UpdateValues();
    }

    public void UpgradeHealth()
    {
        if (CheckUpgradePoints())
        {
            stats.maxHealth += healthAddition;
            stats.Health += healthAddition;

            hpBar.SetHealth(stats.Health, stats.maxHealth);

            Upgrade();
        }
    }

    public void UpgradeSpeed()
    {
        if (CheckUpgradePoints())
        {
            stats.speed += speedAddition;

            Upgrade();
        }
    }

    public void UpgradeFireRate()
    {
        if (CheckUpgradePoints())
        {
            stats.fireRate += fireRateAddition;

            Upgrade();
        }
    }

    public void UpgradeDamage()
    {
        if (CheckUpgradePoints())
        {
            stats.damage += damageAddition;

            Upgrade();
        }
    }
}
