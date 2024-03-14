using System;
using UnityEngine;

namespace not_this_again.Character
{
    [Serializable]
    public class CharacterAnimator
    {
        protected Animator Animator;

        #region Variables

        public AnimatorStateInfo m_CurrentStateInfo;
        public AnimatorStateInfo m_NextStateInfo;
        protected internal bool m_IsAnimatorTransitioning;
        public AnimatorStateInfo m_PreviousCurrentStateInfo;   
        protected AnimatorStateInfo m_PreviousNextStateInfo;
        protected bool m_PreviousIsAnimatorTransitioning;
        
        readonly int m_HashAirborneVerticalSpeed = Animator.StringToHash("AirborneVerticalSpeed");
        public readonly int m_HashForwardSpeed = Animator.StringToHash("ForwardSpeed");
        internal readonly int m_HashAngleDeltaRad = Animator.StringToHash("AngleDeltaRad");
        public readonly int m_HashTimeoutToIdle = Animator.StringToHash("TimeoutToIdle");
        readonly int m_HashGrounded = Animator.StringToHash("Grounded");
        public readonly int m_HashInputDetected = Animator.StringToHash("InputDetected");
        public readonly int m_HashMeleeAttack = Animator.StringToHash("MeleeAttack");
        public readonly int m_HashHurt = Animator.StringToHash("Hurt");
        readonly int m_HashDeath = Animator.StringToHash("Death");
        readonly int m_HashRespawn = Animator.StringToHash("Respawn");
        readonly int m_HashHurtFromX = Animator.StringToHash("HurtFromX");
        readonly int m_HashHurtFromY = Animator.StringToHash("HurtFromY");
        public readonly int m_HashStateTime = Animator.StringToHash("StateTime");
        public readonly int m_HashFootFall = Animator.StringToHash("FootFall");

        // States
        internal readonly int m_HashLocomotion = Animator.StringToHash("Locomotion");
        internal readonly int m_HashAirborne = Animator.StringToHash("Airborne");
        internal readonly int m_HashLanding = Animator.StringToHash("Landing");    // Also a parameter.
        public readonly int m_HashEllenCombo1 = Animator.StringToHash("EllenCombo1");
        public readonly int m_HashEllenCombo2 = Animator.StringToHash("EllenCombo2");
        public readonly int m_HashEllenCombo3 = Animator.StringToHash("EllenCombo3");
        public readonly int m_HashEllenCombo4 = Animator.StringToHash("EllenCombo4");
        readonly int m_HashEllenDash = Animator.StringToHash("Dash");
        public readonly int m_HashEllenDeath = Animator.StringToHash("EllenDeath");

        // Tags
        readonly int m_HashBlockInput = Animator.StringToHash("BlockInput");

        #endregion
        
        // Called at the start of FixedUpdate to record the current state of the base layer of the animator.
        public void CacheAnimatorState()
        {
            m_PreviousCurrentStateInfo = m_CurrentStateInfo;
            m_PreviousNextStateInfo = m_NextStateInfo;
            m_PreviousIsAnimatorTransitioning = m_IsAnimatorTransitioning;

            m_CurrentStateInfo = Animator.GetCurrentAnimatorStateInfo(0);
            m_NextStateInfo = Animator.GetNextAnimatorStateInfo(0);
            m_IsAnimatorTransitioning = Animator.IsInTransition(0);
        }

        // Called after the animator state has been cached to determine whether this script should block user input.
        public void UpdateInputBlocking(PlayerInput mInput)
        {
            bool inputBlocked = m_CurrentStateInfo.tagHash == m_HashBlockInput && !m_IsAnimatorTransitioning;
            inputBlocked |= m_NextStateInfo.tagHash == m_HashBlockInput;
            mInput.playerControllerInputBlocked = inputBlocked;
        }

        // Called each physics step (so long as the Animator component is set to Animate Physics) after
        // FixedUpdate to override root motion.
        void OnAnimatorMove(Transform transform, CharacterMovement move, CharacterController m_CharCtrl)
        {
            Vector3 movement;

            // If Ellen is on the ground...
            if (move.isGrounded)
            {
                // ... raycast into the ground...
                RaycastHit hit;
                Ray ray = new Ray(transform.position + Vector3.up * CharacterMovement.KGroundedRayDistance 
                                                                  * 0.5f, -Vector3.up);
                if (Physics.Raycast(ray, out hit, CharacterMovement.KGroundedRayDistance, Physics.AllLayers, 
                        QueryTriggerInteraction.Ignore))
                {
                    // ... and get the movement of the root motion rotated to lie along the plane of the ground.
                    movement = Vector3.ProjectOnPlane(Animator.deltaPosition, hit.normal);
                }
                else
                {
                    // If no ground is hit just get the movement as the root motion.
                    // Theoretically this should rarely happen as when grounded the ray should always hit.
                    movement = Animator.deltaPosition;
                }
            }
            else
            {
                // If not grounded the movement is just in the forward direction.
                movement = move.ForwardSpeed * transform.forward * Time.deltaTime;
            }

            // Rotate the transform of the character controller by the animation's root rotation.
            m_CharCtrl.transform.rotation *= Animator.deltaRotation;

            // Add to the movement with the calculated vertical speed.
            movement += move.verticalSpeed * Vector3.up * Time.deltaTime;

            // Move the character controller.
            m_CharCtrl.Move(movement);

            // After the movement store whether or not the character controller is grounded.
            move.isGrounded = m_CharCtrl.isGrounded;

            // If Ellen is not on the ground then send the vertical speed to the animator.
            // This is so the vertical speed is kept when landing so the correct landing animation is played.
            if (!move.isGrounded)
                Animator.SetFloat(m_HashAirborneVerticalSpeed, move.verticalSpeed);

            // Send whether or not Ellen is on the ground to the animator.
            Animator.SetBool(m_HashGrounded, move.isGrounded);
        }
    }
}