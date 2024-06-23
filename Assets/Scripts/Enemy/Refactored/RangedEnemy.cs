using Enemy.Refactored.Utilities;
using FinalScripts.Refactored.Attacker;
using UnityEditor;
using UnityEngine;

namespace FinalScripts.Refactored
{
    [DefaultExecutionOrder(100)]
    [RequireComponent(typeof(RagdollReplacer), typeof(EnemyBase))]
    public class RangedEnemy : MonoBehaviour
    {
        public TargetScanner playerScanner;
        public float fleeingDistance = 3.0f;
        public RangedAttacker rangeWeapon;

        [Header("Audio")]
        public RandomAudioPlayer attackAudio;
        public RandomAudioPlayer footstepAudio;
        public RandomAudioPlayer hitAudio;
        public RandomAudioPlayer gruntAudio;
        public RandomAudioPlayer deathAudio;
        public RandomAudioPlayer spottedAudio;

        public EnemyBase Controller => _controller;
        public AttributesManager Target => _target;

        private AttributesManager _target = null;
        private EnemyBase _controller;

        private bool _fleeing = false;
        private Vector3 _rememberedTargetPosition;

        protected void OnEnable()
        {
            _controller = GetComponentInChildren<EnemyBase>();
            _controller.Animator.Play("Idle", 0, Random.value);
            SceneLinkedSMB<RangedEnemy>.Initialise(_controller.Animator, this);
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
            Controller.AddForce(pushForce.normalized * 7.0f - Physics.gravity * 0.6f);

            Controller.Animator.SetTrigger("Hit");
            Controller.Animator.SetTrigger("Thrown");

            deathAudio.transform.SetParent(null, true);
            deathAudio.PlayRandomClip();

            Destroy(deathAudio, deathAudio.Clip == null ? 0.0f : deathAudio.Clip.length + 0.5f);
        }

        public void ApplyDamage(Transform damager, int amount)
        {
            Vector3 pushForce = transform.position - damager.position;
            pushForce.y = 0;

            transform.forward = -pushForce.normalized;
            Controller.AddForce(pushForce.normalized * 5.5f, false);

            Controller.Animator.SetTrigger("Hit");
            hitAudio.PlayRandomClip();
        }

        public void Shoot()
        {
            rangeWeapon.Attack(_rememberedTargetPosition);
        }

        public void TriggerAttack()
        {
            _controller.Animator.SetTrigger("Attack");
        }

        public void RememberTargetPosition()
        {
            if (_target == null)
                return;

            _rememberedTargetPosition = _target.transform.position;
        }

        void PlayStep(int frontFoot)
        {
            if (footstepAudio != null)
            {
                footstepAudio.PlayRandomClip();
            }
        }

        public void Grunt ()
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

        public void CheckNeedFleeing()
        {
            if (_target == null)
            {
                _fleeing = false;
                Controller.Animator.SetBool("Fleeing", _fleeing);
                return;
            }

            Vector3 fromTarget = transform.position - _target.transform.position;

            if (_fleeing || fromTarget.sqrMagnitude <= fleeingDistance * fleeingDistance)
            {
                Vector3 fleePoint = transform.position + fromTarget.normalized * 2 * fleeingDistance;

                Debug.DrawLine(fleePoint, fleePoint + Vector3.up * 10.0f);

                if (!_fleeing)
                {
                    Controller.SetFollowNavmeshAgent(true);
                }

                _fleeing = Controller.SetTarget(fleePoint);

                if (_fleeing)
                {
                    Controller.Animator.SetBool("Fleeing", _fleeing);
                }
            }

            if (_fleeing && fromTarget.sqrMagnitude > fleeingDistance * fleeingDistance * 4)
            {
                _fleeing = false;
                Controller.Animator.SetBool("Fleeing", _fleeing);
            }
        }

        public void FindTarget()
        {
            _target = playerScanner.Detect(transform, _target == null);
            _controller.Animator.SetBool("HasTarget", _target != null);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            playerScanner.EditorGizmo(transform);
        }
#endif
    }
    
    
#if UNITY_EDITOR
    [CustomEditor(typeof(RangedEnemy))]
    public class RangedBehaviourEditor : Editor
    {
        RangedEnemy _target;

        void OnEnable()
        {
            _target = target as RangedEnemy;
        }

        public override void OnInspectorGUI()
        {
            if (_target.playerScanner.detectionRadius < _target.fleeingDistance)
            {
                EditorGUILayout.HelpBox("The scanner detection radius is smaller than the fleeing range.\n" +
                    "The spitter will never shoot at the player as it will flee past the range at which it can see the player",
                    MessageType.Warning, true);    
            }
            
            base.OnInspectorGUI();
        }
    }

#endif
}