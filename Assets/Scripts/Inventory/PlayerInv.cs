using System.Collections.Generic;
using System.Linq;
using Abilities;
using AYellowpaper.SerializedCollections;
using SolidUtilities.Collections;
using TriInspector;
using UnityEngine;

namespace FinalScripts
{
    public class PlayerInv : MonoBehaviour
    {
        private RewardSystem _rewardSystem;
        private SaveLoadJsonFormatter _saveSystem;

        public List<AbilitySlot> abilitySlots;

        [SerializedDictionary("Ability", "Upgrades")] [ReadOnly] 
        public SerializableDictionary<AbilityData, List<AbilityUpgrade>> abilities;

        private readonly Dictionary<int, KeyCode> _slotToKeyDict = new()
        {
            {1, KeyCode.Alpha1},
            {2, KeyCode.Alpha2},
            {3, KeyCode.Alpha3},
            {4, KeyCode.Alpha4},
            {5, KeyCode.Alpha5}
        };

        private void Start()
        {
            _rewardSystem = FindObjectOfType<RewardSystem>();
            _saveSystem = FindObjectOfType<SaveLoadJsonFormatter>();
            _rewardSystem.GiveRewards += ReceiveRewards;

            LoadGameInventory();
        }

        private void LoadGameInventory()
        {
            _saveSystem.LoadGame(out List<string> abIds);

            if (abIds is { Count: > 0 })
            {
                foreach (var id in abIds)
                {
                    abilities.Keys.First(x => x.id == id).Unlock?.Invoke();
                }
            }
        }

        private void ReceiveRewards(Reward obj)
        {
            switch (obj.type)
            {
                case RewardType.Ability:
                    abilities.Add(obj.ability, new List<AbilityUpgrade>());
                    obj.ability.Unlock?.Invoke();
                    break;
                
                case RewardType.AbilityUpgrade:
                    abilities[obj.abilityUpgrade.associatedAbility].Add(obj.abilityUpgrade);
                    break;
            }
        }

        public bool HasAbility(AbilityData ability)
        {
            return abilities.ContainsKey(ability);
        }

        public bool HasUpgrade(AbilityUpgrade upgrade)
        {
            return HasAbility(upgrade.associatedAbility) && abilities[upgrade.associatedAbility].Contains(upgrade);
        }

        public void SetAbilityOnSlot(AbilityData ability, int slot)
        {
            int i = _rewardSystem.abilities.IndexOf(ability);
            _rewardSystem.abilityScripts[i].abilityKey = _slotToKeyDict[i];
            abilitySlots.First(x => x.slot == slot).SetAbility(ability);
        }
    }
}