using System.Collections;
using System.Collections.Generic;
using FinalScripts;
using UnityEngine;

public class WeaponAttributes : MonoBehaviour
{
    public AttributesManager atm;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemyBehaviour>(out var eb))
        {
            eb.TakeDamage.Invoke(atm.attack);
            GetComponentInParent<AttackControl>().AttackSuccessful();
        }
        else if (other.TryGetComponent<EnemyAppendage>(out var ap))
        {
            ap.TakeDamage(atm.attack);
            GetComponentInParent<AttackControl>().AttackSuccessful();
        }
    }
}
