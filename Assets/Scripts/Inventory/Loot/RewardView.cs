using System;
using UnityEngine;

namespace FinalScripts
{
    public class RewardView : MonoBehaviour
    {
        public RewardSingleView[] rewardViews;

        private Reward[] _currentRewards;
        private Action<Reward> _onRewardSelected;

        public void SetRewards(Reward[] rewards, Action<Reward> onRewardSelected)
        {
            _currentRewards = rewards;
            _onRewardSelected = onRewardSelected;

            for (int i = 0; i < rewardViews.Length; i++)
            {
                if (i < rewards.Length)
                {
                    rewardViews[i].gameObject.SetActive(true);
                    rewardViews[i].Setup(rewards[i], OnRewardClicked);
                }
                else
                {
                    rewardViews[i].gameObject.SetActive(false);
                }
            }

            gameObject.SetActive(true); // Show the reward UI
        }

        private void OnRewardClicked(Reward reward)
        {
            _onRewardSelected?.Invoke(reward);
            gameObject.SetActive(false); // Hide the reward UI
        }
    }
}