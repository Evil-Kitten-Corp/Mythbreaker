using System;
using System.Collections;
using UnityEngine;

namespace Abilities
{
    public class AbilityController : MonoBehaviour
    {
        public AbilityData data;
        public KeyCode abilityKey;
        
        public bool IsCooldown { get; private set; } 
        public bool CanRecast { get; private set; }  // New
        public float CdTimer { get; private set; }
        public float RecastTimer { get; private set; }  // New

        public float Cooldown => data.cooldown;
        public float MaxRecastTime => data.maxRecastTime; // New
        public bool IsRecastable => data.canRecast;

        public event Action OnCooldownStart;
        public event Action OnCooldownEnd;
        public event Action OnRecast; // New

        private float _timeSinceRecast;

        [Header("Debug")] public float recastTimeThreshold = 0.2f;

        protected virtual void Start()
        {
            OnCooldownStart += () => IsCooldown = true;
            OnCooldownEnd += () => IsCooldown = false;
            OnRecast += () => CanRecast = false;
            RecastTimer = MaxRecastTime;
        }

        private void Tick()
        {
            if (IsCooldown)
            {
                CdTimer -= Time.deltaTime;

                if (CdTimer <= 0)
                {
                    OnCooldownEnd?.Invoke();
                    //CanRecast = true;
                }
            }
            else if (IsRecastable && CanRecast)
            {
                RecastTimer -= Time.deltaTime;

                if (RecastTimer <= 0)
                {
                    Recast();
                }
            }
        }

        protected void Update()
        {
            if (IsCooldown || IsRecastable) // Updated
            {
                Tick();
            }

            if (Input.GetKeyDown(abilityKey) && !data.hasCastDelay)
            {
                Use();
            }

            if (Input.GetKeyDown(abilityKey) && data.hasCastDelay && !IsCooldown)
            {
                StartCoroutine(WaitForCast());
            }
        }

        public void Use()
        {
            if (IsCooldown)
            {
                return;
            }

            if (CanRecast)
            {
                float currentTime = Time.time;
                
                if (currentTime - _timeSinceRecast >= recastTimeThreshold)
                {
                    if (Input.GetKeyDown(abilityKey))
                    {
                        Recast();
                        OnCooldownStart?.Invoke();
                        CdTimer = Cooldown;
                        _timeSinceRecast = currentTime;
                        return;
                    }

                    return;
                }
                
                return;
            }

            BeforeAbility();
            Ability();    
            AfterAbility();

            if (IsRecastable)
            {
                CanRecast = true;
                RecastTimer = MaxRecastTime;
            }
            else
            {
                OnCooldownStart?.Invoke(); 
                CdTimer = Cooldown;
            }
        }

        protected virtual IEnumerator WaitForCast()
        {
            Use();
            yield break;
        }

        protected virtual void BeforeAbility() { } // Can be overridden in derived classes

        protected virtual void Ability()
        {
            if (IsRecastable)
            {
                _timeSinceRecast = Time.time;
            }
        } // Can be overridden in derived classes
        
        protected virtual void AfterAbility() { }  // Can be overridden in derived classes

        protected virtual void Recast()
        {
            Debug.Log("Called recast.");
            OnRecast?.Invoke();
        }  // Can be overridden in derived classes
    }
}