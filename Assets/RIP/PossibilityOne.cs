using System;
using System.Collections.Generic;
using UnityEngine;

namespace RIP
{
    public class PossibilityOne : MonoBehaviour
    {
        public Animator anim;
        public List<TAttackTwo> attacks;
        private float _lastClickedTime;
        private float _lastComboEnd;
        private int _comboCounter;
        
        private static readonly int Attack1 = Animator.StringToHash("Attack");

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
            }
            
            ExitAttack();
        }

        void Attack()
        {
            if (Time.time - _lastComboEnd >= 0.5f && _comboCounter <= attacks.Count) 
            {
                CancelInvoke(nameof(EndCombo));

                if (Time.time - _lastClickedTime >= 0.2f)
                {
                    anim.applyRootMotion = false;
                    //anim.runtimeAnimatorController = attacks[_comboCounter].animatorOC;
                    anim.runtimeAnimatorController = attacks[_comboCounter % attacks.Count].animatorOC;
                    anim.SetTrigger(Attack1);
                    _comboCounter++;
                    _lastClickedTime = Time.time;

                    if (_comboCounter > attacks.Count)
                    {
                        _comboCounter = 0;
                    }
                }
            }
        }

        void ExitAttack()
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && 
                anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            {
                Invoke(nameof(EndCombo), 1);
            }
        }

        void EndCombo()
        {
            anim.applyRootMotion = true;
            _comboCounter = 0;
            _lastComboEnd = Time.time;
        }

        public float? GetCurrentAnimDmg()
        {
            if (attacks[_comboCounter % attacks.Count] != null)
                return attacks[_comboCounter % attacks.Count].damage;

            return null;
        }
    }
}