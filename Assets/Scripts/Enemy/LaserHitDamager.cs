using System;
using System.Collections;
using UnityEngine;

namespace FinalScripts
{
    [RequireComponent(typeof(Collider))]
    public class LaserHitDamager: MonoBehaviour
    {
        public int damageOnHit;
        private bool _isDamaging;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<AttributesManager>(out var am))
            {
                if (!_isDamaging)
                {
                    StartCoroutine(ApplyDamage(am));
                }
            }
        }

        private IEnumerator ApplyDamage(AttributesManager am)
        {
            _isDamaging = true;

            while (true)
            {
                am.TakeDamage(damageOnHit);
                yield return new WaitForSeconds(1f);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<AttributesManager>(out var am))
            {
                StopCoroutine(ApplyDamage(am));
                _isDamaging = false;
            }
        }

        private void OnDisable()
        {
            _isDamaging = false;
        }
    }
}