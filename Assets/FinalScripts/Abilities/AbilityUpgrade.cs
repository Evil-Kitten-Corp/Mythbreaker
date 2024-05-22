using Abilities;
using UnityEngine;

namespace FinalScripts
{
    [CreateAssetMenu(menuName = "Mythbreaker/Ability/Upgrade", fileName = "Ability Upgrade", order = 0)]
    public class AbilityUpgrade : ScriptableObject
    {
        public Sprite icon;
        public string displayName;
        public string description;
        public AbilityData associatedAbility;
    }
}