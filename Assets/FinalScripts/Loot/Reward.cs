using System;
using Abilities;
using UnityEngine;

namespace FinalScripts
{
    [Serializable]
    public class Reward
    {
        public RewardType type;
        public AbilityData ability;
        public AbilityUpgrade abilityUpgrade;

        public Sprite Sprite => type == RewardType.Ability ? ability.icon : abilityUpgrade.icon;
        public string Name => type == RewardType.Ability ? ability.abName : abilityUpgrade.displayName;
        public string Description => type == RewardType.Ability ? ability.abDescription : abilityUpgrade.description;
    }
}