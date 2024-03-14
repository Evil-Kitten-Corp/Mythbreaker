using System.Collections.Generic;
using Abilities;
using BrunoMikoski.ServicesLocation;
using UnityEngine;

namespace Base
{
    public class PlayerCharacter : MonoBehaviour
    {
        public GameObject abilityHolder;
        public PlayerInventory inventory;

        private Dictionary<AbilityInfo, AbilityBase> _activeInstances;

        public void Start()
        {
            ServiceLocator.Instance.RegisterInstance(this);
            
            inventory.onNewAbility.DynamicListeners += ListenToAbility;
        }

        private void ListenToAbility(AbilityInfo obj)
        {
            obj.onAbilityEquipped.DynamicListeners += OnAbilityEquipped;
            obj.onAbilityUnequipped.DynamicListeners += OnAbilityUnequipped;
        }

        public AbilityBase GetAbilityInstance(AbilityInfo info)
        {
            return _activeInstances[info];
        }

        private void OnAbilityUnequipped(AbilityInfo arg1)
        {
            Destroy(_activeInstances[arg1].gameObject);
            _activeInstances.Remove(arg1);
        }

        private void OnAbilityEquipped(AbilityInfo arg1)
        {
            _activeInstances.Add(arg1, arg1.Add());
        }
    }
}