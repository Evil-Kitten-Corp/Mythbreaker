using System;
using System.Collections;
using DG.Tweening;
using Enemy.Refactored.Utilities;
using UnityEngine;
using UnityEngine.AI;

namespace FinalScripts.Refactored
{
    public class MeleeEnemyBT: EnemyBT
    {
        public int damage;
        public float attackCooldown;

        private AttributesManager _target;
        private bool _canAttack = true;

        private TargetDistributor.TargetFollower _followerInstance;
        private NavMeshAgent _agent;

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.updateRotation = false;
        }

        private void Update()
        {
            if (_agent.velocity.sqrMagnitude > Mathf.Epsilon)
            {
                transform.rotation = Quaternion.LookRotation(_agent.velocity.normalized);
            }
        }

        public void AnimatorEventShoot()
        {
            if (attackAudio != null)
            {
                attackAudio.PlayRandomClip();
            }

            if (_target == null)
            {
                _target = FindObjectOfType<AttributesManager>();
            }

            transform.DODynamicLookAt(_target.transform.position, 1f);

            _target.TakeDamage(damage);
            _canAttack = false;
            StartCoroutine(Cooldown());
        }
        
        public override bool Attack(Transform t)
        {
            if (t.TryGetComponent(out _target))
            {
                return true; 
            }

            return false;
        }

        public override bool CanAttack()
        {
            return _canAttack;
        }
        
        private IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(attackCooldown);
            _canAttack = true;
        }

        public void SetFollowerInstance(TargetDistributor.TargetFollower registerNewFollower)
        {
            _followerInstance = registerNewFollower;
        }

        public void UnregisterFollower()
        {
            if (_followerInstance != null)
            {
                _followerInstance.Distributor.UnregisterFollower(_followerInstance);
            }
        }
        
        protected void OnDisable()
        {
            UnregisterFollower();
        }
    }
}