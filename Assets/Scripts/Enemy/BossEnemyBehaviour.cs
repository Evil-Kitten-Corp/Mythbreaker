using System.Collections;
using System.Linq;
using Abilities;
using DG.Tweening;
using UnityEngine;

namespace FinalScripts
{
    public class BossEnemyBehaviour : EnemyBehaviour
    {
        public EnemyAppendage[] legs;
        public Transform eye;
        public Laser laser;
        public AudioClip laserSound;
        public float laserCooldown;
        private float _chargeTimer;
        private float _laserTimer;

        public Light eyeLight;
        public float laserActivateTimeDelay;
        public float lerpSpeed;
        public float lerpThicknessSpeed;
        public ParticleSystem scorch;

        public GameObject[] laserComponents;
        public LineRenderer[] laserParts;
        public GameObject laserHit;
        
        public float stompRadius = 5.0F;
        public float stompPower = 10.0F;
        public AudioClip stompSound;

        public AudioClip deathSound;
        
        public ParticleSystem shockwave;
        
        private float _lastUsedLaser;

        private bool _canLaser = false;
        private bool _canStomp = true;

        private Coroutine _laserCo;
        private Coroutine _chargeCo;
        private bool _forceEmptyTarget;

        public override void Start()
        {
            _canBeKnockedup = false;
            _lastUsedLaser = Time.time;
            StartCoroutine(LaserCooldown());
            base.Start();

            _target = GameObject.FindWithTag("ShootPt").transform;

            if (_target != null)
            {
                _target.GetComponentInParent<AttributesManager>().OnDie += () =>
                {
                    _forceEmptyTarget = true;
                    _target = null;
                };
            }
            
            _agent.updateRotation = false;
            
            // [ IDLE ]
            
            _entryActions.Add(EnemyStates.IDLE, null);

            _updateActions.Add(EnemyStates.IDLE, () =>
            {
                if (!_forceEmptyTarget)
                {
                    _target = GameObject.FindWithTag("ShootPt").transform;
                }
            });
            
            _exitActions.Add(EnemyStates.IDLE, null);
            
            //ENTRY
            _entryActions.Add(EnemyStates.KNOCKDOWN, null);
            
            _entryActions.Add(EnemyStates.SEEK, () => 
            { 
                Debug.Log("Boss is running away.");
                //we will actually use this to instead flee
                //_anim.SetBool(Walking, true); 
            });
           
           _entryActions.Add(EnemyStates.ATTACK, () =>
           {
               Debug.Log("Boss is attacking."); 
               _laserTimer = 0;
               _lastUsedLaser = Time.time;
               
               if (_laserCo == null && _canLaser)
               {
                   //check if the player is in our fov, to evade the case where the player is under us,
                   //and we still laser shoot it on an ungodly angle
                   Vector3 targetDir = _target.transform.position - transform.position;
                   float angle = Vector3.Angle(targetDir, transform.forward);
                   
                   if (angle <= 45)
                   {
                       ShootLaser();
                       Debug.Log("I see you.");
                       return;
                   }
                   else
                   {
                       Debug.Log("I can't see you.");
                   }
               }
               
               shockwave.Play();
               GetComponent<AudioSource>().PlayOneShot(stompSound);

               var att = _target.GetComponentInParent<AttributesManager>();

               if (att != null)
               {
                   if (att.CanStomp())
                   {
                       Debug.Log("Can be stomped.");
                       Rigidbody rb = _target.GetComponent<Rigidbody>();

                       if (rb != null)
                       {
                           rb.AddExplosionForce(stompPower, transform.position, stompRadius, 3.0F);
                           Debug.Log("We actually added force to Vita's rb.");
                       }
                   
                       att.Knockup();
                   }
               }

               _canStomp = false;
               StartCoroutine(StompCooldown());
           });
           
           _entryActions.Add(EnemyStates.HIT, null);
           
           _entryActions.Add(EnemyStates.DEATH, () =>
           {
               Debug.Log("Boss is dead.");
               _anim.SetTrigger(Die);
               
               if (TargetingSystem.Instance != null)
               {
                   TargetingSystem.Instance.screenTargets.Remove(gameObject);
               }
               
               OnDeath?.Invoke(gameObject);
               Destroy(gameObject, 2f);
               var aud = GetComponent<AudioSource>();
               aud.volume = 0.4f;
               aud.PlayOneShot(deathSound);
               _target.GetComponentInParent<AttributesManager>().OnDefeatBoss?.Invoke();
           });
           
           //UPDATE
           
           _updateActions.Add(EnemyStates.SEEK, null);
           _updateActions.Add(EnemyStates.ATTACK, null);
           _updateActions.Add(EnemyStates.HIT, null);
           _updateActions.Add(EnemyStates.DEATH, null);
           _updateActions.Add(EnemyStates.KNOCKDOWN, null);
           
           //EXIT
           
           
           _exitActions.Add(EnemyStates.SEEK, null);
           _exitActions.Add(EnemyStates.ATTACK, null);
           _exitActions.Add(EnemyStates.HIT, null);
           _exitActions.Add(EnemyStates.DEATH, null);
           _exitActions.Add(EnemyStates.KNOCKDOWN, null);
           
           // [ PREPARE ]
           _entryActions.Add(EnemyStates.PREPARE, () =>
           {
               Debug.Log("Boss is preparing to attack.");
               _chargeTimer = 0;
               transform.LookAt(_target);
               _anim.SetTrigger("Prepare");
           });
           
           _updateActions.Add(EnemyStates.PREPARE, () => { _chargeCo ??= StartCoroutine(Charge()); });
           
           _exitActions.Add(EnemyStates.PREPARE, null);
           
           Debug.Log("Entry Actions Keys: " + string.Join(", ", _entryActions.Keys));
           Debug.Log("Update Actions Keys: " + string.Join(", ", _updateActions.Keys));
           Debug.Log("Exit Actions Keys: " + string.Join(", ", _exitActions.Keys));
        }

