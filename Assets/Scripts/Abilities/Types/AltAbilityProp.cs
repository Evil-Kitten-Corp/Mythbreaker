using System.Collections;
using Abilities;
using UnityEngine;
using TriInspector;

namespace Base
{
    public class AltAbilityProp : AltAbilityBase
    {
        private AbilityPreviewer _abilityPreviewer;
        private Transform _instLocation;
        
        [ValidateInput(nameof(ValidateProp))]
        public GameObject propPrefab;
        
        public override void Start()
        {
            _abilityPreviewer = player.GetComponent<AbilityPreviewer>();
            _abilityPreviewer.SubscribeToAbilityCast(AbilityCastComplete, this);
            base.Start();
        }

        private TriValidationResult ValidateProp()
        {
            if (propPrefab == null) return TriValidationResult.Error("Need to specify prefab.");
            if (propPrefab.GetComponent<IAbilityProp>() == null) return TriValidationResult.Warning("Prefab must " +
                "contain an ability prop behaviour.");
            return TriValidationResult.Valid;
        }

        protected override IEnumerator Activate()
        {
            while (_instLocation == null)
            {
                yield return new WaitForEndOfFrame();
            }

            Instantiate(propPrefab, _instLocation);
            yield return base.Activate();
        }
        
        private void AbilityCastComplete(Transform t)
        {
            _instLocation = t;
            Use();
        }
    }
}