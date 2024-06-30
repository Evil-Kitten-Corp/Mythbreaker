using System.Collections.Generic;
using System.Linq;
using Abilities;
using AYellowpaper.SerializedCollections;
using DefaultNamespace;
using SolidUtilities.Collections;
using TriInspector;
using UnityEngine;

namespace FinalScripts
{
    public class PlayerInv : MonoBehaviour
    {
        private WaveFinal _rewardSystem;
        private SaveLoadJsonFormatter _saveSystem;

        public List<AbilitySlot> abilitySlots;

        [SerializedDictionary("Ability", "Upgrades")] [ReadOnly] 
        public SerializableDictionary<AbilityData, List<AbilityUpgrade>> abilities;

        private readonly Dictionary<int, KeyCode> _slotToKeyDict = new()
        {
            {0, KeyCode.Alpha1},
            {1, KeyCode.Alpha2},
            {2, KeyCode.Alpha3},
            {3, KeyCode.Alpha4},
            {4, KeyCode.Alpha5}
        };

        private void Start()
        {
            _rewardSystem = FindObjectOfType<WaveFinal>();
            _saveSystem = FindObjectOfType<SaveLoadJsonFormatter>();

            //LoadGameInventory();
        }

        /*private void LoadGameInventory()
        {
            _saveSystem.LoadGame(out List<string> abIds);

            if (abIds is { Count: > 0 })
            {
                foreach (var id in abIds)
                {
                    abilities.Keys.First(x => x.id == id).Unlock?.Invoke();
                }
            }
        }*/

        public void ReceiveRewards(Reward obj)
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
            if (ability.canGetMoreThanOnce)
            {
                return;
            }
            
            _rewardSystem.rewards[ability].abilityKey = _slotToKeyDict[slot];
            abilitySlots.First(x => x.slot == slot + 1).SetAbility(ability); 
        }
    }
}