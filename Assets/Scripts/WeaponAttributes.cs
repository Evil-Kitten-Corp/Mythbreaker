using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttributes : MonoBehaviour
{
    public AttributesManager atm;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyBehaviour>().TakeDamage.Invoke(atm.attack);
            GetComponentInParent<AttackControl>().AttackSuccessful();
        }
    }
}
