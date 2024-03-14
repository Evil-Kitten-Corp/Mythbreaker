using System;
using not_this_again.Weapons;
using UnityEngine;

namespace not_this_again.Character
{
    [Serializable]
    public class CharacterMovement
    {
        public float maxForwardSpeed = 8f;        // How fast you can run.
        public float gravity = 20f;               // How fast you accelerate downwards when airborne.
        public float jumpSpeed = 10f;             // How fast you take off when jumping.
        public float minTurnSpeed = 400f;         // How fast you turn when moving at maximum speed.
        public float maxTurnSpeed = 1200f;        // How fast you turn when stationary.
        public float idleTimeout = 5f;            // How long before you start considering random idles.
        
        public RandomAudioPlay footsteps;         
        public RandomAudioPlay landing;
        public RandomAudioPlay jumping;

        /* Constants */
        internal const float KAirborneTurnSpeedProportion = 5.4f;
        internal const float KGroundedRayDistance = 1f;
        internal const float KJumpAbortSpeed = 10f;
        internal const float KMinEnemyDotCoeff = 0.2f;
        internal const float KInverseOneEighty = 1f / 180f;
        internal const float KStickingGravityProportion = 0.3f;
        internal const float KGroundAcceleration = 20f;
        internal const float KGroundDeceleration = 25f;

        public bool isGrounded = true;            // Whether or not we're currently standing on the ground.
        public bool previouslyGrounded = true;    // Whether or not we're standing on the ground last frame.
        protected bool ReadyToJump;      // Whether or not the input state and the player are correct to allow jumping.
        public float desiredForwardSpeed;         // How fast we aim to be going along the ground based on input.
        protected internal float ForwardSpeed;    // How fast we're currently going along the ground.
        public float verticalSpeed;       
        
        protected internal Quaternion TargetRotation;  // What rotation we're aiming to have based on input.
        protected internal float AngleDiff;       // Angle in degrees between current rotation and target rotation.
        protected internal Collider[] OverlapResult = new Collider[8];

        // Called each physics step.
        public void CalculateVerticalMovement(PlayerInput input, bool inCombo)
        {
            // If jump is not currently held and Ellen is on the ground then she is ready to jump.
            if (!input.JumpInput && isGrounded)
                ReadyToJump = true;

            if (isGrounded)
            {
                // When grounded we apply a slight negative vertical speed to make Ellen "stick" to the ground.
                verticalSpeed = -gravity * KStickingGravityProportion;

                // If jump is held, Ellen is ready to jump and not currently in the middle of a melee combo...
                if (input.JumpInput && ReadyToJump && !inCombo)
                {
                    // ... then override the previously set vertical speed and make sure she cannot jump again.
                    verticalSpeed = jumpSpeed;
                    isGrounded = false;
                    ReadyToJump = false;
                }
            }
            else
            {
                // If Ellen is airborne, the jump button is not held and Ellen is currently moving upwards...
                if (!input.JumpInput && verticalSpeed > 0.0f)
                {
                    // ... decrease Ellen's vertical speed.
                    // This is what causes holding jump to jump higher that tapping jump.
                    verticalSpeed -= KJumpAbortSpeed * Time.deltaTime;
                }

                // If a jump is approximately peaking, make it absolute.
                if (Mathf.Approximately(verticalSpeed, 0f))
                {
                    verticalSpeed = 0f;
                }

                // If Ellen is airborne, apply gravity.
                verticalSpeed -= gravity * Time.deltaTime;
            }
        }

        // Called each physics step to help determine whether Ellen can turn under player input.
        public bool IsOrientationUpdated(CharacterAnimator anim, CharacterCombat combat)
        {
            bool updateOrientationForLocomotion = !anim.m_IsAnimatorTransitioning && 
                anim.m_CurrentStateInfo.shortNameHash == anim.m_HashLocomotion || 
                anim.m_NextStateInfo.shortNameHash == anim.m_HashLocomotion;
            
            bool updateOrientationForAirborne = !anim.m_IsAnimatorTransitioning && 
                anim.m_CurrentStateInfo.shortNameHash == anim.m_HashAirborne || 
                anim.m_NextStateInfo.shortNameHash == anim.m_HashAirborne;
            
            bool updateOrientationForLanding = !anim.m_IsAnimatorTransitioning && 
                anim.m_CurrentStateInfo.shortNameHash == anim.m_HashLanding || 
                anim.m_NextStateInfo.shortNameHash == anim.m_HashLanding;

            return updateOrientationForLocomotion || updateOrientationForAirborne || updateOrientationForLanding || 
                   combat.inCombo && !combat.inAttack;
        }
    }
}