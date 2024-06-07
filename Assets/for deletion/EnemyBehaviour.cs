using System;
using System.Collections;
using System.Collections.Generic;
using Minimalist.Bar.Quantity;
using Abilities;
using FinalScripts;
using TriInspector;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public abstract class EnemyBehaviour : MonoBehaviour
{
    public QuantityBhv health;
    public EnemyStates startingState = EnemyStates.IDLE;
    public float attackCooldown = 1.0f;

    [Title("Death Effect")] 
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;

    internal Material[] _skinnedMats;
    
    public Action<float> TakeDamage;
    public Action<GameObject> OnDeath;
    public Action KnockUp;
    
    internal EnemyStates _currentState;
    internal Animator _anim;
    internal NavMeshAgent _agent;
    internal Transform _target;
    internal float _dmgToTake;
    internal float _lastAttackTime; 
    
    internal readonly Dictionary<EnemyStates, Action> _updateActions = new();
    internal readonly Dictionary<EnemyStates, Action> _entryActions = new();
    internal readonly Dictionary<EnemyStates, Action> _exitActions = new();

    internal static readonly int Walking = Animator.StringToHash("Walking");
    internal static readonly int Attack = Animator.StringToHash("Attack");
    internal static readonly int Hit = Animator.StringToHash("Hit");
    internal static readonly int Die = Animator.StringToHash("Die");
    
    public EnemyStats stats;
    internal static readonly int DissolveAmount = Shader.PropertyToID("_DissolveAmount");

    public enum EnemyStates 
    {
        IDLE, 
        SEEK,
        PREPARE,
        ATTACK, 
        HIT, 
        DEATH,
        KNOCKDOWN
    }

    public virtual void Start()
    {
        _currentState = startingState;
        _anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        OnDeath += OnDie;
        TakeDamage += OnTakeDamage;
        KnockUp += OnKnockup;

        if (skinnedMeshRenderer != null)
        {
            _skinnedMats = skinnedMeshRenderer.materials;
        }
    }

    internal void OnKnockup()
    {
        if (_currentState == EnemyStates.DEATH)
        {
            return;
        }
        
        SwitchState(EnemyStates.KNOCKDOWN);
    }

    internal void OnDie(GameObject obj)
    {
        StartCoroutine(Dissolve());
    }

    internal IEnumerator Dissolve()
    {
        if (_skinnedMats.Length > 0)
        {
            float i = 0;

            while (_skinnedMats[0].GetFloat(DissolveAmount) < 1)
            {
                i += dissolveRate;

                foreach (var t in _skinnedMats)
                {
                    t.SetFloat(DissolveAmount, i);
                }

                yield return new WaitForSeconds(refreshRate);
            }
        }
    }

    internal void Seek(Vector3 location) 
    {
        _agent.SetDestination(location);
    }

    internal void OnTakeDamage(float amount)
    {
        if (_currentState == EnemyStates.DEATH)
        {
            return;
        }
        
        _dmgToTake = amount;
        SwitchState(EnemyStates.HIT);
    }
    
    internal virtual void DealDamage(Transform target)
    {
        var atm = target.GetComponent<AttributesManager>();
        
        if (atm != null)
        {
            atm.TakeDamage((int)stats.attackDamage);
        }
    }

    public virtual void Update()
    {
        _updateActions[_currentState]?.Invoke();
    }
    
    internal void SwitchState(EnemyStates state) 
    {
        _exitActions[_currentState]?.Invoke();
        _currentState = state;
        _entryActions[_currentState]?.Invoke();
    }
    
    internal bool CanAttack()
    {
        return Time.time - _lastAttackTime >= attackCooldown;
    }
}
