using Abilities;
using UnityEngine;
using UnityEngine.UI;

namespace FinalScripts
{
    public class AbilitySlot : MonoBehaviour
    {
        public int slot;
        public Image image;

        private AbilityData _abData;

        private AbilityData AbilityRepresented 
        {
            get => _abData;
            set
            {
                if (value == null)
                {
                    image.gameObject.SetActive(false);
                }
                else
                {
                    image.gameObject.SetActive(true);
                    image.sprite = value.icon;
                }

                _abData = value;
            }
        }

        private void Start()
        {
            AbilityRepresented = null;
        }

        public void SetAbility(AbilityData ab)
        {
            AbilityRepresented = ab;
        }
    }
}