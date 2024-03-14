using System;
using System.Collections;
using Base;
using BrunoMikoski.ServicesLocation;
using UnityEngine;

namespace Abilities
{
    public abstract class AbilityBase: MonoBehaviour, IAbilityBehaviour
    {
        public AbilitySlot abilitySlot;
        public AbilityInfo data;

        private float _tempCooldown = -1;
        private AbilityInfo _abilityInfo;
        
        public event Action OnHandleCooldown;
        public event Action OnAbilityStart;
        public event Action OnAbilityEnd;

        private void Start()
        {
            OnHandleCooldown += HandleCooldown;
            OnAbilityStart += Activate;
            OnAbilityEnd += Deactivate;
        }

        private void HandleCooldown()
        {
            _tempCooldown = data.cooldown;
        }

        private void Update()
        {
            if (_tempCooldown >= 0)
            {
                _tempCooldown += Time.deltaTime;
            }

            if (CanUse)
            {
                if (Input.GetKeyDown(this.GetKeyCode(ServiceLocator.Instance.GetInstance<InputManager>())))
                {
                    Use();
                }
            }
        }

        public void Use()
        {
            OnAbilityStart?.Invoke();
        }

        protected virtual void Activate()
        {
            StartCoroutine(UseThis());
        }

        protected virtual IEnumerator UseThis()
        {
            OnHandleCooldown?.Invoke();
            Debug.Log($"Used ability {data.abilityName}.");
            OnAbilityEnd?.Invoke();
            yield break;
        }

        protected virtual void Deactivate() {}

        private bool CanUse => !(_tempCooldown >= 0);

        public float GetCurrentCooldownLeft() => _tempCooldown;

        public AbilityInfo AbilityInfo
        {
            get => data;
            set => data = value;
        }
    }
}