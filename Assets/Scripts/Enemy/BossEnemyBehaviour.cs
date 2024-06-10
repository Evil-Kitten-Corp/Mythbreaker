using System.Collections;
using System.Linq;
using Abilities;
using UnityEngine;

namespace FinalScripts
{
    public class BossEnemyBehaviour : EnemyBehaviour
    {
        public EnemyAppendage[] legs;
        public Transform eye;
        public Laser laser;
        public float laserCooldown;
        private float _chargeTimer;
        private float _laserTimer;

        public float laserActivateTimeDelay;
        public float lerpSpeed;
        public float lerpThicknessSpeed;
        public ParticleSystem scorch;
        
        public float stompRadius = 5.0F;
        public float stompPower = 10.0F;
        public ParticleSystem shockwave;
        
        private float _lastUsedLaser;

        private bool _canLaser = false;
        private bool _canStomp = true;

        public override void Start()
        {
            _canBeKnockedup = false;
            _lastUsedLaser = Time.time;
            StartCoroutine(LaserCooldown());
            base.Start();
            
            //ENTRY
            _entryActions.Add(EnemyStates.KNOCKDOWN, null);
            _entryActions.Add(EnemyStates.IDLE, null);
            
            _entryActions.Add(EnemyStates.SEEK, () => 
            { 
                Debug.Log("Boss is running away.");
                //we will actually use this to instead flee
                _anim.SetBool(Walking, true); 
            });
           
           _entryActions.Add(EnemyStates.ATTACK, () =>
           {
               Debug.Log("Boss is attacking.");

               if (_canLaser)
               {
                   _laserTimer = 0;
                   _lastUsedLaser = Time.time;
                   ShootLaser();
               }
               else
               {
                   shockwave.Play();
                   Rigidbody rb = _target.GetComponent<Rigidbody>();

                   if (rb != null)
                       rb.AddExplosionForce(stompPower, transform.position, stompRadius, 3.0F);

                   if (rb.TryGetComponent<AttributesManager>(out var att))
                   {
                       att.Knockup();
                   }

                   _canStomp = false;
                   StartCoroutine(StompCooldown());
               }
               
           });
           
           _entryActions.Add(EnemyStates.HIT, null);
           
           _entryActions.Add(EnemyStates.DEATH, () =>
           {
               Debug.Log("Boss is dead.");
               _anim.SetTrigger(Die);
               TargetingSystem.Instance.screenTargets.Remove(gameObject);
               OnDeath?.Invoke(gameObject);
               Destroy(gameObject, _anim.GetCurrentAnimatorClipInfo(0).Length + 5f);
               AttributesManager.OnDefeatBoss?.Invoke();
           });
           
           //UPDATE
           
           _updateActions.Add(EnemyStates.IDLE, () =>
           {
               _target = GameObject.FindWithTag("Player").transform;
               transform.LookAt(_target);
           });
           
           _updateActions.Add(EnemyStates.SEEK, Flee);
           
           _updateActions.Add(EnemyStates.ATTACK, () =>
           {
               if (_canLaser)
               {
                   _laserTimer += Time.deltaTime;

                   if (_laserTimer >= 3)
                   {
                       laser.gameObject.SetActive(false);
                       _canLaser = false;
                       StartCoroutine(LaserCooldown());
                       SwitchState(EnemyStates.IDLE);
                       _lastAttackTime = Time.time;
                   }
               }
           });
           
           _updateActions.Add(EnemyStates.HIT, null);
           _updateActions.Add(EnemyStates.DEATH, null);
           _updateActions.Add(EnemyStates.KNOCKDOWN, null);
           
           //EXIT
           
           _exitActions.Add(EnemyStates.IDLE, null);
           
           _exitActions.Add(EnemyStates.SEEK, () =>
           {
               _anim.SetBool(Walking, false);
           });
           
           _exitActions.Add(EnemyStates.ATTACK, () =>
           {
               Debug.Log("Boss stopped attacking.");
               StopAllCoroutines();
               laser.Deactivate();
               laser.HitDeactivate();
               StartCoroutine(LaserDeactivateCoroutine());

               IEnumerator LaserDeactivateCoroutine()
               {
                   float startThickness = laser.thickness;
                   float lerp = 0;
                   while (lerp < 1)
                   {
                       laser.thickness = Mathf.Lerp(startThickness, 0, lerp);
                       lerp += Time.deltaTime * lerpThicknessSpeed;
                       yield return null;
                   }
               
                   laser.ResetLaser();
                   laser.length = 0;
               }
               
           });
           
           _exitActions.Add(EnemyStates.HIT, null);
           _exitActions.Add(EnemyStates.DEATH, null);
           _exitActions.Add(EnemyStates.KNOCKDOWN, null);
           
           //
           _entryActions.Add(EnemyStates.PREPARE, () =>
           {
               Debug.Log("Boss is preparing to attack.");
               _chargeTimer = 0;
               transform.LookAt(_target);
               _anim.SetTrigger("Prepare");
           });
           
           _updateActions.Add(EnemyStates.PREPARE, () =>
           {
               _chargeTimer += Time.deltaTime;

               if (_chargeTimer >= 3)
               {
                   SwitchState(EnemyStates.ATTACK);
               }
           });
           
           _exitActions.Add(EnemyStates.PREPARE, null);
        }

        private void Flee()
        {
            Vector3 dirToPlayer = transform.position - _target.transform.position;
            Vector3 newPos = transform.position + dirToPlayer;
            
            _agent.SetDestination(newPos);
        }

        private int CheckLegsAlive()
        {
            return legs.Count(x => !x.isDead);
        }

        private void ShootLaser()
        {
            StartCoroutine(Laser(_target));
        }

        private IEnumerator StompCooldown()
        {
            yield return new WaitForSeconds(attackCooldown);
            _canStomp = true;
        }
        
        private IEnumerator LaserCooldown()
        {
            yield return new WaitForSeconds(laserCooldown);
            _canLaser = true;
        }

        private IEnumerator Laser(Transform target)
        {
            laser.Activate();
            yield return new WaitForSeconds(laserActivateTimeDelay);
            float startLength = 0;
            
            float lerp = 0;
            
            while (lerp < 1)
            {
                laser.length = Mathf.Lerp(startLength, (target.position - eye.position).magnitude, lerp);
                lerp += Time.deltaTime * lerpSpeed;
                yield return null;
            }
            
            laser.HitActivate();
            //scorch.Play();
            
            while (true)
            {
               // scorch.transform.localPosition = new Vector3(target.position.x * 2 - 1f, target.position.y * 2 - 1f, 10);
                
                if ((target.position - eye.position).magnitude < 0)
                {
                    laser.length += Time.deltaTime * lerpSpeed * 20;
                    laser.HitDeactivate();
                }
                else
                {
                    laser.length = (target.position - eye.position).magnitude;
                }

                yield return null;
            }
        }
        
        public override void Update()
        {
            if (CheckLegsAlive() == 0)
            {
                SwitchState(EnemyStates.DEATH);
                return;
            }
            
            switch (_currentState) 
            {
                case EnemyStates.IDLE:
                    if (_target != null) SwitchState(EnemyStates.SEEK); 
                    break;
                case EnemyStates.SEEK:
                    if (_target == null) SwitchState(EnemyStates.IDLE);
                    if (_canLaser || _canStomp) SwitchState(EnemyStates.PREPARE);
                    break;
                case EnemyStates.ATTACK:
                    SwitchState(EnemyStates.IDLE);
                    break;
                case EnemyStates.HIT:
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
    }
}