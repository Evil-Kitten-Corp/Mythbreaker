using System.Collections;
using Enemy.Refactored.Utilities;
using UnityEngine;

namespace FinalScripts.Refactored
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class RangedEnemyProjectile: MonoBehaviour, IPooledObj<RangedEnemyProjectile>
    {
        public int PoolID { get; set; }
        
        public ObjectPooler<RangedEnemyProjectile> Pool { get; set; }
        
        [Header("Parameters")] 
        public BulletMovementType projectileMovementType;
        public float speed;
        public float baseDamage;
        public float poolDelay = 1f;
        public float maxLifeTime = 10f;
        
        [Header("References")]
        [Space(10)]
        public GameObject meshObject;
        public GameObject effectsObject;
        
        public bool IsFired { get; private set; }

        private bool _targetHit;
        private Transform _currentTargetTf;

        private Rigidbody _myRigidBody;
        private Collider _myCollider;
        
        private Coroutine _homingMovementCor, _lifeTimerCor;
        
        private bool _homingCorRunning, _lifeTimerRunning;
        
        public void SetupProjectile()
        {
            _myRigidBody = GetComponent<Rigidbody>();
            _myCollider = GetComponent<Collider>();
            effectsObject.SetActive(false);
            gameObject.SetActive(false);
        }
        
        public void InitiateProjectile(Transform target)
        {
            _currentTargetTf = target;
            FireProjectile();
        }
        
        private void FireProjectile()
        {
            IsFired = true;
            _targetHit = false;

            gameObject.SetActive(true);

            if (projectileMovementType == BulletMovementType.Homing)
            {
                if (!_homingCorRunning)
                {
                    _homingMovementCor = StartCoroutine(HomingMovement());
                }
            }
            else if (projectileMovementType == BulletMovementType.Aimed)
            {
                Vector3 dir = (_currentTargetTf.position - transform.position).normalized;
                transform.forward = dir;
                _myRigidBody.velocity = dir * speed;
            }

            if (!_lifeTimerRunning)
            {
                _lifeTimerCor = StartCoroutine(LifeTimer());
            }

            _myCollider.enabled = true;
            meshObject.SetActive(true);
        }
        
        private IEnumerator HomingMovement()
        {
            _homingCorRunning = true;

            bool targetValid = true;
            GameObject targetObj = _currentTargetTf.gameObject;

            while (!_targetHit && targetValid)
            {
                if (_currentTargetTf == null || targetObj.activeInHierarchy == false)
                {
                    _myRigidBody.velocity = transform.forward * speed;

                    targetValid = false;
                }
                else
                {
                    Vector3 moveDirection = (_currentTargetTf.position - transform.position).normalized;
                    _myRigidBody.velocity = moveDirection * speed;
                    transform.LookAt(_currentTargetTf);
                }

                yield return null;
            }

            _homingCorRunning = false;
        }

        private IEnumerator LifeTimer()
        {
            _lifeTimerRunning = true;

            yield return new WaitForSeconds(maxLifeTime);

            _lifeTimerRunning = false;

            if (projectileMovementType == BulletMovementType.Homing)
            {
                if (_homingCorRunning)
                {
                    StopCoroutine(_homingMovementCor);
                    _homingCorRunning = false;
                }
            }

            if (!_targetHit)
            {
                _myRigidBody.velocity = Vector3.zero;

                _myCollider.enabled = false;
                meshObject.SetActive(false);
                
                PoolBack();
            }
        }
       
        private void OnTriggerEnter(Collider other)
        {
            if (_targetHit)
            {
                return;
            }

            _targetHit = true;

            if (projectileMovementType == BulletMovementType.Homing)
            {
                if (_homingCorRunning)
                {
                    StopCoroutine(_homingMovementCor);
                    _homingCorRunning = false;
                }
            }

            if (_lifeTimerRunning)
            {
                StopCoroutine(_lifeTimerCor);
                _lifeTimerRunning = false;
            }

            _myRigidBody.velocity = Vector3.zero;

            _myCollider.enabled = false;
            meshObject.SetActive(false);

            effectsObject.SetActive(true);
            
            other.transform.GetComponent<AttributesManager>()?.TakeDamage((int)baseDamage);

            Invoke(nameof(PoolBack), poolDelay);
        }

        public void PoolBack()
        {
            Pool.Free(this);
        }

        public enum BulletMovementType
        {
            Homing, 
            Aimed
        }
    }
}