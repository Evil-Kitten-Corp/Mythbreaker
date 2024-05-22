using System.Collections.Generic;
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

        [SerializedDictionary("Ability", "Upgrades")] [ReadOnly] 
        public SerializableDictionary<AbilityData, List<AbilityUpgrade>> abilities;

        private void Start()
        {
            _rewardSystem = FindObjectOfType<RewardSystem>();
            _rewardSystem.GiveRewards += ReceiveRewards;
        }

        private void ReceiveRewards(Reward obj)
        {
            switch (obj.type)
            {
                case RewardType.Ability:
                    abilities.Add(obj.ability, new List<AbilityUpgrade>());
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
    }
}