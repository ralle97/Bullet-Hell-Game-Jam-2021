using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UpgradePointsCounterUI : MonoBehaviour
{
    private TextMeshProUGUI upgradePointsText;

    private void Awake()
    {
        upgradePointsText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        upgradePointsText.text = "Upgrade Points: " + GameMaster.UpgradePoints.ToString() + "UP";
    }
}
