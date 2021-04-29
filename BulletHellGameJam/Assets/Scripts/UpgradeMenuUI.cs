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
    private TextMeshProUGUI healthButtonText;
    [SerializeField]
    private TextMeshProUGUI speedButtonText;
    [SerializeField]
    private TextMeshProUGUI fireRateButtonText;
    [SerializeField]
    private TextMeshProUGUI damageButtonText;

    [SerializeField]
    private int healthAddition = 50;
    [SerializeField]
    private float speedAddition = 1f;
    [SerializeField]
    private float fireRateAddition = 1f;
    [SerializeField]
    private int damageAddition = 5;

    [SerializeField]
    private int healthUpgradeCost = 1;
    [SerializeField]
    private int speedUpgradeCost = 1;
    [SerializeField]
    private int fireRateUpgradeCost = 1;
    [SerializeField]
    private int damageUpgradeCost = 1;

    private int healthUpgradedCount = 0;
    private int speedUpgradedCount = 0;
    private int fireRateUpgradedCount = 0;
    private int damageUpgradedCount = 0;

    private int countAfterCostUp = 3;

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

        healthButtonText.text = "Upgrade\n(" + healthUpgradeCost + "UP)";
        speedButtonText.text = "Upgrade\n(" + speedUpgradeCost + "UP)";
        fireRateButtonText.text = "Upgrade\n(" + fireRateUpgradeCost + "UP)";
        damageButtonText.text = "Upgrade\n(" + damageUpgradeCost + "UP)";
    }

    private bool CheckUpgradePoints(int cost)
    {
        if (GameMaster.UpgradePoints < cost)
        {
            audioManager.PlaySound(noUpgradePointsSound);
            return false;
        }
        else
        {
            return true;
        }
    }

    private void Upgrade(int cost)
    {
        GameMaster.UpgradePoints -= cost;
        audioManager.PlaySound(upgradeSound);

        UpdateValues();
    }

    public void UpgradeHealth()
    {
        if (CheckUpgradePoints(healthUpgradeCost))
        {
            stats.maxHealth += healthAddition;
            stats.Health += healthAddition;

            hpBar.SetHealth(stats.Health, stats.maxHealth);

            Upgrade(healthUpgradeCost);
            healthUpgradedCount++;
            if (healthUpgradedCount % countAfterCostUp == 0)
            {
                healthUpgradeCost++;
            }
        }
    }

    public void UpgradeSpeed()
    {
        if (CheckUpgradePoints(speedUpgradeCost))
        {
            stats.speed += speedAddition;

            Upgrade(speedUpgradeCost);
            speedUpgradedCount++;
            if (speedUpgradedCount % countAfterCostUp == 0)
            {
                speedUpgradeCost++;
            }
        }
    }

    public void UpgradeFireRate()
    {
        if (CheckUpgradePoints(fireRateUpgradeCost))
        {
            stats.fireRate += fireRateAddition;

            Upgrade(fireRateUpgradeCost);
            fireRateUpgradedCount++;
            if (fireRateUpgradedCount % countAfterCostUp == 0)
            {
                fireRateUpgradeCost++;
            }
        }
    }

    public void UpgradeDamage()
    {
        if (CheckUpgradePoints(damageUpgradeCost))
        {
            stats.damage += damageAddition;

            Upgrade(damageUpgradeCost);
            damageUpgradedCount++;
            if (damageUpgradedCount % countAfterCostUp == 0)
            {
                damageUpgradeCost++;
            }
        }
    }
}
