using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace not_this_again.Character
{
    public class PlayerInput: MonoBehaviour
    {
        [HideInInspector]
        public bool playerControllerInputBlocked;

        protected Vector2 m_Movement;
        protected Vector2 m_Camera;
        protected bool m_Jump;
        protected bool m_Attack;
        protected bool m_Pause;
        protected bool m_Aim;
        protected bool m_Ability1;
        protected bool m_Ability2;
        protected bool m_ExternalInputBlocked;

        [SerializeField]
        private float mouseSensitivity = 1;

        public Vector2 MoveInput
        {
            get
            {
                if (playerControllerInputBlocked || m_ExternalInputBlocked)
                    return Vector2.zero;
                return m_Movement;
            }
        }

        public Vector2 CameraInput
        {
            get
            {
                if (playerControllerInputBlocked || m_ExternalInputBlocked)
                    return Vector2.zero;
                return m_Camera * mouseSensitivity;
            }
        }

        public bool JumpInput => m_Jump && !playerControllerInputBlocked && !m_ExternalInputBlocked;

        public bool Attack => m_Attack && !playerControllerInputBlocked && !m_ExternalInputBlocked;

        public bool Aim => m_Aim && !playerControllerInputBlocked && !m_ExternalInputBlocked;

        public bool Ability1 => m_Ability1 && !playerControllerInputBlocked && !m_ExternalInputBlocked;

        public bool Ability2 => m_Ability2 && !playerControllerInputBlocked && !m_ExternalInputBlocked;

        public bool Pause => m_Pause;

        WaitForSeconds _mAttackInputWait;
        Coroutine _mAttackWaitCoroutine;

        const float KAttackInputDuration = 0.03f;

        void Awake()
        {
            CinemachineCore.GetInputAxis = GetAxisCustom;
            _mAttackInputWait = new WaitForSeconds(KAttackInputDuration);
       }

        private float GetAxisCustom(string axisName)
        {
            if (axisName == "CameraX")
                return CameraInput.x;

            if (axisName == "CameraY")
                return CameraInput.y;

            return 0;
        }
        
        void OnMove(InputValue value)
        {
            m_Movement = value.Get<Vector2>();
        }
        
        void OnLook(InputValue value)
        {
            m_Camera = value.Get<Vector2>();
        }
        
        void OnFire()
        {

            if (_mAttackWaitCoroutine != null)
                StopCoroutine(_mAttackWaitCoroutine);

            _mAttackWaitCoroutine = StartCoroutine(AttackWait());
        }

        void OnAim(InputValue value)
        {
            m_Aim = value.isPressed;
        }

        void OnJump(InputValue value)
        {
            m_Jump = value.isPressed;
        }

        void OnAbility1(InputValue value)
        {

            m_Ability1 = value.isPressed;
        }
        void OnAbility2(InputValue value)
        {

            m_Ability2 = value.isPressed;
        }

        private IEnumerator AttackWait()
        {
            m_Attack = true;

            yield return _mAttackInputWait;

            m_Attack = false;
        }

        public bool HasControl()
        {
            return !m_ExternalInputBlocked;
        }

        public void ReleaseControl()
        {
            m_ExternalInputBlocked = true;
        }

        public void GainControl()
        {
            m_ExternalInputBlocked = false;
        }
    }
}