using Cinemachine;
using DG.Tweening;
using not_this_again.Weapons;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace not_this_again.Character
{
    public class CharacterCombat: MonoBehaviour
    {
        public Data.CombatData combatData;
        
        public CinemachineVirtualCamera aimCamera;
        public Rig aimRig;
        public Transform headTarget;
        public Weapons.Weapon weapon;     
        
        public RandomAudioPlay hurtAudio;
        public RandomAudioPlay deathAudio;
        public RandomAudioPlay attackAudio;

        public bool isAiming;

        public bool inAttack;
        public bool inCombo;
        
        readonly int _hashMeleeAttack = Animator.StringToHash("MeleeAttack");
        readonly int _hashCombo1 = Animator.StringToHash("EllenCombo1");
        readonly int _hashCombo2 = Animator.StringToHash("EllenCombo2");
        readonly int _hashCombo3 = Animator.StringToHash("EllenCombo3");
        readonly int _hashCombo4 = Animator.StringToHash("EllenCombo4");
        readonly int _hashDash = Animator.StringToHash("Dash");

        public void EquipWeapon(bool b, Animator animator)
        {
            weapon.gameObject.SetActive(b);
            inAttack = false;
            inCombo = b;

            if (!b)
                animator.ResetTrigger(_hashMeleeAttack);
        }

        public bool IsWeaponEquipped(CharacterAnimator animator)
        {
            bool equipped = animator.m_NextStateInfo.shortNameHash == _hashCombo1 || 
                            animator.m_CurrentStateInfo.shortNameHash == _hashCombo1;
            
            equipped |= animator.m_NextStateInfo.shortNameHash == _hashCombo2 || 
                        animator.m_CurrentStateInfo.shortNameHash == _hashCombo2;
            
            equipped |= animator.m_NextStateInfo.shortNameHash == _hashCombo3 || 
                        animator.m_CurrentStateInfo.shortNameHash == _hashCombo3;
            
            equipped |= animator.m_NextStateInfo.shortNameHash == _hashCombo4 || 
                        animator.m_CurrentStateInfo.shortNameHash == _hashCombo4;
            
            equipped |= animator.m_NextStateInfo.shortNameHash == _hashDash ||
                        animator.m_CurrentStateInfo.shortNameHash == _hashDash;

            return equipped;
        }

        public void SetAim(bool aim, CinemachineFreeLook gameCamera)
        {
            isAiming = aim;

            if (aim)
            {
                transform.rotation = Quaternion.Euler(0f, gameCamera.m_XAxis.Value, 0f);
                aimCamera.m_Priority = 11;
                DOVirtual.Float(aimRig.weight, 1f, 0.2f, SetAimRigWeight);
            }
            else
            {
                aimCamera.m_Priority = 9;
                DOVirtual.Float(aimRig.weight, 0, .2f, SetAimRigWeight);
            }
            void SetAimRigWeight(float weight)
            {
                aimRig.weight = weight;
            }
        }

        public void Aim(PlayerInput input)
        {
            var rot = headTarget.localRotation.eulerAngles;
            rot.x -= input.CameraInput.y;
            if (rot.x > 180)
                rot.x -= 360;
            rot.x = Mathf.Clamp(rot.x, -80, 80);
            headTarget.localRotation = Quaternion.Slerp(headTarget.localRotation, Quaternion.Euler(rot), .5f);

            rot = transform.eulerAngles;
            rot.y += input.CameraInput.x;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rot), .5f);
        }
    }
}