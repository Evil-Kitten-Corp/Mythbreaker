using System.Globalization;
using Base;
using BrunoMikoski.ServicesLocation;
using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Abilities
{
    public class Slot : MonoBehaviour, IDropHandler
    {
        [Title("References")]
        public TMP_Text cooldownText;
        public Image cooldownMask; 
        
        [Title("Individual Type")]
        public AbilitySlot slotType;
        
        private bool _cooldown;
        private AbilityInfo _ability;
        private PlayerCharacter _player;
        
        private void Start()
        {
            _player = ServiceLocator.Instance.GetInstance<PlayerCharacter>();
        }

        private void Update()
        {
            if (_cooldown)
            {
                var ab = _player.GetAbilityInstance(_ability);
                cooldownText.text = ab != null ? ab.GetCurrentCooldownLeft().ToString(CultureInfo.CurrentCulture) : null; 
                if (ab != null) cooldownMask.fillAmount = ab.GetCurrentCooldownLeft() / _ability.cooldown;
            }
        }

        private void EnterCooldown()
        {
            _cooldown = true;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (transform.childCount == 0)
            {
                AbilityUI droppedAbilityUI = eventData.pointerDrag.GetComponent<AbilityUI>();

                if (droppedAbilityUI != null)
                {
                    droppedAbilityUI.parentAfterDrag = transform;
                    _ability = droppedAbilityUI.ability;
                    
                    if (_ability != null)
                    {
                        _ability.Equip();
                    }
                }

                var ab = _player.GetAbilityInstance(_ability);
                if (ab != null) ab.OnHandleCooldown += EnterCooldown;
            }
        }
    }
}