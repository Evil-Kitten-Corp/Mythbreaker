using System.Collections.Generic;
using System.Linq;
using BrunoMikoski.ServicesLocation;
using ExtEvents;
using UnityEngine;
using UnityEngine.UI;

namespace Abilities
{
    public class PlayerInventory : MonoBehaviour
    {
        public GameObject inventoryUI;
        public GameObject abilityUIPrefab;
        
        public List<AbilityInfo> equippedAbilities;
        public List<AbilityInfo> obtainedAbilities;

        public ExtEvent<AbilityInfo> onNewAbility = new();

        private void Start()
        {
            ServiceLocator.Instance.RegisterInstance(this);

            obtainedAbilities.AddRange(equippedAbilities);
            var temp = obtainedAbilities.Distinct().ToList();
            obtainedAbilities.Clear();

            foreach (var ab in temp)
            {
                AddAbility(ab);
            }
        }

        private void InstantiateSlot(AbilityInfo ability)
        {
            var ab = Instantiate(abilityUIPrefab, inventoryUI
                .GetComponentInChildren<LayoutGroup>().transform);
            
            ab.GetComponent<AbilityUI>().SetAbility(ability);
        }

        public void AddAbility(AbilityInfo ability)
        {
            if (!IsAlreadyInInventory(ability))
            {
                return;
            }

            obtainedAbilities.Add(ability);
            SendAbility(ability);
            ability.onAbilityEquipped.DynamicListeners += EquipAbility;
            ability.onAbilityUnequipped.DynamicListeners += UnequipAbility;
            
            InstantiateSlot(ability);
        }

        private void SendAbility(AbilityInfo ability)
        {
            onNewAbility.Invoke(ability);
        }

        private void EquipAbility(AbilityInfo ability)
        {
            equippedAbilities.Add(ability);
        }

        private void UnequipAbility(AbilityInfo ability)
        {
            equippedAbilities.Remove(ability);
        }

        private bool IsAlreadyInInventory(AbilityInfo slot)
        {
            return obtainedAbilities.Exists(a => a == slot);
        }
    }
}