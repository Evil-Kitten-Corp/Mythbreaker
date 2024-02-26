using System;
using System.Collections;
using UnityEngine;

namespace Characters
{
    public class PlayerMovement : MonoBehaviour
    {
        public PlayerControls InputActions;
        private bool isJumping = false;
        private bool canFireHookShot = false;
        private Coroutine hookShotCooldownCoroutine;

        [SerializeField] private float hookShotCooldownDuration = 2f;
        
        // Start is called before the first frame update
        void Start()
        {
            InputActions = new PlayerControls();
            InputActions.Default.Attack.performed += _ => OnAttack();
            InputActions.Default.Jump.performed += _ => Jump();
            InputActions.Default.Hookshot.performed += _ => HookShot();
        }

        private void OnAttack()
        {
            throw new NotImplementedException();
        }
        
        private void Jump()
        {
            if (!isJumping)
            {
                // Perform jump action
                isJumping = true;
            }
            else if (isJumping)
            {
                // Start the hook shot cooldown timer
                if (hookShotCooldownCoroutine != null)
                {
                    StopCoroutine(hookShotCooldownCoroutine);
                }
                hookShotCooldownCoroutine = StartCoroutine(StartHookShotCooldown());
            }
        }

        private void HookShot()
        {
            if (canFireHookShot)
            {
                // Perform hook shot action
                // Reset the cooldown timer
                if (hookShotCooldownCoroutine != null)
                {
                    StopCoroutine(hookShotCooldownCoroutine);
                }
                hookShotCooldownCoroutine = StartCoroutine(StartHookShotCooldown());
            }
        }

        private IEnumerator StartHookShotCooldown()
        {
            canFireHookShot = true;
            yield return new WaitForSeconds(hookShotCooldownDuration);
            canFireHookShot = false;
        }

        private void OnEnable()
        {
            InputActions.Enable();
        }

        private void OnDisable()
        {
            InputActions.Disable();
        }
    }
}
