using Abilities;
using UnityEngine;

namespace Base
{
    public class AltAbilityStacking : AltAbilityBase
    {
        public int maxStacks;
        public GameObject abilityPrefab;
        private int _currentStacks;

        public override void Start() 
        {
            base.Start();
            player.OnPerformAttack += AddStack;
        }

        public override void Use()
        {
            for (int i = 0; i < _currentStacks; i++)
            {
                Instantiate(abilityPrefab, player.abilityHolder.transform);
            }
            
            base.Use();
        }

        private void AddStack()
        {
            if (_currentStacks < maxStacks)
            {
                _currentStacks++;
            }
        }
    }
}