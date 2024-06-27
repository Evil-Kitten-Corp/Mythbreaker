using FinalScripts;
using UnityEngine;

public class WeaponAttributes : MonoBehaviour
{
    public AttributesManager attributes;

    private void OnTriggerEnter(Collider other)
    {
        WeaponAndAttackManager wp = GetComponentInParent<WeaponAndAttackManager>();

        if (other.TryGetComponent<EnemyBase>(out var eb))
        {
            eb.ApplyDamage(attributes.transform, attributes.attack);
            wp.AttackSuccessful();
        }
        else if (other.TryGetComponent<EnemyAppendage>(out var ap))
        {
            ap.TakeDamage(attributes.attack);
            wp.AttackSuccessful();
        }
    }
}
