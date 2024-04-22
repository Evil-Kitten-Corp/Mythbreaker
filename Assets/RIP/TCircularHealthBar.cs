using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TCircularHealthBar : MonoBehaviour
{
    private const float LERP_SPEED = 0.05f;
    public Image barImage;
    public Image damagedBarImage;
    public TMP_Text healthText;
    public TPlayer healthSystem;

    private void Start()
    {
        damagedBarImage.fillAmount = barImage.fillAmount;
    }

    private void Update()
    {
        barImage.fillAmount = healthSystem._health / healthSystem.maxHealth;
        healthText.text = healthSystem._health + " / " + healthSystem.maxHealth;

        if (barImage.fillAmount != damagedBarImage.fillAmount)
        {
            damagedBarImage.fillAmount = Mathf.Lerp(damagedBarImage.fillAmount, barImage.fillAmount, LERP_SPEED);
        }
    }

}