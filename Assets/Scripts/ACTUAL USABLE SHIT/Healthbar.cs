using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Image fill;
    public TMP_Text text;
    public AttributesManager mcAtm;
    public float speed = 2;

    private float _currHealthAmount;
    private float _maxHealthAmount;

    private void Start()
    {
        _currHealthAmount = mcAtm.health.Amount;
        _maxHealthAmount = mcAtm.health.MaximumAmount;
    }

    private void Update()
    {
        _currHealthAmount = mcAtm.health.Amount;
        text.text = _currHealthAmount + " HP";
        float newFillAmount = _currHealthAmount / _maxHealthAmount;

        fill.fillAmount = Mathf.Lerp(fill.fillAmount, newFillAmount, Time.deltaTime * speed);
    }
}
