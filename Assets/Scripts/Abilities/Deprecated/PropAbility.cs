using System;
using System.Collections;
using UnityEngine;

namespace Abilities
{
    public class PropAbility : AbilityBase
    {
        public GameObject abilityPrefab;

        public void Init(GameObject arg)
        {
            abilityPrefab = arg;
        }
        
        protected override IEnumerator UseThis()
        {
            Instantiate(abilityPrefab, transform);
            
            base.UseThis();
            yield break;
        }
    }

    public interface IAbilityWrapper
    {
        public IAbilityBehaviour ToAbilityBehaviour(GameObject parent);
    }

    [Serializable]
    public class PropAbilityWrapper : IAbilityWrapper
    {
        public GameObject abilityPrefab;

        public PropAbility ToPropAbility(GameObject parent)
        {
            var pA = parent.AddComponent<PropAbility>();
            pA.Init(abilityPrefab);
            return pA;
        }

        public IAbilityBehaviour ToAbilityBehaviour(GameObject parent)
        {
            return ToPropAbility(parent);
        }
    }

    [Serializable]
    public class DebugAbilityWrapper : IAbilityWrapper
    {
        public DebugAbility ToDebugAbility(GameObject parent)
        {
            var dA = parent.AddComponent<DebugAbility>();
            return dA;
        }

        public IAbilityBehaviour ToAbilityBehaviour(GameObject parent)
        {
            return ToDebugAbility(parent);
        }
    }
}