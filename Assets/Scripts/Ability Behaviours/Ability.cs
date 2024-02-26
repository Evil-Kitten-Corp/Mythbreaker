using System.Collections;
using System.Collections.Generic;
using TNRD;
using UnityEngine;

namespace Ability_Behaviours
{
    public abstract class Ability : MonoBehaviour
    {
        public AbilityData ability;
        
        [Header("[ Extras ]")] [Tooltip("In order of execution")]
        [SerializeField] public List<SerializableInterface<IAbilityBehaviour>> abilityExtraProperties;

        public void Update()
        {
            if (ability.cancellable && Input.GetKeyDown(KeyCode.Escape))
            {
                foreach (var property in abilityExtraProperties)
                {
                    property.Value.Unapply();
                }
                
                StopAllCoroutines();
            }
        }

        public void Use()
        {
            if (ability.canUse)
            {
                StartCoroutine(Apply());
            }
        }

        protected virtual IEnumerator Apply()
        {
            foreach (var property in abilityExtraProperties)
            {
                yield return StartCoroutine(property.Value.Apply());
            }
            
            EnterCooldown();
        }

        private void EnterCooldown()
        {
            ability.OnCooldown.Invoke();
            ability.canUse = false;
        }
    }
}