using FinalScripts;
using FinalScripts.Refactored;
using UnityEngine;

public class WeaponAttributes : MonoBehaviour
{
    public AttributesManager attributes;
    public AttackData currentAttackData;

    private void OnTriggerEnter(Collider other)
    {
        WeaponAndAttackManager wp = GetComponentInParent<WeaponAndAttackManager>();
        
        if (other.TryGetComponent<EnemyBT>(out var bt))
        {
            bt.TakeDamage(wp.baseAttack + currentAttackData.damage, currentAttackData.canCrit);
            wp.AttackSuccessful();
        }
        else if (other.TryGetComponent<EnemyBase>(out var eb))
        {
            eb.ApplyDamage(attributes.transform, wp.baseAttack + currentAttackData.damage, currentAttackData.canCrit);
            wp.AttackSuccessful();
        }
        else if (other.TryGetComponent<EnemyAppendage>(out var ap))
        {
            ap.TakeDamage((int)(wp.baseAttack + currentAttackData.damage));
            wp.AttackSuccessful();
        }
    }
}
