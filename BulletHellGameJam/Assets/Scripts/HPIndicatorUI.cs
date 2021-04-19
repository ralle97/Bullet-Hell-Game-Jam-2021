using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPIndicatorUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform healthBarRect;

    private Image hpImage;

    private void Start()
    {
        if (healthBarRect == null)
        {
            Debug.LogError("HP INDICATOR: No health bar object referenced!");
        }
    }

    public void SetHealth(int currentHP, int maxHP)
    {
        float value = (float)currentHP / maxHP;

        if (currentHP <= 0.2f * maxHP)
        {
            hpImage = healthBarRect.GetComponent<Image>();
            hpImage.color = Color.red;
        }
        else if (currentHP <= 0.4f * maxHP)
        {
            hpImage = healthBarRect.GetComponent<Image>();
            hpImage.color = Color.yellow;
        }
        else
        {
            hpImage = healthBarRect.GetComponent<Image>();
            hpImage.color = Color.green;
        }

        healthBarRect.localScale = new Vector3(value, healthBarRect.localScale.y, healthBarRect.localScale.z);
    }
}
