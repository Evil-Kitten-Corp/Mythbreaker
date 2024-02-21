using System.Collections.Generic;
using System.Linq;
using BrunoMikoski.AnimationSequencer;
using UnityEngine;

namespace Level
{
    public class LevelRewardsUI : MonoBehaviour
    {
        public LevelController levelController;
        public AnimationSequencerController sequencer;
        public List<LevelRewardInstance> rewardsList;

        private void Start()
        {
            levelController.onLevelComplete.DynamicListeners += GetRewards;
        }

        private void GetRewards()
        {
            var guaranteed = levelController.levelEndRewards.GetGuaranteeedLoot();
            
            var rand = levelController.levelEndRewards
                .GetRandomLoot(rewardsList.Count - guaranteed.Count);
            
            var rewards = guaranteed.Intersect(rand).ToList();

            for (int i = 0; i < rewards.Count; i++)
            {
                rewardsList[i].SetReward(rewards[i]);
            }
            
            sequencer.Play();
        }

        private void OnDestroy()
        {
            levelController.onLevelComplete.DynamicListeners -= GetRewards;
        }
    }
}