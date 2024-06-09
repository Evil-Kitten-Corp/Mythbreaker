using UnityEngine;

namespace Abilities
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : PoolableObject
    {
        public float autoDestroyTime = 5f;
        public float moveSpeed = 2f;
        public int damage = 5;
        public Rigidbody Rigidbody;
        protected Transform Target;

        private const string DISABLE_METHOD_NAME = "Disable";

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        protected virtual void OnEnable()
        {
            CancelInvoke(DISABLE_METHOD_NAME);
            Invoke(DISABLE_METHOD_NAME, autoDestroyTime);
        }

        public virtual void Spawn(Vector3 Forward, int damage, Transform Target)
        {
            this.damage = damage;
            this.Target = Target;
            Rigidbody.AddForce(Forward * moveSpeed, ForceMode.VelocityChange);
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            AttributesManager damageable;

            if (other.TryGetComponent(out damageable))
            {
                damageable.TakeDamage(damage);
            }

            Disable();
        }

        protected void Disable()
        {
            CancelInvoke(DISABLE_METHOD_NAME);
            Rigidbody.velocity = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
}