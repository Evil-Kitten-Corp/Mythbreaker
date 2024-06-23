using UnityEngine;

namespace FinalScripts.Refactored
{
    public class RigidbodyDelayedForce : MonoBehaviour
    {
        public Vector3 forceToAdd;

        private void Start()
        {
            Rigidbody[] rigidbodies = gameObject.GetComponentsInChildren<Rigidbody>();

            foreach (var t in rigidbodies)
            {
                t.maxAngularVelocity = 45;
                t.angularVelocity = transform.right * -45.0f;
                t.velocity = forceToAdd;
            }
        }
    }
}