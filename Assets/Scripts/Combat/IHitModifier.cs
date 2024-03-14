using System;
using System.Collections;
using UnityEngine;

namespace Combat
{
    public interface IHitModifier
    {
        public IEnumerator Tick(IDamageable damageable);
    }

    [Serializable]
    public class DoTModifier : IHitModifier
    {
        public float damagePerTick;
        public float totalTime;
        private float _currentTime;

        public DoTModifier(float damagePerTick, float totalTime)
        {
            this.damagePerTick = damagePerTick;
            this.totalTime = totalTime;
        }

        public IEnumerator Tick(IDamageable damageable)
        {
            _currentTime = totalTime;

            while (_currentTime > 0)
            {
                damageable.TakeDamage(damagePerTick);
                yield return new WaitForSeconds(1);
                _currentTime--;
            }
        }
    }
}