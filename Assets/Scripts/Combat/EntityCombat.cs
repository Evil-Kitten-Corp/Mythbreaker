using System;
using System.Collections;
using System.Collections.Generic;
using Ability_Behaviours.UI;
using Base;
using BrunoMikoski.ServicesLocation;
using Combat.Statuses;
using TriInspector;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

namespace Combat
{
    [DeclareFoldoutGroup("Foldout", Title = "Death VFX")]
    [DeclareTabGroup("Settings")]
    public class EntityCombat : MonoBehaviour, IDamageable
    {
        [GroupNext("Settings"), Tab("General")] 
        public Animator animator;
        public bool debug;
        public EntityData data;
        public bool hasWeapon;
        [ShowIf(nameof(hasWeapon))] public WeaponInstance weapon;

        [GroupNext("Settings"), Tab("Player")] 
        public bool isPlayer;
        [ShowIf(nameof(isPlayer))] [SerializeReference] public List<IHitModifier> attackModifiers;

        [GroupNext("Settings"), Tab("Death")] 
        public float expGained;
        private bool _alive = true;

        [GroupNext("Foldout")] 
        public SkinnedMeshRenderer skinnedMeshRenderer;
        public VisualEffect vfxGraph;
        public float dissolveRate = 0.05f;
        public float refreshRate = 0.1f;
        public float dieDelay = 0.25f;

        #region Private

        private Material[] _dissolveMaterials;
        private float _currentHealth;
        private static readonly int DissolveAmount = Shader.PropertyToID("DissolveAmount_");
        private static readonly int Die1 = Animator.StringToHash("Die");
        private static readonly int Revive1 = Animator.StringToHash("Revive");
        private static readonly int Hit = Animator.StringToHash("Hit");
        private delegate void AttackDelegate();
        private AttackDelegate attack;

        private event Action OnWeaponEquip;
        private event Action OnWeaponUnequip;

        private IgnitedStatus _status = null;
        private StunnedStatus _stunned = null;

        #endregion

        void EquipWeapon(WeaponInstance wpn)
        {
            hasWeapon = true;
            weapon = wpn;
            attack = weapon.Attack;
            OnWeaponEquip?.Invoke();
        }   
        
        void UnequipWeapon()
        {
            hasWeapon = false;
            weapon = null;
            attack = Attack;
            OnWeaponUnequip?.Invoke();
        }   
        
        void Start()
        {
            if (hasWeapon)
            {
                EquipWeapon(weapon);
            }
            else
            {
                UnequipWeapon();
            }
            
            _currentHealth = data.maxHealth;
            
            if (vfxGraph != null)
            {
                vfxGraph.Stop();
                vfxGraph.gameObject.SetActive(false);
            }

            if (skinnedMeshRenderer != null)
                _dissolveMaterials = skinnedMeshRenderer.materials;
        }
    
        private void Update()
        {
            _status?.Update();

            if (_alive && debug)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    StartCoroutine(DissolveCo());
                }
            }

            if (!_alive && debug)
            {
                if (Input.GetKeyDown(KeyCode.R))
                    Revive();
            }
        }
    
        IEnumerator DissolveCo()
        {
            _alive = false;

            if (animator != null)
                animator.SetTrigger(Die1);
            else
            {
                _alive = true;
                yield break;
            }

            yield return new WaitForSeconds(dieDelay);
        
            if (vfxGraph != null)
            {
                vfxGraph.gameObject.SetActive(true);
                vfxGraph.Play();
            }

            float counter = 0; 

            if (_dissolveMaterials.Length > 0)
            {   
                while (_dissolveMaterials[0].GetFloat(DissolveAmount) < 1)
                {
                    counter += dissolveRate;
                
                    foreach (var t in _dissolveMaterials)
                        t.SetFloat(DissolveAmount, counter);

                    yield return new WaitForSeconds (refreshRate);
                }
            }
        }

        private void Revive()
        {
            if (animator != null)
            {
                animator.SetTrigger(Revive1);
                _alive = true;
            }

            if (_dissolveMaterials.Length > 0)
            {
                foreach (var t in _dissolveMaterials)
                    t.SetFloat(DissolveAmount, 0);
            }
        }

        public void Attack()
        {
            
        }

        public void OnHit(float dmg)
        {
            TakeDamage(dmg);
        }

        public void OnHit(List<IHitModifier> modifiers)
        {
            foreach (var hm in modifiers)
            {
                StartCoroutine(hm.Tick(this));
            }
        }

        public void TakeDamage(float dmg)
        {
            _currentHealth -= dmg;
            Vector3 randomness = new Vector3(Random.Range(0f, 0.25f), Random.Range(0f, 0.25f), Random.Range(0f, 0.25f));
            ServiceLocator.Instance.GetInstance<DamagePopUpGenerator>().CreatePopUp(transform.position + 
                randomness, dmg.ToString(), Color.yellow);

            if (animator != null)
            {
                animator.SetTrigger(Hit);
            }
        }

        public void Die()
        {
            if (_status != null)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, 10);

                foreach (Collider col in colliders)
                {
                    if (col.CompareTag("Player"))
                    {
                        return;
                    }
                    
                    EntityCombat enemyHealth = col.GetComponent<EntityCombat>();
                    
                    if (enemyHealth != null && !enemyHealth.IsDead())
                    {
                        enemyHealth.AddStatus(new IgnitedStatus(_status.tickDamage, _status.tickInterval, _status
                        .duration, enemyHealth));
                    }
                }
            }
        }

        private bool IsDead()
        {
            return _currentHealth > 0;
        }

        public void AddStatus(IStatusEffect status)
        {
            if (status.status == Status.Ignite)
            {
                _status = (IgnitedStatus)status;
            }
            else
            {
                _stunned = (StunnedStatus)status;
            }
        }

        public void RemoveStatus(Status status)
        {
            if (status == Status.Ignite)
            {
                _status = null;
            }
            else
            {
                _stunned = null;
            }
        }

        public void ApplySlow(float slowAmount)
        {
            throw new NotImplementedException();
        }

        public void RestoreSpeed()
        {
            throw new NotImplementedException();
        }
    }
}