using Ability_Behaviours;
using BrunoMikoski.ScriptableObjectCollections;
using TypeReferences;
using UnityEngine;

namespace Collections.Resources.Abilities
{
    public class Ability : RarityChooser
    {
        public string itemName;
        public string itemDescription;
        public Sprite itemImg;
        public Sprite itemIcon;
        public Rarities rarity;

        [Inherits(typeof(IAbilityBehaviour))]
        [SerializeField] private TypeReference abilityBehaviour;
        
        public string ItemName => itemName;

        public string ItemDescription => itemDescription;

        public Sprite ItemImg => itemImg;

        public Sprite ItemIcon => itemIcon;

        public Rarities Rarity => rarity;
    }
}
