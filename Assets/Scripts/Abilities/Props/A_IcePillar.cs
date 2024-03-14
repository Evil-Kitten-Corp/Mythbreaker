using System.Collections;
using Combat;
using UnityEngine;

namespace Abilities.Props
{
    public class A_IcePillar : MonoBehaviour
    {
        public float slowRadius = 5f; // Radius of the slow area
        public float slowAmount = 0.35f; // Amount to slow down enemies (35% of their movement speed)
        public float minSlowDuration = 2f; // Duration of the slow effect after leaving the area

        private void Start()
        {
            ApplySlowEffect();
        }

        private void ApplySlowEffect()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, slowRadius);

            foreach (Collider col in colliders)
            {
                if (col.CompareTag("Enemy"))
                {
                    // Apply slow effect to the enemy
                    EntityCombat enemyMovement = col.GetComponent<EntityCombat>();
                    if (enemyMovement != null)
                    {
                        enemyMovement.ApplySlow(slowAmount);
                    }
                }
            }
        }
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                EntityCombat enemyMovement = other.GetComponent<EntityCombat>();
                if (enemyMovement != null)
                {
                    enemyMovement.ApplySlow(slowAmount);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                StartCoroutine(RemoveSlowDelayed(other.gameObject));
            }
        }

        private IEnumerator RemoveSlowDelayed(GameObject enemy)
        {
            yield return new WaitForSeconds(minSlowDuration);

            EntityCombat enemyMovement = enemy.GetComponent<EntityCombat>();
            if (enemyMovement != null)
            {
                enemyMovement.RestoreSpeed();
            }
        }
    }
}