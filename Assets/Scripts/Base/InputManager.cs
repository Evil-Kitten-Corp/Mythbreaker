using System.Collections.Generic;
using BrunoMikoski.ServicesLocation;
using TriInspector;
using UnityEngine;

namespace Base
{
    public class InputManager: MonoBehaviour
    {
        [Title("Keys")] 
        public KeyCode Ability1 = KeyCode.Alpha1;
        public KeyCode Ability2 = KeyCode.Alpha2;
        public KeyCode Ability3 = KeyCode.Alpha3;
        public KeyCode Ability4 = KeyCode.Alpha4;
        public KeyCode LightAttack = KeyCode.Mouse0;
        public KeyCode HeavyAttack = KeyCode.Mouse1;
        public KeyCode Grapple = KeyCode.Q;
        public KeyCode Dash = KeyCode.LeftControl;

        public InputManager()
        {
            Map = new Dictionary<KeyCode, AbilitySlot>
            {
                { Ability1, AbilitySlot.A1 },
                { Ability2, AbilitySlot.A2 },
                { Ability3, AbilitySlot.A3 },
                { Ability4, AbilitySlot.A4 }
            };
        }

        public Dictionary<KeyCode, AbilitySlot> Map { get; }

        private void Start()
        {
            ServiceLocator.Instance.RegisterInstance(this);
        }
    }
}