using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = "Ability", menuName = "Abilities/Data", order = 0)]
    public class AbilityData : ScriptableObject
    {
        public string god;
        public string abName;
        [TextArea] public string abDescription;
        
        public float cooldown;
        public bool canRecast;
        public float maxRecastTime;
        public Sprite icon;
        public Sprite altIcon;

        public bool hasCastDelay;
        public bool hasStacks;
        public int maxStacks;
    }
}