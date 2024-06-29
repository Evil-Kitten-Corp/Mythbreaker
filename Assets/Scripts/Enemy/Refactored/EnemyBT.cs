using System.Collections;
using Abilities;
using Minimalist.Bar.Quantity;
using UnityEngine;

namespace FinalScripts.Refactored
{
    public class EnemyBT: MonoBehaviour
    {
        public Bullet bullet;
        public Transform shootPoint;

        public RagdollReplacer ragdoll;
        public QuantityBhv health;

        public Animator anim;
        
        private Transform _t;
        private float lastShootTime;
        public float shootCooldown = 2.0f;

        public void TakeDamage(float amount)
        {
            if (health.Amount >= 0)
            {
                return;
            }

            health.Amount -= amount;
            anim.SetTrigger("Hit");

            if (health.Amount >= 0) Die();
        }

        public void Die()
        {
            ragdoll.Replace();
        }

        public void AnimatorEventShoot()
        {
            //bullet.Shoot(shootPoint);
            lastShootTime = Time.time;
            StartCoroutine(Cooldown());
        }

        public bool Shoot(Transform target)
        {
            _t = target;
            return true;
        }

        public bool CanShoot()
        {
            return Time.time >= lastShootTime + shootCooldown;
        }

        private IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(shootCooldown);
        }
    }
}