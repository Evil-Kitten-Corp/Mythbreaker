using System.Collections;
using TMPro;
using UnityEngine;

namespace Abilities.Types
{
    public class AttackModifierAbility: AbilityBase
    {
        public int maxAttacks = 4;
        public GameObject uiPrefab;
        
        private GameObject _ui;
        private int _currentAttacks;

        protected override void Activate()
        {
            _ui = Instantiate(uiPrefab, FindObjectOfType<Canvas>().transform);
            _ui.transform.SetAsFirstSibling();
            _currentAttacks = maxAttacks;
            
            base.Activate();
        }

        protected override IEnumerator UseThis()
        {
            while (_currentAttacks > 0)
            {
                _ui.GetComponentInChildren<TMP_Text>().text = _currentAttacks.ToString();
            }
            
            Destroy(_ui);
            return base.UseThis();
        }

        public void SendHit()
        {
            _currentAttacks--;
        }
    }
}