using UnityEngine;

namespace Base
{
    public class A_Vortex : MonoBehaviour, IAbilityProp
    {
        public float pullForce = 10f;

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                // direction from the enemy to the center of the vortex
                Vector3 direction = transform.position - other.transform.position;

                // pull the enemy towards the center of the vortex
                Rigidbody rb = other.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(direction.normalized * pullForce * Time.deltaTime, ForceMode.Acceleration);
                }
            }
        }

    }
}