        private void Flee()
        {
            Vector3 dirToPlayer = transform.position - _target.transform.position;
            Vector3 newPos = transform.position + dirToPlayer;
            transform.LookAt(_target); 
            
            _agent.SetDestination(newPos);
        }

        private int CheckLegsAlive()
        {
            return legs.Count(x => !x.isDead);
        }

        private void ShootLaser()
        {
            _laserTimer = 0;
            _laserCo = StartCoroutine(Laser(_target));
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
            _laserCo = null;
        }

        private IEnumerator Charge()
        {
            eyeLight.color = Color.red;
            eyeLight.intensity = 0;
            eyeLight.DOIntensity(10, laserActivateTimeDelay);
            GetComponent<AudioSource>().PlayOneShot(laserSound);
            yield return new WaitForSeconds(laserActivateTimeDelay);
            
            SwitchState(EnemyStates.ATTACK);
            _chargeCo = null;
        }

        private IEnumerator Laser(Transform target)
        {
            foreach (var go in laserComponents)
            {
                go.SetActive(true);
            }
            
            scorch.Play();
            
            while (_laserTimer < 3)
            {
                _laserTimer += Time.deltaTime;
                
                foreach (var line in laserParts)
                {
                    line.SetPosition(0, eye.position);
                    line.SetPosition(1, target.position);
                }

                laserHit.transform.position = laserParts[0].GetPosition(1);

                yield return null;
            }
            
            _canLaser = false;
            SwitchState(EnemyStates.IDLE);
            _lastAttackTime = Time.time;

            foreach (var go in laserComponents)
            {
                go.SetActive(false); 
            }
            
            eyeLight.DOColor(Color.green, 1f);
            eyeLight.DOIntensity(38, 1f);
            yield return new WaitForSeconds(1f);
            StartCoroutine(LaserCooldown());
        }
        
        public override void Update()
        {
            if (CheckLegsAlive() == 0)
            {
                SwitchState(EnemyStates.DEATH);
                return;
            }
            
            if (!_forceEmptyTarget && _currentState != EnemyStates.PREPARE && _currentState != EnemyStates.DEATH)
            {
                //check if the player is in our fov
                Vector3 targetDir = _target.transform.position - transform.position;
                float angle = Vector3.Angle(targetDir, transform.forward);
                   
                if (angle <= 45)
                {
                    var targetRotation = Quaternion.LookRotation(_target.transform.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, .5f * Time.deltaTime);
                }
                
                //else lets just not frustrate the player a lot
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
                    if (!_canLaser && !_canStomp) 
                    {
                        SwitchState(EnemyStates.IDLE);
                    }
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