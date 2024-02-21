using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Level
{
    public class LevelRewardInstance : MonoBehaviour
    {
        public Image image;
        public TMP_Text nameField;
        public TMP_Text descriptionField;
        
        public void SetReward(RarityChooser reward)
        {
            image.sprite = reward.ItemImg;
            nameField.text = reward.ItemName;
            descriptionField.text = reward.ItemDescription;
        }
    }
}