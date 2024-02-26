using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace TEMP_SCRIPTS.Hack_And_Slash_Test.Scripts.Character
{
    public class CharacterMovement : MonoBehaviour
    {
        [Header("[Character Movement]")] public Character Character;
        [SerializeField] private Vector3 InputVelocity;
        [SerializeField] private Vector3 InputDirection;
        [SerializeField] private Vector3 DesiredMoveDirection;
        [SerializeField] private Vector3 StrafeMoveDirection;
        public Vector3 AirMoveDirection;

        [Header("[Character Movement - Turn]")]
        public bool IsCheckTurn;

        public float Angle;
        public float TurnAngle = 150f;
        private float TurnDelayTime;

        [Header("[Character Movement - Coroutine]")]
        private Coroutine MoveTurnCoroutine;

        [Header("[Draw Gizmos]")] public bool IsDrawGizmos;

        public Vector3 GetInputDirection => InputDirection;

        public Vector3 GetDesiredMoveDirection => DesiredMoveDirection;

        public Vector3 GetStrafeMoveDirection => StrafeMoveDirection;

        public bool IsTurn => Vector3.Angle(transform.forward, GetDesiredMoveDirection) >= (double)TurnAngle;

        private void Update()
        {
            SetEssentialValues();
            CacheValues();
            UpdateCharacterInfo();
        }

        private void OnDrawGizmos()
        {
            if (!IsDrawGizmos)
                return;
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position + transform.TransformDirection(0.0f, 1f, 0.0f),
                InputDirection.normalized * 5f * Character.CharacterAnim.GetFloat("Speed"));
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position + transform.TransformDirection(0.0f, 1f, 0.0f),
                DesiredMoveDirection.normalized * 5f * Character.CharacterAnim.GetFloat("Speed"));
        }

        private void SetEssentialValues()
        {
            if (!Character.LocomotionData.IsGrounded)
                return;
            Character.LocomotionData.Acceleration = Vector3.Lerp(Character.LocomotionData.Acceleration,
                Character.LocomotionData.CalculateAcceleraction(),
                Time.deltaTime * Character.LocomotionData.AccelerationLerpSpeed);
            Character.LocomotionData.Speed =
                new Vector3(Character.LocomotionData.Velocity.x, 0.0f, Character.LocomotionData.Velocity.z)
                    .sqrMagnitude;
            Character.LocomotionData.LerpSpeed = Mathf.Lerp(Character.LocomotionData.LerpSpeed,
                Character.LocomotionData.Speed, Time.deltaTime);
            Character.LocomotionData.IsMoving = Character.LocomotionData.Speed > 0.10000000149011612;
            Character.CharacterAnim.SetBool("IsMove", Character.LocomotionData.IsMoving);
            if (Character.LocomotionData.IsMoving)
                Character.LocomotionData.LastVelocityRotation =
                    Quaternion.LookRotation(new Vector3(0.0f, 0.0f, Character.LocomotionData.Velocity.z), Vector3.up);
            Character.LocomotionData.InputAmount = InputVelocity.sqrMagnitude;
            Character.LocomotionData.HasInput = Character.LocomotionData.InputAmount > 0.0;
            Character.LocomotionData.MovementInputAmount =
                Character.LocomotionData.GetCurrentAcceleration().sqrMagnitude /
                Character.LocomotionData.GetMaxAcceleration();
            Character.LocomotionData.HasMovementInput = Character.LocomotionData.MovementInputAmount > 0.0;
            if (Character.LocomotionData.HasMovementInput)
                Character.LocomotionData.LastMovementInputRotation = Quaternion.LookRotation(
                    new Vector3(0.0f, 0.0f, Character.LocomotionData.GetCurrentAcceleration().z), Vector3.up);
            Character.LocomotionData.AimYawRate =
                Mathf.Abs((Character.LocomotionData.GetControlRotation().z - Character.LocomotionData.PreviousAimYaw) /
                          Time.deltaTime);
        }

        private void CacheValues()
        {
            Character.LocomotionData.PreviousVelocity = Character.LocomotionData.Velocity;
        }

        private void SetMovementState()
        {
            switch (Character.LocomotionData.MovementState)
            {
                case eMovementState.Grounded:
                    UpdateMovementValues();
                    UpdateRotationValues();
                    MoveTurn();
                    break;
                case eMovementState.InAir:
                    UpdateMovementValues();
                    UpdateRotationValues();
                    break;
            }
        }

        private void UpdateCharacterInfo()
        {
            var vector2 = MonoSingleton<InputSystemManager>.instance.GetInputAction.Locomotion.Move
                .ReadValue<Vector2>();
            InputVelocity = new Vector3(vector2.x, 0.0f, vector2.y);
            SetMovementState();
        }

        private void UpdateMovementValues()
        {
            Character.LocomotionData.VelocityBlend = Character.LocomotionData.InterpVelocityBlend(
                Character.LocomotionData.VelocityBlend, Character.LocomotionData.CalculateVelocityBlend(Character),
                Character.LocomotionData.VelocityBlendLerpSpeed, Time.deltaTime);
            var forward = Camera.main.transform.forward;
            var right = Camera.main.transform.right;
            forward.y = 0.0f;
            right.y = 0.0f;
            forward.Normalize();
            right.Normalize();
            InputDirection = forward * InputVelocity.z + right * InputVelocity.x;
            InputDirection.Normalize();
            DesiredMoveDirection = Vector3.Lerp(DesiredMoveDirection,
                forward * InputVelocity.z + right * InputVelocity.x,
                Time.deltaTime * Character.LocomotionData.RotationSpeed);
            DesiredMoveDirection.Normalize();
            StrafeMoveDirection = Vector3.Lerp(StrafeMoveDirection,
                new Vector3(
                    (float)(DesiredMoveDirection.x * (double)transform.right.x +
                            DesiredMoveDirection.z * (double)transform.right.z), 0.0f,
                    (float)(DesiredMoveDirection.x * (double)transform.forward.x +
                            DesiredMoveDirection.z * (double)transform.forward.z)),
                Time.deltaTime * Character.LocomotionData.RotationSpeed);
            if (Character.LocomotionData.LockedRotation)
                return;
            var num = (int)Character.CharacterController.Move(DesiredMoveDirection *
                                                              ((!Character.CharacterAnim.applyRootMotion
                                                                  ? GetStateSpeed()
                                                                  : 1f) * Character.Ratio * Time.deltaTime));
            if (Character.LocomotionData.IsGrounded)
            {
                if (!Character.LocomotionData.HasInput)
                {
                    if (Character.LocomotionData.CharacterMoveMode == eCharacterMoveMode.Directional && !IsTurn)
                        if (Character.LocomotionData.CharacterState != eCharacterState.Walk)
                            ;
                    Character.LocomotionData.CharacterState = eCharacterState.Idle;
                    Character.LocomotionData.IsSprint = false;
                    Character.CharacterAnim.SetBool("IsSprint", false);
                }
                else
                {
                    Character.LocomotionData.CharacterState = Character.LocomotionData.IsSprint
                        ? eCharacterState.Sprint
                        : eCharacterState.Walk;
                }
            }

            Character.LocomotionData.Velocity = Character.LocomotionData.GetVelocity(Character);
            Character.LocomotionData.RelativeAccelerationAmount =
                Character.LocomotionData.CalculateRelativeAccelerationAmount(Character);
            Character.LocomotionData.LeanAmount.LR = Mathf.Lerp(Character.LocomotionData.LeanAmount.LR,
                Mathf.Clamp(Character.LocomotionData.RelativeAccelerationAmount.x * Character.LocomotionData.Gait, -1f,
                    1f), Time.deltaTime * Character.LocomotionData.LeanLerpSpeed);
            Character.LocomotionData.LeanAmount.FB = Mathf.Lerp(Character.LocomotionData.LeanAmount.FB,
                Mathf.Clamp(Character.LocomotionData.RelativeAccelerationAmount.z * Character.LocomotionData.Gait, -1f,
                    1f), Time.deltaTime * Character.LocomotionData.LeanLerpSpeed);
        }

        private void UpdateRotationValues()
        {
            if (Character.LocomotionData.LockedRotation)
                return;
            switch (Character.LocomotionData.CharacterMoveMode)
            {
                case eCharacterMoveMode.Directional:
                    if (Character.LocomotionData.IsGrounded)
                    {
                        if (!Character.LocomotionData.IsMoving)
                            break;
                        transform.rotation = Quaternion.Slerp(transform.rotation,
                            Quaternion.LookRotation(DesiredMoveDirection),
                            Time.deltaTime * Character.LocomotionData.RotationSpeed);
                        break;
                    }

                    if (!Character.LocomotionData.IsMoving)
                        break;
                    transform.rotation = Quaternion.Slerp(transform.rotation,
                        Quaternion.LookRotation(DesiredMoveDirection),
                        Time.deltaTime * Character.LocomotionData.AirRotationSpeed);
                    break;
                case eCharacterMoveMode.Strafe:
                    if (!Character.LocomotionData.IsGrounded)
                        break;
                    StrafeTurn_Update();
                    break;
            }
        }

        private IEnumerator CheckTurn()
        {
            IsCheckTurn = true;
            yield return new WaitWhile(() => Character.LocomotionData.InputAmount < 0.5f);
            var elapsedTime = 0.25f;
            while (elapsedTime > 0.0 && Character.LocomotionData.IsGrounded && !Character.LocomotionData.LockedRotation)
            {
                elapsedTime -= Time.deltaTime;
                if (IsTurn && TurnDelayTime <= (double)Time.time)
                {
                    TurnDelayTime = Time.time + 0.15f;
                    Character.CharacterAnim.CrossFadeInFixedTime("Turn_Blend", 0.25f);
                    Character.CharacterAnim.SetFloat("Direction",
                        Vector3.SignedAngle(Character.transform.forward, GetDesiredMoveDirection, Vector3.up));
                    Character.LocomotionData.IsSprint = true;
                    Character.CharacterAnim.SetBool("IsSprint", true);
                    elapsedTime = 0.0f;
                }

                yield return new WaitForFixedUpdate();
            }

            yield return new WaitWhile(() => Character.LocomotionData.InputAmount >= 0.5f);
            IsCheckTurn = false;
        }

        private void MoveTurn()
        {
            if (IsCheckTurn || !Character.LocomotionData.IsGrounded ||
                Character.CharacterAnim.GetFloat("Speed") <= 0.5 ||
                Character.LocomotionData.CharacterMoveMode != eCharacterMoveMode.Directional)
                return;
            MoveTurnCoroutine = StartCoroutine(CheckTurn());
        }

        private void StrafeTurn()
        {
            var angle = Vector3.SignedAngle(transform.forward, Camera.main.transform.forward, Vector3.up);
            Character.CharacterAnim.SetFloat("Angle", angle);
            if (angle >= 45f)
            {
                if (TurnDelayTime <= Time.time)
                {
                    TurnDelayTime = Time.time + 0.25f;
                    Character.CharacterAnim.CrossFadeInFixedTime("Strafe Turn_L", 0.1f);
                    var forward2 = Camera.main.transform.forward;
                    forward2.y = 0f;
                    Character.transform.DORotateQuaternion(Quaternion.LookRotation(forward2), 0.4f)
                        .SetEase(Ease.Linear);
                }
            }
            else if (angle <= -45f && TurnDelayTime <= Time.time)
            {
                TurnDelayTime = Time.time + 0.25f;
                Character.CharacterAnim.CrossFadeInFixedTime("Strafe Turn_R", 0.1f);
                var forward = Camera.main.transform.forward;
                forward.y = 0f;
                Character.transform.DORotateQuaternion(Quaternion.LookRotation(forward), 0.4f).SetEase(Ease.Linear);
            }
        }

        private void StrafeTurn_Update()
        {
            if (Character.LocomotionData.IsMoving) return;
            var angle = Vector3.SignedAngle(transform.forward, Camera.main.transform.forward, Vector3.up);
            Character.CharacterAnim.SetFloat("Angle", angle);
            if (angle >= 45f)
            {
                if (TurnDelayTime <= Time.time)
                {
                    TurnDelayTime = Time.time + 0.35f;
                    Character.CharacterAnim.CrossFadeInFixedTime("Strafe Turn_R", 0.2f);
                    var forward2 = Camera.main.transform.forward;
                    forward2.y = 0f;
                    forward2.Normalize();
                    Character.transform.rotation = Quaternion.Slerp(Character.transform.rotation,
                        Quaternion.LookRotation(forward2), Time.deltaTime * Character.LocomotionData.RotationSpeed);
                }
            }
            else if (angle <= -45f && TurnDelayTime <= Time.time)
            {
                TurnDelayTime = Time.time + 0.35f;
                Character.CharacterAnim.CrossFadeInFixedTime("Strafe Turn_L", 0.2f);
                var forward = Camera.main.transform.forward;
                forward.y = 0f;
                forward.Normalize();
                Character.transform.rotation = Quaternion.Slerp(Character.transform.rotation,
                    Quaternion.LookRotation(forward), Time.deltaTime * Character.LocomotionData.RotationSpeed);
            }
        }

        public Vector3 ClampVector(Vector3 vector, float minValue, float maxValue)
        {
            vector.x = Mathf.Clamp(vector.x, minValue, maxValue);
            vector.y = Mathf.Clamp(vector.y, minValue, maxValue);
            vector.z = Mathf.Clamp(vector.z, minValue, maxValue);
            return vector;
        }

        public float GetStateSpeed()
        {
            Character.LocomotionData.CurrentMovementSettings.CurrentSpeed =
                Character.LocomotionData.CharacterState switch
                {
                    eCharacterState.Idle => Mathf.Lerp(Character.LocomotionData.CurrentMovementSettings.CurrentSpeed,
                        0.0f, Time.deltaTime),
                    eCharacterState.Walk => Mathf.Lerp(Character.LocomotionData.CurrentMovementSettings.CurrentSpeed,
                        Character.LocomotionData.AnimationCurveData.WalkCurve.Evaluate(Character.LocomotionData
                            .CurrentMovementSettings.WalkSpeed), Time.deltaTime),
                    eCharacterState.Run => Mathf.Lerp(Character.LocomotionData.CurrentMovementSettings.CurrentSpeed,
                        Character.LocomotionData.AnimationCurveData.RunCurve.Evaluate(Character.LocomotionData
                            .CurrentMovementSettings.RunSpeed), Time.deltaTime),
                    eCharacterState.Sprint => Mathf.Lerp(Character.LocomotionData.CurrentMovementSettings.CurrentSpeed,
                        Character.LocomotionData.AnimationCurveData.RunCurve.Evaluate(Character.LocomotionData
                            .CurrentMovementSettings.SprintSpeed), Time.deltaTime),
                    _ => Character.LocomotionData.CurrentMovementSettings.CurrentSpeed
                };

            return Character.LocomotionData.CurrentMovementSettings.CurrentSpeed * Character.Ratio;
        }
    }
}