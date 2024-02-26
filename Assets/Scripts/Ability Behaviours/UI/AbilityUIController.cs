using System.Collections.Generic;
using BrunoMikoski.ServicesLocation;
using UnityEngine;

namespace Ability_Behaviours.UI
{
    public class AbilityUIController: MonoBehaviour
    {
        public List<AbilityUI> abilitySlots;

        private void Awake()
        {
            ServiceLocator.Instance.RegisterInstance(this);
        }

        public AbilityData GetAbilityBySlot(AbilitySlot slot)
        {
            return GetAbilityUI(slot).ability;
        }

        public AbilityUI GetAbilityUI(AbilityData data)
        {
            return abilitySlots.Find(x => x.ability == data);
        }

        public AbilityUI GetAbilityUI(AbilitySlot slot)
        {
            return abilitySlots.Find(x => x.slot == slot);
        }
    }
}