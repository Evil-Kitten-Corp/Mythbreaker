using System.Collections;
using Abilities;
using Combat;
using UnityEngine;

namespace Base
{
    public class AltAbilityKnockup : AltAbilityBase
    {
        public float knockupForce = 10f;
        public float damage = 10f;
        public float detectionRadius = 5f;
        public LayerMask enemyLayer;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }

        protected override IEnumerator Activate()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);

            foreach (Collider col in colliders)
            {
                Rigidbody rb = col.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // Apply knockup force
                    rb.AddForce(Vector3.up * knockupForce, ForceMode.Impulse);

                    // Trigger animation
                    col.GetComponent<Animator>().SetTrigger("Knockup");

                    // Deal damage
                    EntityCombat enemyHealth = col.GetComponent<EntityCombat>();
                    if (enemyHealth != null)
                    {
                        enemyHealth.TakeDamage(damage);
                    }
                }
            }

            return base.Activate();
        }
    }
}