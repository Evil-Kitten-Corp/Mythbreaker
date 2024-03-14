using System.Collections.Generic;
using UnityEngine;

namespace not_this_again.Weapons
{
    public interface IDamageable
    {
        public List<GameObject> DamageMessageReceivers { get; set; }
        public bool IsInvulnerable { get; set; }
        Character.Data.CombatData CombatData { get; }

        GameObject GetGameObject();
        
        void ApplyDamage(DamageMessage data);
        void TakeDamage(IDamageable mOwner, float f);
        void SetLookAt(GameObject mOwnerGameObject, float f, float f1);
    }
}