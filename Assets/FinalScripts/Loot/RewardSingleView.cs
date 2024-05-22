using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FinalScripts
{
    public class RewardSingleView: MonoBehaviour
    {
        public Image image;
        public TMP_Text displayName;
        public TMP_Text description;

        private Reward _reward;
        private Action<Reward> _onRewardSelected;

        private void Awake()
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(OnButtonClick);
        }

        public void Setup(Reward reward, Action<Reward> onRewardSelected)
        {
            _reward = reward;
            _onRewardSelected = onRewardSelected;

            image.sprite = reward.Sprite;
            displayName.text = reward.Name;
            description.text = reward.Description;
        }

        private void OnButtonClick()
        {
            _onRewardSelected?.Invoke(_reward);
        }
    }
}