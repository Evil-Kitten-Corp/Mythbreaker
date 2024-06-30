using System.Collections;
using Abilities;
using FinalScripts;
using FinalScripts.Refactored;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpikeAbility : AbilityController
{
    public AudioSource source;
    public AudioClip soundOnCast;
    
    private WeaponAndAttackManager _attackControl;
    private int _abilityStacks;
    private Transform _target;

    public GameObject spikePrefab;
    public Transform firePoint;
    private ParticleSystem _effect;

    protected override void Start()
    {
        _effect = spikePrefab.GetComponent<ParticleSystem>();
        _attackControl = GetComponentInParent<WeaponAndAttackManager>();
        _attackControl.OnAttackSuccessful += AddStack;
    }

    public override int Stacks()
    {
        return _abilityStacks;
    }

    private void AddStack()
    {
        if (_abilityStacks == data.maxStacks)
        {
            return;
        }

        _abilityStacks++;
    }

    public int GetStacks() => _abilityStacks;

    protected override IEnumerator WaitForCast()
    {
        if (IsCooldown)
        {
            yield break;
        }
        
        if (TargetingSystem.Instance != null)
        {
            TargetingSystem.Instance.Activate();
        }
        
        yield return new WaitUntil(() => Input.GetKeyUp(abilityKey));

        if (TargetingSystem.Instance != null)
        {
            _target = TargetingSystem.Instance.Deactivate();
        }
        else
        {
            if (_target == null)
            {
                _target = FindObjectOfType<EnemyBT>().transform;

                if (_target == null)
                {
                    _target = FindObjectOfType<EnemyAppendage>().transform;
                }
            }
        }
        
        yield return base.WaitForCast();
    }

    protected override bool Ability()
    {
        if (_abilityStacks <= 0)
        {
            return false;
        }
        
        if (_target == null)
        {
            return false;
        }

        for (int i = 0; i < _abilityStacks; i++)
        {
            GameObject projectile = Instantiate(spikePrefab, firePoint.position, firePoint.rotation);
            
            projectile.GetComponent<TargetProjectile>().UpdateTarget(_target, Vector3.zero);

            _effect.Play();
        
            source.PlayOneShot(soundOnCast);
        }

        _abilityStacks = 0;
        
        base.Ability();
        return true;
    }
}
