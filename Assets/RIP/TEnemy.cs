using System;
using BrunoMikoski.ServicesLocation;
using TMPro;
using UnityEngine.UI;

public class TEnemy : TCombatEntity
{
    public Image healthBar;
    public TMP_Text healthText;
    
    public override void Start()
    {
        base.Start();
        OnBecameVisible();
    }

    public override void Update()
    {
        base.Update();
        
        healthBar.fillAmount = _health / maxHealth;
        healthText.text = _health + " / " + maxHealth;
    }

    private void OnBecameVisible()
    {
        if (!ServiceLocator.Instance.GetInstance<AbilityManager>().abilityPreviewerSingle.screenTargets.Contains(transform))
            ServiceLocator.Instance.GetInstance<AbilityManager>().abilityPreviewerSingle.screenTargets.Add(transform);
    }

    private void OnBecameInvisible()
    {
        if (ServiceLocator.Instance.GetInstance<AbilityManager>().abilityPreviewerSingle.screenTargets.Contains(transform))
            ServiceLocator.Instance.GetInstance<AbilityManager>().abilityPreviewerSingle.screenTargets.Remove(transform);
    }

    private void OnDestroy()
    {
        if (ServiceLocator.Instance.GetInstance<AbilityManager>().abilityPreviewerSingle.screenTargets.Contains(transform))
            ServiceLocator.Instance.GetInstance<AbilityManager>().abilityPreviewerSingle.screenTargets.Remove(transform);
    }
}