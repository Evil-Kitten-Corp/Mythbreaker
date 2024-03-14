using System.Linq;
using Abilities;
using UnityEngine;

namespace Base
{
    public static class Extensions
    {
        public static KeyCode GetKeyCode(this AbilitySlot ab, InputManager im)
        {
            return im.Map.First(x => x.Value == ab).Key;
        }
        
        public static KeyCode GetKeyCode(this AbilityBase ab, InputManager im)
        {
            return im.Map.First(x => x.Value == ab.abilitySlot).Key;
        }
        
        public static KeyCode GetKeyCode(this AltAbilityBase ab, InputManager im)
        {
            return im.Map.First(x => x.Value == ab.abilitySlot).Key;
        }
    }
}