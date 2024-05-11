using System;
using System.Collections.Generic;
using Minimalist.Bar.Quantity;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyBehaviour : MonoBehaviour
{
    public QuantityBhv health;
    public int attackDamage;
    public EnemyStates startingState = EnemyStates.IDLE;
    public float attackCooldown = 1.0f; 
    
    public Action<float> TakeDamage;
    
    private EnemyStates _currentState;
    private Animator _anim;
    private NavMeshAgent _agent;
    private Transform _target;
    private float _dmgToTake;
    private float _lastAttackTime; 
    
    private readonly Dictionary<EnemyStates, Action> _updateActions = new();
    private readonly Dictionary<EnemyStates, Action> _entryActions = new();
    private readonly Dictionary<EnemyStates, Action> _exitActions = new();

    private static readonly int Walking = Animator.StringToHash("Walking");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Hit = Animator.StringToHash("Hit");
    private static readonly int Die = Animator.StringToHash("Die");

    public enum EnemyStates 
    {
        IDLE, 
        SEEK, 
        ATTACK, 
        HIT, 
        DEATH
    }

    void Start()
    {
        _currentState = startingState;
        _anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        TakeDamage += OnTakeDamage;
        
        //Hard coding for now, we'll implement this better after testing
        
       //ENTRY
       
       _entryActions.Add(EnemyStates.IDLE, null);
       
       _entryActions.Add(EnemyStates.SEEK, () =>
       {
           _anim.SetBool(Walking, true);
       });
       
       _entryActions.Add(EnemyStates.ATTACK, () =>
       {
           transform.LookAt(_target);
           _anim.SetTrigger(Attack);
           DealDamage(_target);
           _lastAttackTime = Time.time;
       });
       
       _entryActions.Add(EnemyStates.HIT, () =>
       {
           health.Amount -= _dmgToTake;
           
           Vector3 randomness = new Vector3(Random.Range(0f, 0.25f), 
               Random.Range(0f, 0.25f), Random.Range(0f, 0.25f));
           
           DamagePopUpGenerator.current.CreatePopUp(transform.position + randomness, 
               _dmgToTake.ToString(), Color.cyan);
           
           _anim.SetTrigger(Hit);
       });
       
       _entryActions.Add(EnemyStates.DEATH, () =>
       {
           _anim.SetTrigger(Die);
           health.PassiveDynamics.Enabled = false;
           Destroy(gameObject, _anim.GetCurrentAnimatorClipInfo(1).Length + 5f);
       });
       
       //UPDATE
       
       _updateActions.Add(EnemyStates.IDLE, () =>
       {
           _target = GameObject.FindWithTag("Player").transform;
           transform.LookAt(_target);
       });
       
       _updateActions.Add(EnemyStates.SEEK, () =>
       {
           Seek(_target.position);
       });
       
       _updateActions.Add(EnemyStates.ATTACK, null);
       _updateActions.Add(EnemyStates.HIT, null);
       _updateActions.Add(EnemyStates.DEATH, null);
       
       //EXIT
       
       _exitActions.Add(EnemyStates.IDLE, null);
       
       _exitActions.Add(EnemyStates.SEEK, () =>
       {
           _anim.SetBool(Walking, false);
       });
       
       _exitActions.Add(EnemyStates.ATTACK, null);
       _exitActions.Add(EnemyStates.HIT, null);
       _exitActions.Add(EnemyStates.DEATH, null);
    }

    private void Seek(Vector3 location) 
    {
        _agent.SetDestination(location);
    }

    private void OnTakeDamage(float amount)
    {
        if (_currentState == EnemyStates.DEATH)
        {
            return;
        }
        
        _dmgToTake = amount;
        SwitchState(EnemyStates.HIT);
    }
    
    private void DealDamage(Transform target)
    {
        var atm = target.GetComponent<AttributesManager>();
        
        if (atm != null)
        {
            atm.TakeDamage(attackDamage);
        }
    }

    void Update()
    {
        switch (_currentState) 
        {
            case EnemyStates.IDLE:
                if (_target != null) SwitchState(EnemyStates.SEEK); 
                break;
            case EnemyStates.SEEK:
                if (_target == null) SwitchState(EnemyStates.IDLE);
                if (Vector3.Distance(transform.position, _target.position) <= 1 && CanAttack()) 
                    SwitchState(EnemyStates.ATTACK);
                break;
            case EnemyStates.ATTACK:
                SwitchState(EnemyStates.IDLE);
                break;
            case EnemyStates.HIT:
                if (health.Amount > 0) SwitchState(EnemyStates.IDLE);
                if (health.Amount <= 0) SwitchState(EnemyStates.DEATH);
                break;
            case EnemyStates.DEATH:
                break;
        }
        
        _updateActions[_currentState]?.Invoke();
    }
    
    void SwitchState(EnemyStates state) 
    {
        _exitActions[_currentState]?.Invoke();
        _currentState = state;
        _entryActions[_currentState]?.Invoke();
    }
    
    private bool CanAttack()
    {
        return Time.time - _lastAttackTime >= attackCooldown;
    }
}
