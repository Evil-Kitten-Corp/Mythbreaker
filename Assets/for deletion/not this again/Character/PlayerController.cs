using System.Collections.Generic;
using Cinemachine;
using not_this_again.Abilities;
using not_this_again.Weapons;
using UnityEngine;

namespace not_this_again.Character
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
    public class PlayerController : MonoBehaviour, IDamageable
    {
        public CharacterMovement movement;
        public CharacterCombat combat;
        public CharacterAnimator animator;
        
        public CinemachineFreeLook gameCamera;
        public Ability[] abilities;
        
        private Animator _anim;
        private PlayerInput _mInput;
        private CharacterController _mCharCtrl;
        private Renderer[] _renderers;
        private float _idleTimer;
        private bool _canAttack;
        private Data.CombatData _combatData;

        private bool IsMoveInput => !Mathf.Approximately(_mInput.MoveInput.sqrMagnitude, 0f);

        /* Interface Stuff */
        public List<GameObject> DamageMessageReceivers { get; set; }
        public bool IsInvulnerable { get; set; }

        public Data.CombatData CombatData => combat.combatData;

        void Reset()
        {
            combat.weapon = GetComponentInChildren<Weapons.Weapon>();

            Transform footStepSource = transform.Find("FootstepSource");
            
            if (footStepSource != null)
                movement.footsteps = footStepSource.GetComponent<RandomAudioPlay>();

            Transform hurtSource = transform.Find("HurtSource");
            
            if (hurtSource != null)
                combat.hurtAudio = hurtSource.GetComponent<RandomAudioPlay>();

            Transform landingSource = transform.Find("LandingSource");
            
            if (landingSource != null)
                movement.landing = landingSource.GetComponent<RandomAudioPlay>();
        }
        
        void Awake()
        {
            _mInput = GetComponent<PlayerInput>();
            _anim = GetComponent<Animator>();
            _mCharCtrl = GetComponent<CharacterController>();

            combat.weapon.SetOwner(this);
        }
        
        void OnEnable()
        {
            //SceneLinkedSMB<PlayerController>.Initialise(animator, this);

            DamageMessageReceivers.Add(this.gameObject);
            IsInvulnerable = true;

            combat.EquipWeapon(false, _anim);

            _renderers = GetComponentsInChildren<Renderer>();
        }

        // Called automatically by Unity whenever the script is disabled.
        void OnDisable()
        {
            DamageMessageReceivers.Remove(this.gameObject);

            foreach (var t in _renderers)
            {
                t.enabled = true;
            }
        }

        // Called automatically by Unity once every Physics step.
        void FixedUpdate()
        {
            animator.CacheAnimatorState();
            animator.UpdateInputBlocking(_mInput);
            combat.EquipWeapon(combat.IsWeaponEquipped(animator), _anim);

            _anim.SetFloat(animator.m_HashStateTime, Mathf.Repeat(_anim.GetCurrentAnimatorStateInfo(0)
                .normalizedTime, 1f));
            
            _anim.ResetTrigger(animator.m_HashMeleeAttack);

            if (_mInput.Aim != combat.isAiming)
                combat.SetAim(_mInput.Aim, gameCamera);

            if (_mInput.Attack && _canAttack)
                if (_mInput.Aim)
                    abilities[0].TriggerAbility();
                else
                    _anim.SetTrigger(animator.m_HashMeleeAttack);

            if (_mInput.Ability1)
                abilities[1].TriggerAbility();
            if (_mInput.Ability2 && _mInput.Aim)
                abilities[2].TriggerAbility();
            
            CalculateForwardMovement();
            movement.CalculateVerticalMovement(_mInput, combat.inCombo);
            
            if (_mInput.Aim)
                combat.Aim(_mInput);
            else
            {
                SetTargetRotation();
                if (movement.IsOrientationUpdated(animator, combat) && IsMoveInput)
                    UpdateOrientation();
            }

            PlayAudio();
            TimeoutToIdle();

            movement.previouslyGrounded = movement.isGrounded;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public void ApplyDamage(DamageMessage data)
        {
        }

        public void TakeDamage(IDamageable mOwner, float f)
        {
            
        }

        public void SetLookAt(GameObject mOwnerGameObject, float f, float f1)
        {
            
        }

        void PlayAudio()
        {
            float footfallCurve = _anim.GetFloat(animator.m_HashFootFall);

            if (footfallCurve > 0.01f && !movement.footsteps.playing && movement.footsteps.canPlay)
            {
                movement.footsteps.playing = true;
                movement.footsteps.canPlay = false;
                movement.footsteps.PlayRandomClip();
            }
            else if (movement.footsteps.playing)
            {
                movement.footsteps.playing = false;
            }
            else if (footfallCurve < 0.01f && !movement.footsteps.canPlay)
            {
                movement.footsteps.canPlay = true;
            }

            if (movement.isGrounded && !movement.previouslyGrounded)
            {
                movement.landing.PlayRandomClip();
            }

            if (!movement.isGrounded && movement.previouslyGrounded && movement.verticalSpeed > 0f)
            {
                movement.jumping.PlayRandomClip();
            }

            if (animator.m_CurrentStateInfo.shortNameHash == animator.m_HashHurt && 
                animator.m_PreviousCurrentStateInfo.shortNameHash != animator.m_HashHurt)
            {
                combat.hurtAudio.PlayRandomClip();
            }

            if (animator.m_CurrentStateInfo.shortNameHash == animator.m_HashEllenDeath && 
                animator.m_PreviousCurrentStateInfo.shortNameHash != animator.m_HashEllenDeath)
            {
                combat.deathAudio.PlayRandomClip();
            }

            if (animator.m_CurrentStateInfo.shortNameHash == animator.m_HashEllenCombo1 && 
                animator.m_PreviousCurrentStateInfo.shortNameHash != animator.m_HashEllenCombo1 ||
                animator.m_CurrentStateInfo.shortNameHash == animator.m_HashEllenCombo2 && 
                animator.m_PreviousCurrentStateInfo.shortNameHash != animator.m_HashEllenCombo2 ||
                animator.m_CurrentStateInfo.shortNameHash == animator.m_HashEllenCombo3 && 
                animator.m_PreviousCurrentStateInfo.shortNameHash != animator.m_HashEllenCombo3 ||
                animator.m_CurrentStateInfo.shortNameHash == animator.m_HashEllenCombo4 && 
                animator.m_PreviousCurrentStateInfo.shortNameHash != animator.m_HashEllenCombo4)
            {
                combat.attackAudio.PlayRandomClip();
            }
        }
        
        public void TimeoutToIdle()
        {
            bool inputDetected = IsMoveInput || _mInput.Attack || _mInput.JumpInput;
            if (movement.isGrounded && !inputDetected)
            {
                _idleTimer += Time.deltaTime;

                if (_idleTimer >= movement.idleTimeout)
                {
                    _idleTimer = 0f;
                    _anim.SetTrigger(animator.m_HashTimeoutToIdle);
                }
            }
            else
            {
                _idleTimer = 0f;
                _anim.ResetTrigger(animator.m_HashTimeoutToIdle);
            }

            _anim.SetBool(animator.m_HashInputDetected, inputDetected);
        }
        
        public void CalculateForwardMovement()
        {
            // Cache the move input and cap it's magnitude at 1.
            Vector2 moveInput = _mInput.MoveInput;
            if (moveInput.sqrMagnitude > 1f)
                moveInput.Normalize();

            // Calculate the speed intended by input.
            movement.desiredForwardSpeed = moveInput.magnitude * movement.maxForwardSpeed;

            // Determine change to speed based on whether there is currently any move input.
            float acceleration = IsMoveInput ? CharacterMovement.KGroundAcceleration : CharacterMovement.KGroundDeceleration;

            if (combat.isAiming)
                movement.desiredForwardSpeed = Mathf.Clamp(moveInput.magnitude * movement.maxForwardSpeed, 0, 4);

            // Adjust the forward speed towards the desired speed.
            movement.ForwardSpeed = Mathf.MoveTowards(movement.ForwardSpeed, movement.desiredForwardSpeed, acceleration * Time.deltaTime);

            // Set the animator parameter to control what animation is being played.
            _anim.SetFloat(animator.m_HashForwardSpeed, movement.ForwardSpeed);
        }
        
        public void SetTargetRotation()
        {
            // Create three variables, move input local to the player, flattened forward direction of the camera and a
            // local target rotation.
            Vector2 moveInput = _mInput.MoveInput;
            Vector3 localMovementDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

            Vector3 forward = Quaternion.Euler(0f, gameCamera.m_XAxis.Value, 0f) * Vector3.forward;
            forward.y = 0f;
            forward.Normalize();

            Quaternion targetRotation;

            // If the local movement direction is the opposite of forward then the target rotation should be towards the camera.
            if (Mathf.Approximately(Vector3.Dot(localMovementDirection, Vector3.forward), -1.0f))
            {
                targetRotation = Quaternion.LookRotation(-forward);
            }
            else
            {
                // Otherwise the rotation should be the offset of the input from the camera's forward.
                Quaternion cameraToInputOffset = Quaternion.FromToRotation(Vector3.forward, localMovementDirection);
                targetRotation = Quaternion.LookRotation(cameraToInputOffset * forward);
            }

            // The desired forward direction of Ellen.
            Vector3 resultingForward = targetRotation * Vector3.forward;

            // If attacking try to orient to close enemies.
            if (combat.inAttack)
            {
                // Find all the enemies in the local area.
                Vector3 centre = transform.position + transform.forward * 2.0f + transform.up;
                Vector3 halfExtents = new Vector3(3.0f, 1.0f, 2.0f);
                int layerMask = 1 << LayerMask.NameToLayer("Enemy");
                int count = Physics.OverlapBoxNonAlloc(centre, halfExtents, movement.OverlapResult, 
                    targetRotation, layerMask);

                // Go through all the enemies in the local area...
                float closestDot = 0.0f;
                Vector3 closestForward = Vector3.zero;
                int closest = -1;

                for (int i = 0; i < count; ++i)
                {
                    // ... and for each get a vector from the player to the enemy.
                    Vector3 playerToEnemy = movement.OverlapResult[i].transform.position - transform.position;
                    playerToEnemy.y = 0;
                    playerToEnemy.Normalize();

                    // Find the dot product between the direction the player wants to go and the direction to the enemy.
                    // This will be larger the closer to Ellen's desired direction the direction to the enemy is.
                    float d = Vector3.Dot(resultingForward, playerToEnemy);

                    // Store the closest enemy.
                    if (d > CharacterMovement.KMinEnemyDotCoeff && d > closestDot)
                    {
                        closestForward = playerToEnemy;
                        closestDot = d;
                        closest = i;
                    }
                }

                // If there is a close enemy...
                if (closest != -1)
                {
                    // The desired forward is the direction to the closest enemy.
                    resultingForward = closestForward;

                    // We also directly set the rotation, as we want snappy fight and orientation isn't
                    // updated in the UpdateOrientation function during an atatck.
                    transform.rotation = Quaternion.LookRotation(resultingForward);
                }
            }

            // Find the difference between the current rotation of the player and the desired rotation of the
            // player in radians.
            float angleCurrent = Mathf.Atan2(transform.forward.x, transform.forward.z) * Mathf.Rad2Deg;
            float targetAngle = Mathf.Atan2(resultingForward.x, resultingForward.z) * Mathf.Rad2Deg;

            movement.AngleDiff = Mathf.DeltaAngle(angleCurrent, targetAngle);
            movement.TargetRotation = targetRotation;
        }
        
        public void UpdateOrientation()
        {
            _anim.SetFloat(animator.m_HashAngleDeltaRad, movement.AngleDiff * Mathf.Deg2Rad);

            Vector3 localInput = new Vector3(_mInput.MoveInput.x, 0f, _mInput.MoveInput.y);
            float groundedTurnSpeed = Mathf.Lerp(movement.maxTurnSpeed, movement.minTurnSpeed, 
                movement.ForwardSpeed / movement.desiredForwardSpeed);
            float actualTurnSpeed = movement.isGrounded ? groundedTurnSpeed : 
                Vector3.Angle(transform.forward, localInput) * CharacterMovement.KInverseOneEighty * 
                CharacterMovement.KAirborneTurnSpeedProportion * groundedTurnSpeed;
            
            movement.TargetRotation = Quaternion.RotateTowards(transform.rotation, movement.TargetRotation, 
                actualTurnSpeed * Time.deltaTime);

            transform.rotation = movement.TargetRotation;
        }
    }
}