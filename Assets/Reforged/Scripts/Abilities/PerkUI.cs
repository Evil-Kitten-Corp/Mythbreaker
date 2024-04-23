using UnityEngine;
using UnityEngine.UI;

namespace Abilities
{
    public class PerkUI : MonoBehaviour
    {
        public Image iconSlot;
        
        private AbilityData _container = null;
        
        public string Name => _container != null ? _container.abName : "Undiscovered";

        public string Description => _container != null ? _container.abDescription : "Play more to discover " +
            "this ability/upgrade.";

        public Sprite Icon => _container != null ? _container.icon : iconSlot.sprite;

        public string God => _container != null ? _container.god : "";

        public void SetState(bool discovered, AbilityData ab = null)
        {
            if (discovered && ab != null)
            {
                _container = ab;
                iconSlot.sprite = ab.icon;
            }
        }
    }
}