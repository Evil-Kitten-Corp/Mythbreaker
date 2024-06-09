using System.Collections;
using Abilities;
using UnityEngine;

namespace FinalScripts
{
    public class FelineEnemyBehaviour : EnemyBehaviour
    {
        public float pounceForce = 10f;
        private float _pounceTimer;
        
        public override void Start()
        {
            base.Start();
            
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
               _lastAttackTime = Time.time;
               _agent.isStopped = true;
               Vector3 direction = (_target.position - transform.position).normalized;
               _anim.SetTrigger(Attack);
               GetComponent<Rigidbody>().AddForce(direction * pounceForce, ForceMode.VelocityChange);
               StartCoroutine(EndLunge());
           });
           
           _entryActions.Add(EnemyStates.HIT, () =>
           {
               health.Amount -= _dmgToTake;
               
               Vector3 randomness = new Vector3(Random.Range(0f, 0.25f), 
                   Random.Range(0f, 0.25f), Random.Range(0f, 0.25f));
               
               DamagePopUpGenerator.Current.CreatePopUp(transform.position + randomness, 
                   _dmgToTake.ToString(), Color.cyan);
               
               _anim.SetTrigger(Hit);
           });
           
           _entryActions.Add(EnemyStates.DEATH, () =>
           {
               _anim.SetTrigger(Die);
               healthBar.SetActive(false);
               health.enabled = false;
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
           
           //
           _entryActions.Add(EnemyStates.PREPARE, () =>
           {
               _pounceTimer = 0;
               transform.LookAt(_target);
               _anim.SetTrigger("Prepare");
           });
           
           _updateActions.Add(EnemyStates.PREPARE, () =>
           {
               _pounceTimer += Time.deltaTime;

               if (_pounceTimer >= 2)
               {
                   SwitchState(EnemyStates.ATTACK);
               }
           });
           
           _exitActions.Add(EnemyStates.PREPARE, null);
        }

        public override void Update()
        {
            switch (_currentState) 
            {
                case EnemyStates.IDLE:
                    if (_target != null) SwitchState(EnemyStates.SEEK); 
                    break;
                case EnemyStates.SEEK:
                    if (_target == null) SwitchState(EnemyStates.IDLE);
                    if (Vector3.Distance(transform.position, _target.position) <= stats.range && CanAttack()) 
                        SwitchState(EnemyStates.PREPARE);
                    break;
                case EnemyStates.ATTACK:
                    SwitchState(EnemyStates.IDLE);
                    break;
                case EnemyStates.HIT:
                    if (health.Amount > 0) SwitchState(EnemyStates.IDLE);
                    if (health.Amount <= 0) SwitchState(EnemyStates.DEATH);
                    break;
                case EnemyStates.PREPARE:
                    break;
                case EnemyStates.KNOCKDOWN:
                    break;
                case EnemyStates.DEATH:
                    break;
            }
            
            base.Update();
        }
        
        IEnumerator EndLunge()
        {
            yield return new WaitForSeconds(0.5f);
            DealDamage(_target);
            _agent.isStopped = false;
        }
    }
}