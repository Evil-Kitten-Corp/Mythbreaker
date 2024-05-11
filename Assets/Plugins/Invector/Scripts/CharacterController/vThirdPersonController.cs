﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Invector.vCharacterController
{
    public class vThirdPersonController : vThirdPersonAnimator
    {
        private void Start()
        {
            currentStamina = maxStamina;
            //staminaBar.maxValue = maxStamina;
            //staminaBar.value = currentStamina;
        }

        private void Update()
        {
            if (isSprinting)
            {
                UseStamina(staminaDecay * Time.deltaTime);
            }
        }

        public virtual void ControlAnimatorRootMotion()
        {
            if (!this.enabled) return;

            if (inputSmooth == Vector3.zero)
            {
                transform.position = animator.rootPosition;
                transform.rotation = animator.rootRotation;
            }

            if (useRootMotion)
                MoveCharacter(moveDirection);
        }

        public virtual void ControlLocomotionType()
        {
            if (lockMovement) return;

            if (locomotionType.Equals(LocomotionType.FreeWithStrafe) && !isStrafing || locomotionType.Equals(LocomotionType.OnlyFree))
            {
                SetControllerMoveSpeed(freeSpeed);
                SetAnimatorMoveSpeed(freeSpeed);
            }
            else if (locomotionType.Equals(LocomotionType.OnlyStrafe) || locomotionType.Equals(LocomotionType.FreeWithStrafe) && isStrafing)
            {
                isStrafing = true;
                SetControllerMoveSpeed(strafeSpeed);
                SetAnimatorMoveSpeed(strafeSpeed);
            }

            if (!useRootMotion)
                MoveCharacter(moveDirection);
        }

        public virtual void ControlRotationType()
        {
            if (lockRotation) return;

            bool validInput = input != Vector3.zero || (isStrafing ? strafeSpeed.rotateWithCamera : freeSpeed.rotateWithCamera);

            if (validInput)
            {
                // calculate input smooth
                inputSmooth = Vector3.Lerp(inputSmooth, input, (isStrafing ? strafeSpeed.movementSmooth : freeSpeed.movementSmooth) * Time.deltaTime);

                Vector3 dir = (isStrafing && (!isSprinting || sprintOnlyFree == false) || (freeSpeed.rotateWithCamera && input == Vector3.zero)) && rotateTarget ? rotateTarget.forward : moveDirection;
                RotateToDirection(dir);
            }
        }

        public virtual void UpdateMoveDirection(Transform referenceTransform = null)
        {
            if (input.magnitude <= 0.01)
            {
                moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, (isStrafing ? strafeSpeed.movementSmooth : freeSpeed.movementSmooth) * Time.deltaTime);
                return;
            }

            if (referenceTransform && !rotateByWorld)
            {
                //get the right-facing direction of the referenceTransform
                var right = referenceTransform.right;
                right.y = 0;
                //get the forward direction relative to referenceTransform Right
                var forward = Quaternion.AngleAxis(-90, Vector3.up) * right;
                // determine the direction the player will face based on input and the referenceTransform's right and forward directions
                moveDirection = (inputSmooth.x * right) + (inputSmooth.z * forward);
            }
            else
            {
                moveDirection = new Vector3(inputSmooth.x, 0, inputSmooth.z);
            }
        }

        public virtual void Sprint(bool value)
        {
            var sprintConditions = (input.sqrMagnitude > 0.1f && isGrounded &&
                !(isStrafing && !strafeSpeed.walkByDefault && (horizontalSpeed >= 0.5 || horizontalSpeed <= -0.5 || verticalSpeed <= 0.1f)));

            if (value && sprintConditions && CanSprint())
            {
                if (input.sqrMagnitude > 0.1f)
                {
                    if (isGrounded && useContinuousSprint)
                    {
                        isSprinting = !isSprinting;

                        if (isSprinting)
                        {
                            ShowStamina(true);
                            UseStamina(staminaDecay);
                        }
                    }
                    else if (!isSprinting)
                    {
                        isSprinting = true;
                        ShowStamina(true);
                        UseStamina(staminaDecay);
                    }
                }
                else if (!useContinuousSprint && isSprinting)
                {
                    ShowStamina(false);
                    isSprinting = false;
                }
            }
            else if (!CanSprint())
            {
                ShowStamina(false);
                isSprinting = false;
            }
            else if (isSprinting)
            {
                ShowStamina(false);
                isSprinting = false;
            }
        }

        private bool CanSprint()
        {
            return currentStamina > 0 && allowStamina;
        }

        public virtual void Strafe()
        {
            isStrafing = !isStrafing;
        }

        public virtual void Jump()
        {
            // trigger jump behaviour
            jumpCounter = jumpTimer;
            isJumping = true;

            // trigger jump animations
            if (input.sqrMagnitude < 0.1f)
                animator.CrossFadeInFixedTime("Jump", 0.1f);
            else
                animator.CrossFadeInFixedTime("JumpMove", .2f);
        }

        #region Stamina Sys
        
        #region Stamina
        [Header("Stamina")]
        public bool allowStamina;
        public float currentStamina;
        public float maxStamina;
        public float staminaDecay;
        public float staminaRegen;
        public Slider staminaBar;
        private Coroutine _staminaRegenCoroutine;

        #endregion

        private void ShowStamina(bool t)
        {
            staminaBar.gameObject.SetActive(t);
        }

        private void UseStamina(float qty)
        {
            if (currentStamina - qty >= 0)
            {
                currentStamina -= qty;
                staminaBar.value = currentStamina;

                if (_staminaRegenCoroutine != null)
                {
                    StopCoroutine(_staminaRegenCoroutine);
                }
                
                _staminaRegenCoroutine = StartCoroutine(RegenStamina());
            }
            else
            {
                Debug.Log("Not enough stamina!");
            }
        }

        private IEnumerator RegenStamina()
        {
            yield return new WaitForSeconds(2);

            while (currentStamina < maxStamina)
            {
                currentStamina += maxStamina / 100;
                staminaBar.value = currentStamina;
                yield return new WaitForSeconds(staminaRegen);
            }

            _staminaRegenCoroutine = null;
        }

        #endregion
    }
}