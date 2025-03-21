﻿using System.Collections;
using DG.Tweening;
using Enemy.Refactored.Utilities;
using UnityEngine;

namespace FinalScripts.Refactored
{
    public class RangedEnemyBT: EnemyBT
    {
        public Transform firePoint;
        public ParticleSystem fireParticles;
        
        public float shootCooldown = 2.0f;
        private bool _canShoot = true;
        
        private ObjectPooler<RangedEnemyProjectile> _projectilePool;
        public RangedEnemyProjectile projectile;
        
        private RangedEnemyProjectile _loadedProjectile;
        private bool _isAfraid = false;

        private void Start()
        {
            _projectilePool = new ObjectPooler<RangedEnemyProjectile>();
            _projectilePool.Initialize(20, projectile);
        }
        
        public void LoadProjectile()
        {
            if (_loadedProjectile != null)
            {
                return;
            }

            _loadedProjectile = _projectilePool.GetNew();
            _loadedProjectile.transform.SetParent(transform, false);
            _loadedProjectile.transform.position = firePoint.position;
            _loadedProjectile.transform.rotation = Quaternion.identity;
            
            _loadedProjectile.SetupProjectile();
        }

        void AttackProjectile(Transform target)
        {
            if (_loadedProjectile == null)
            {
                LoadProjectile();
            }

            if (attackAudio != null)
            {
                attackAudio.PlayRandomClip();
            }
            
            fireParticles.Play();

            _loadedProjectile.transform.SetParent(null, true);
            _loadedProjectile.InitiateProjectile(target);
            _loadedProjectile = null;
        }

        public void AnimatorEventShoot()
        {
            var target = GameObject.FindWithTag("ShootPt").transform;
            
            transform.DODynamicLookAt(target.position, .1f);
            AttackProjectile(target);
            _canShoot = false;
            
            StartCoroutine(Cooldown());
        }

        public override bool Attack(Transform target)
        {
            return true;
        }

        public override bool CanAttack()
        {
            return _canShoot;
        }

        public void Afraid(bool v)
        {
            if (_isAfraid == v) //if we want him afraid and he's already afraid or
                                //if we don't want him afraid and he's already not
            {
                return;
            }

            if (_isAfraid && v == false) //if we don't want him afraid and he's currently afraid
            {
                anim.SetBool("Ducking", false);
                _isAfraid = false;
            }
            else //if we want him afraid
            {
                anim.SetTrigger("Afraid");
                anim.SetBool("Ducking", true);
                _isAfraid = true;
            }
        }

        private IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(shootCooldown);
            _canShoot = true;
        }
    }
}