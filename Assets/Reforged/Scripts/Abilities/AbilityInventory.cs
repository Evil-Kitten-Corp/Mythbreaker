using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Abilities
{
    public class AbilityInventory : UIMenu
    {
        public LayoutGroup abilityDisplay;

        [Header("Ability Previewer")] 
        public Image abilityIcon;
        public TMP_Text abilityTitle;
        public TMP_Text abilityGod;
        public TMP_Text abilityCooldown;
        public TMP_Text abilityDescription;

        public void AddAbility(AbilityView ab)
        {
            ab.transform.parent = abilityDisplay.transform; 
        }

        public void DisplayAbilityInfo(AbilityView ab)
        {
            abilityIcon.sprite = ab.Data.icon;
            abilityTitle.text = ab.Data.abName;
            abilityGod.text = ab.Data.god;
            abilityCooldown.text = ab.Data.cooldown + "s";
            abilityDescription.text = ab.Data.abDescription;
        }
    }
}