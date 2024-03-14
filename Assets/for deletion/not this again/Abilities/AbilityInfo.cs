using UnityEngine;

namespace not_this_again.Abilities
{
    [CreateAssetMenu(fileName = "Ability", menuName = "Tools/Rewritten/Ability", order = 0)]
    public class AbilityInfo : ScriptableObject
    {
        public string title;
        public Sprite icon;
        public float cooldownTime = 1;
        public bool canUse = true;
    }
}