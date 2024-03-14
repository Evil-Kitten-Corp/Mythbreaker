using System;
using System.Collections;
using Base;
using UnityEngine;
using BrunoMikoski.ServicesLocation;

namespace Abilities
{
    public abstract class AltAbilityBase : MonoBehaviour
    {
        public AbilitySlot abilitySlot;
        public AbilityInfo data;
        public event Action OnHandleCooldown;
        public event Action OnAbilityStart;
        public event Action OnAbilityEnd;
        
        protected event Action OnListeningToAbility;
        
        protected AltPlayer player;
        private float _tempCooldown = -1;
        public bool isCasting = false;

        public virtual void Start()
        {
            OnHandleCooldown += HandleCooldown;
            OnAbilityEnd += Deactivate;
            OnListeningToAbility = player.mapper[abilitySlot];
            player.mapper[abilitySlot] += OnListeningToAbility;
            OnListeningToAbility += Use;
        }
        
        private void HandleCooldown()
        {
            _tempCooldown = data.cooldown;
        }

        public virtual void Update()
        {
            if (_tempCooldown >= 0)
            {
                _tempCooldown += Time.deltaTime;
            }

            if (CanUse && !isCasting)
            {
                if (Input.GetKeyDown(this.GetKeyCode(ServiceLocator.Instance.GetInstance<InputManager>())))
                {
                    Use();
                }
            }
        }
        
        protected virtual IEnumerator Activate()
        {
            OnHandleCooldown?.Invoke();
            Debug.Log($"Used ability {data.abilityName}.");
            OnAbilityEnd?.Invoke();
            yield break;
        }
        
        protected virtual void Deactivate() {}

        private bool CanUse => !(_tempCooldown >= 0);

        public float GetCurrentCooldownLeft() => _tempCooldown;

        public virtual void ChangeAbilitySlot(AbilitySlot abslot)
        {
            player.mapper[abilitySlot] -= OnListeningToAbility;
            OnListeningToAbility -= Use;
            abilitySlot = abslot;
            OnListeningToAbility = player.mapper[abslot];
            player.mapper[abilitySlot] += OnListeningToAbility;
            OnListeningToAbility += Use;
        }

        public virtual void Use()
        {
            OnAbilityStart?.Invoke();
            StartCoroutine(Activate());
        }
    }
}