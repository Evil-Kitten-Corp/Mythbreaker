using System.Collections;
using Abilities;
using UnityEngine;

namespace Base
{
    public class AltAbilityIgnite: AltAbilityBase
    {
        public int maxAttacks = 5; 
        public float burnDuration = 5f; 
        public float tickInterval = 1f;
        public float explosionRadius = 3f; 
        public float burnDamagePerTick = 10f;

        private int _attacksMade = 0;
        private bool _isOn = false;
        
        public override void Start() 
        {
            base.Start();
            player.OnPerformAttack += OnAttack;
        }
        
        public override void ChangeAbilitySlot(AbilitySlot abslot)
        {
            
        }

        public override void Use()
        {
            _attacksMade = 0;
            _isOn = true;
            base.Use();
        }

        protected override IEnumerator Activate()
        {
            while (_attacksMade < maxAttacks)
            {
                yield return new WaitForSeconds(tickInterval);
                //ApplyBurnDamage();
            }
            
            _isOn = false;

            yield return base.Activate();
        }
        
        public void OnAttack()
        {
            if (_isOn)
            {
                _attacksMade++;
            }
        }
    }
}