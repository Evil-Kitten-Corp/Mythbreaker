using System;
using System.Collections.Generic;
using System.Linq;
using Abilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FinalScripts
{
    public class RewardSystem : MonoBehaviour
    {
        public List<AbilityData> abilities;
        public List<AbilityUpgrade> abilityUpgrades;
        public Action<Reward> GiveRewards;

        public RewardView rewardScreen;
        
        private PlayerInv _player;

        private void Start()
        {
            _player = FindObjectOfType<PlayerInv>();
        }

        public void TriggerRewards()
        {
            rewardScreen.gameObject.SetActive(true);
            rewardScreen.SetRewards(ChooseRewards(), delegate(Reward reward)
            {
                GiveRewards.Invoke(reward);
            });
        }
        
        public Reward[] ChooseRewards()
        {
            List<Reward> possibleRewards = (from ability in abilities 
                where !_player.HasAbility(ability) 
                select new Reward { type = RewardType.Ability, ability = ability }).ToList();
            
            possibleRewards.AddRange(from upgrade in abilityUpgrades 
                where !_player.HasUpgrade(upgrade) 
                select new Reward { type = RewardType.AbilityUpgrade, abilityUpgrade = upgrade });

            List<Reward> chosenRewards = new List<Reward>();

            for (int i = 0; i < 3 && possibleRewards.Count > 0; i++)
            {
                int index = Random.Range(0, possibleRewards.Count);
                Reward reward = possibleRewards[index];
                possibleRewards.RemoveAt(index);
                chosenRewards.Add(reward);
            }

            return chosenRewards.ToArray();
        }
    }
}