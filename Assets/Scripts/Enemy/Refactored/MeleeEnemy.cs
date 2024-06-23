using Enemy.Refactored.Utilities;
using FinalScripts.Refactored.Attacker;
using UnityEngine;

namespace FinalScripts.Refactored
{
    [DefaultExecutionOrder(100)]
    [RequireComponent(typeof(RagdollReplacer), typeof(EnemyBase))]
    public class MeleeEnemy : MonoBehaviour
    {
        public EnemyBase Controller => _controller;
        public AttributesManager Target => _target;
        public TargetDistributor.TargetFollower FollowerData => _followerInstance;

        public Vector3 OriginalPosition { get; protected set; }
        [System.NonSerialized] public float AttackDistance = 3;

        public MeleeAttacker meleeWeapon;
        public TargetScanner playerScanner;
        public float timeToStopPursuit;

        [Header("Audio")]
        public RandomAudioPlayer attackAudio;
        public RandomAudioPlayer footstepAudio;
        public RandomAudioPlayer hitAudio;
        public RandomAudioPlayer gruntAudio;
        public RandomAudioPlayer deathAudio;
        public RandomAudioPlayer spottedAudio;

        protected float TimerSinceLostTarget = 0.0f;

        private AttributesManager _target = null;
        private EnemyBase _controller;
        private TargetDistributor.TargetFollower _followerInstance = null; 
        
        protected void OnEnable()
        {
            _controller = GetComponentInChildren<EnemyBase>();
            OriginalPosition = transform.position;
            meleeWeapon.SetOwner(gameObject); 
            _controller.Animator.Play("Idle", 0, Random.value);

            SceneLinkedSMB<MeleeEnemy>.Initialise(_controller.Animator, this);
        }

        /// <summary>
        /// Called by animation events.
        /// </summary>
        void PlayStep()
        {
            if (footstepAudio != null)
            {
                footstepAudio.PlayRandomClip();
            }
        }

        /// <summary>
        /// Called by animation events.
        /// </summary>
        public void Grunt()
        {
            if (gruntAudio != null)
            {
                gruntAudio.PlayRandomClip();
            }
        }

        public void Spotted()
        {
            if (spottedAudio != null)
            {
                spottedAudio.PlayRandomClip();
            }
        }

        protected void OnDisable()
        {
            if (_followerInstance != null)
            {
                _followerInstance.Distributor.UnregisterFollower(_followerInstance);
            }
        }

        private void FixedUpdate()
        {
            _controller.Animator.SetBool("Grounded", _controller.Grounded);
        }

        public void FindTarget()
        {
            AttributesManager target = playerScanner.Detect(transform, _target == null);

            if (_target == null)
            {
                if (target != null)
                {
                    _controller.Animator.SetTrigger("Spotted");
                    _target = target;
                    TargetDistributor distributor = target.GetComponentInChildren<TargetDistributor>();
                    
                    if (distributor != null)
                    {
                        _followerInstance = distributor.RegisterNewFollower();
                    }
                }
            }
            else
            {
                if (target == null)
                {
                    TimerSinceLostTarget += Time.deltaTime;

                    if (TimerSinceLostTarget >= timeToStopPursuit)
                    {
                        Vector3 toTarget = _target.transform.position - transform.position;

                        if (toTarget.sqrMagnitude > playerScanner.detectionRadius * playerScanner.detectionRadius)
                        {
                            if (_followerInstance != null)
                                _followerInstance.Distributor.UnregisterFollower(_followerInstance);

                            //the target move out of range, reset the target
                            _target = null;
                        }
                    }
                }
                else
                {
                    if (target != _target)
                    {
                        if (_followerInstance != null)
                        {
                            _followerInstance.Distributor.UnregisterFollower(_followerInstance);
                        }

                        _target = target;

                        TargetDistributor distributor = target.GetComponentInChildren<TargetDistributor>();
                        
                        if (distributor != null)
                        {
                            _followerInstance = distributor.RegisterNewFollower();
                        }
                    }

                    TimerSinceLostTarget = 0.0f;
                }
            }
        }

        public void StartPursuit()
        {
            if (_followerInstance != null)
            {
                _followerInstance.RequireSlot = true;
                RequestTargetPosition();
            }

            _controller.Animator.SetBool("Pursuit", true);
        }

        public void StopPursuit()
        {
            if (_followerInstance != null)
            {
                _followerInstance.RequireSlot = false;
            }

            _controller.Animator.SetBool("Pursuit", false);
        }

        public void RequestTargetPosition()
        {
            Vector3 fromTarget = transform.position - _target.transform.position;
            fromTarget.y = 0;

            Debug.Log("Follower Instance is: " + _followerInstance.ToString());

            _followerInstance.RequiredPoint = _target.transform.position + fromTarget.normalized * AttackDistance * 0.9f;
        }

        public void WalkBackToBase()
        {
            if (_followerInstance != null)
            {
                _followerInstance.Distributor.UnregisterFollower(_followerInstance);
            }
            
            tag = null;
            
            StopPursuit();
            
            _controller.SetTarget(OriginalPosition);
            _controller.SetFollowNavmeshAgent(true);
        }

        public void TriggerAttack()
        {
            _controller.Animator.SetTrigger("Attack");
        }

        public void AttackBegin()
        {
            meleeWeapon.BeginAttack(false);
        }

        public void AttackEnd()
        {
            meleeWeapon.EndAttack();
        }

        public void OnReceiveMessage(bool die, Transform damager, int amount)
        {
            if (die)
            {
                Death(damager);
            }
            else
            {
                ApplyDamage(damager, amount);
            }
        }

        public void Death(Transform damager)
        {
            Vector3 pushForce = transform.position - damager.position;

            pushForce.y = 0;

            transform.forward = -pushForce.normalized;
            _controller.AddForce(pushForce.normalized * 7.0f - Physics.gravity * 0.6f);

            _controller.Animator.SetTrigger("Hit");
            _controller.Animator.SetTrigger("Thrown");

            deathAudio.transform.SetParent(null, true);
            deathAudio.PlayRandomClip();
            Destroy(deathAudio, deathAudio.Clip == null ? 0.0f : deathAudio.Clip.length + 0.5f);
        }

        public void ApplyDamage(Transform damager, int amount)
        {
            Vector3 pushForce = transform.position - damager.position;
            pushForce.y = 0;

            transform.forward = -pushForce.normalized;
            _controller.AddForce(pushForce.normalized * 5.5f, false);

            _controller.Animator.SetTrigger("Hit");
            hitAudio.PlayRandomClip();
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            playerScanner.EditorGizmo(transform);
        }
#endif
    }
}