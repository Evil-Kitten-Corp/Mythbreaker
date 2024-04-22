using System;
using UnityEngine;

namespace RIP
{
    public class TWeapon : MonoBehaviour
    {
        public float baseDamage = 0;
        public PossibilityOne comboManager;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<TEnemy>() != null)
            {
                float damage = comboManager != null ? comboManager.GetCurrentAnimDmg() ?? baseDamage : baseDamage;
                other.GetComponent<TEnemy>().TakeDamage(damage);
            }
        }
    }
}