using System;
using not_this_again.Enums;
using not_this_again.Weapons;
using UnityEngine;

namespace not_this_again.Utils
{
    /// <summary>
    /// Centralized class to call animation events
    /// </summary>
    public class AnimationEvent : MonoBehaviour
    {
        public Character.PlayerController owner;
        
        public void Anim_OnAttack(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                owner.combat.combatData.attackType = AttackType.LightAttack;
                owner.combat.combatData.attackDirection = AttackDirection.Front;
                Debug.LogError("Anim_OnAttack - parameter is null.");
            }
            else
            {
                owner.combat.combatData.attackType = (AttackType)Enum.Parse(typeof(AttackType), 
                    type.Split("/")[0]);
                
                owner.combat.combatData.attackDirection = (AttackDirection)Enum.Parse(typeof(AttackDirection), 
                    type.Split("/")[1]);
            }
            if (owner.combat.weapon != null && owner.combat.weapon is MeleeWeapon wpn)
            {
                wpn.WeaponCollider.enabled = true;
                wpn.PlayTrace();
            }
        }

        public void Anim_OffAttack()
        {
            if (owner.combat.weapon != null)
            {
                owner.combat.weapon.WeaponCollider.enabled = false;
            }
        }

        public void Anim_OnHit()
        {
            owner.combat.combatData.combatType = CombatType.HitReaction;
        }

        public void Anim_OffHit()
        {
            owner.combat.combatData.combatType = CombatType.None;
        }
    }
}