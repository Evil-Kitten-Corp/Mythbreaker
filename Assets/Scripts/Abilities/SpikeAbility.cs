using System.Collections;
using Abilities;
using UnityEngine;

public class SpikeAbility : AbilityController
{
    public AudioSource source;
    public AudioClip soundOnCast;
    
    private WeaponAndAttackManager _attackControl;
    private int _abilityStacks;
    private Transform _target;

    public GameObject spikePrefab;
    public Transform firePoint;

    protected override void Start()
    {
        _attackControl = GetComponentInParent<WeaponAndAttackManager>();
        _attackControl.OnAttackSuccessful += AddStack;
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
        TargetingSystem.Instance.Activate();
        yield return new WaitUntil(() => Input.GetKeyUp(abilityKey));
        _target = TargetingSystem.Instance.Deactivate();
        yield return base.WaitForCast();
    }

    protected override void Ability()
    {
        if (_abilityStacks <= 0)
        {
            return;
        }
        
        if (_target == null)
        {
            return;
        }

        for (int i = 0; i < _abilityStacks; i++)
        {
            GameObject projectile = Instantiate(spikePrefab, firePoint.position, firePoint.rotation);
            
            projectile.GetComponent<TargetProjectile>().UpdateTarget(_target, Vector3.zero);
            
            var effect = spikePrefab.GetComponent<ParticleSystem>();
            effect.Play();
        
            source.PlayOneShot(soundOnCast);
        }

        _abilityStacks = 0;
        
        base.Ability();
    }
}
