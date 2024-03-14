using System.Collections;
using Abilities;
using Combat;
using UnityEngine;

namespace Base
{
    public class AltAbilityAlphaStrike: AltAbilityBase
    {
        public float damage = 50f;
        public float range = 5f;
        public LayerMask enemyLayer;
        public GameObject strikeEffectPrefab;
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);
        }

        public override void ChangeAbilitySlot(AbilitySlot abslot)
        {
            
        }

        protected override IEnumerator Activate()
        {
            EntityTargeter targeter = player.GetComponent<EntityTargeter>();
            
            Collider[] colliders = Physics.OverlapSphere(targeter.Target.transform.position, range, enemyLayer);

            foreach (Collider col in colliders)
            {
                // Deal damage to enemies
                EntityCombat enemyHealth = col.GetComponent<EntityCombat>();
                
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);
                }
            }

            // Instantiate strike effect at the target position
            if (strikeEffectPrefab != null)
            {
                Instantiate(strikeEffectPrefab, targeter.Target.transform.position, Quaternion.identity);
            }

            return base.Activate();
        }
    }
}