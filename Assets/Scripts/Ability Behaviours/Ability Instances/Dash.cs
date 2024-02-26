using System.Collections;
using UnityEngine;

namespace Ability_Behaviours.Ability_Instances
{
    public class Dash : Ability
    {
        [Header("[ References ]")]
        public Transform orientation;
        public Transform playerCam;
        private Rigidbody rb;

        [Header("[ Properties ]")]
        public float dashForce;
        public float dashUpwardForce;
        public float maxDashYSpeed;
        public float dashDuration;

        [Header("[ Settings ]")]
        public bool useCameraForward = true;
        public bool allowAllDirections = true;
        public bool disableGravity = false;
        public bool resetVel = true;
        
        private Vector3 _delayedForceToApply;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        protected override IEnumerator Apply()
        {
            //pm.dashing = true;
            //pm.maxYSpeed = maxDashYSpeed;

            var forwardT = useCameraForward ? playerCam : orientation;

            Vector3 direction = GetDirection(forwardT);
            Vector3 forceToApply = direction * dashForce + orientation.up * dashUpwardForce;

            if (disableGravity)
                rb.useGravity = false;

            _delayedForceToApply = forceToApply;
            
            Invoke(nameof(DelayedDashForce), 0.025f);
            Invoke(nameof(ResetDash), dashDuration);
            
            return base.Apply();
        }
        
        private void DelayedDashForce()
        {
            if (resetVel)
                rb.velocity = Vector3.zero;

            rb.AddForce(_delayedForceToApply, ForceMode.Impulse);
        }

        private void ResetDash()
        {
            //pm.dashing = false;
            //pm.maxYSpeed = 0;

            if (disableGravity)
                rb.useGravity = true;
        }

        private Vector3 GetDirection(Transform forwardT)
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");

            Vector3 direction;

            if (allowAllDirections)
                direction = forwardT.forward * verticalInput + forwardT.right * horizontalInput;
            else
                direction = forwardT.forward;

            if (verticalInput == 0 && horizontalInput == 0)
                direction = forwardT.forward;

            return direction.normalized;
        }
    }
}