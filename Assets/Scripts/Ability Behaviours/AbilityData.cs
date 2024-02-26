using ExtEvents;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ability_Behaviours
{
    [CreateAssetMenu(fileName = "AbilityData", menuName = "Tools/Abilities/New", order = 0)]
    public class AbilityData : ScriptableObject
    {
        public string id;
        public Sprite image;
        public InputAction input;
        public float cooldown;
        public bool obtainable; 
        public bool cancellable;
        
        [HideInInspector] public bool canUse;
        
        public ExtEvent OnCooldown;
        public ExtEvent OnCooldownEnd;
        
    }
}