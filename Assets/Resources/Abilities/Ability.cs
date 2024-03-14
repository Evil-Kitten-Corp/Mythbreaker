using System.Collections.Generic;
using Ability_Behaviours;
using BrunoMikoski.ScriptableObjectCollections;
using TNRD;
using TriInspector;
using TypeReferences;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Collections.Resources.Abilities
{
    public class Ability : RarityChooser
    {
        public string itemName;
        public string itemDescription;
        public InputActionReference input;
        public Sprite itemImg;
        public Sprite itemIcon;
        public Rarities rarity;

        [SerializeReference] public List<IAbilityBehaviour> listReferenceField = new();

        public string ItemName => itemName;

        public string ItemDescription => itemDescription;

        public Sprite ItemImg => itemImg;

        public Sprite ItemIcon => itemIcon;

        public Rarities Rarity => rarity;
    }
}
