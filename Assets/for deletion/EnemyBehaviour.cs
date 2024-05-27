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

public class EnemyBehaviour : MonoBehaviour
{
    public QuantityBhv health;
    public EnemyStates startingState = EnemyStates.IDLE;
    public float attackCooldown = 1.0f;

    [Title("Death Effect")] 
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;

    private Material[] _skinnedMats;
    
    public Action<float> TakeDamage;
    public Action<GameObject> OnDeath;
    public Action KnockUp;
    
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
    
    public EnemyStats stats;
    private static readonly int DissolveAmount = Shader.PropertyToID("_DissolveAmount");

    public enum EnemyStates 
    {
        IDLE, 
        SEEK, 
        ATTACK, 
        HIT, 
        DEATH,
        KNOCKDOWN
    }

    void Start()
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
        
        //Hard coding for now, we'll implement this better after testing
        
       //ENTRY
       
       _entryActions.Add(EnemyStates.KNOCKDOWN, () =>
       {
           _anim.SetTrigger("KnockUp");  
       });
       
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
           TargetingSystem.Instance.screenTargets.Remove(gameObject);
           OnDeath?.Invoke(gameObject);
           Destroy(gameObject, _anim.GetCurrentAnimatorClipInfo(0).Length + 5f);
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
       
       _updateActions.Add(EnemyStates.KNOCKDOWN, () =>
       {
           if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Knockdown") | 
               _anim.GetCurrentAnimatorStateInfo(0).IsName("GetUp"))
           {
               return;
           }
           
           SwitchState(EnemyStates.IDLE);
       });
       
       //EXIT
       
       _exitActions.Add(EnemyStates.IDLE, null);
       
       _exitActions.Add(EnemyStates.SEEK, () =>
       {
           _anim.SetBool(Walking, false);
       });
       
       _exitActions.Add(EnemyStates.ATTACK, null);
       _exitActions.Add(EnemyStates.HIT, null);
       _exitActions.Add(EnemyStates.DEATH, null);
       _exitActions.Add(EnemyStates.KNOCKDOWN, null);
    }

    private void OnKnockup()
    {
        if (_currentState == EnemyStates.DEATH)
        {
            return;
        }
        
        SwitchState(EnemyStates.KNOCKDOWN);
    }

    private void OnDie(GameObject obj)
    {
        StartCoroutine(Dissolve());
    }

    private IEnumerator Dissolve()
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
            atm.TakeDamage((int)stats.attackDamage);
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
            case EnemyStates.KNOCKDOWN:
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
