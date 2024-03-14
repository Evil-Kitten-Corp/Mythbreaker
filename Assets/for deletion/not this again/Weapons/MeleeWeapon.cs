using System.Collections;
using System.Linq;
using BrunoMikoski.ServicesLocation;
using not_this_again.Enums;
using not_this_again.Utils;
using TriInspector;
using UnityEngine;

namespace not_this_again.Weapons
{
    public class MeleeWeapon : Weapon
    {
        [Title("Weapon Trail")]
        public TraceType traceType;
        public float traceRadius = 0.5f;

        [Title("Contact Points")]
        [TableList(Draggable = true,
            HideAddButton = false,
            HideRemoveButton = false,
            AlwaysExpanded = false)] 
        public AttackPoint[] attackPoints = new AttackPoint[0];
        
        private Coroutine _cTrace;
        
        const int PARTICLE_COUNT = 10;
        protected ParticleSystem[] m_ParticlesPool = new ParticleSystem[PARTICLE_COUNT];
        protected int m_CurrentParticle;
        
        public bool IsTrace => WeaponCollider.enabled;

        private void Start()
        {
            WeaponCollider = GetComponent<Collider>();
        }
        
        public void ShakeDirection(Vector3 direction)
        {
            hitShake.m_DefaultVelocity = direction;
        }

        private IEnumerator BoxTrace()
        {
            yield return new WaitForEndOfFrame();
            while (IsTrace)
            {
                Collider[] colls = Physics.OverlapBox(halfExtents: new Vector3(traceRadius, 
                        traceRadius, traceRadius) * 0.5f, 
                    center: WeaponCollider.bounds.center, orientation: transform.rotation, layerMask: targetLayers.value);
                
                colls = colls.Where(coll => coll.gameObject != _mOwner.gameObject).ToArray();
                Collider[] array = colls;
                
                foreach (Collider coll2 in array)
                {
                    if (coll2.GetComponentInParent<IDamageable>() != null)
                    {
                        IDamageable enemy = coll2.GetComponentInParent<IDamageable>();
                        
                        switch (enemy.CombatData.combatType)
                        {
                            case CombatType.None:
                            case CombatType.Attack:
                            case CombatType.HitReaction:
                                enemy.TakeDamage(_mOwner, 0f);
                                enemy.SetLookAt(_mOwner.gameObject, 0.5f, 3f);
                                ServiceLocator.Instance.GetInstance<TimeManager>().OnSlowMotion(0.1f, 0.065f);
                                break;
                            case CombatType.Dodge:
                                yield break;
                        }
                        yield return new WaitForSeconds(0.1f);
                        //ShakeDirection(hitDecalComponent.GetDirection);
                    }
                }
                yield return null;
            }
        }

        public void PlayTrace()
        {
            if (_cTrace != null)
            {
                StopCoroutine(_cTrace);
            }
            if (traceType == TraceType.Box)
            {
                _cTrace = StartCoroutine(BoxTrace());
            }
        }
        
        public override void Attack(bool throwingAttack)
        {
            _mIsThrowingHit = throwingAttack;

            _mInAttack = true;

            _mPreviousPos = new Vector3[attackPoints.Length];

            for (int i = 0; i < attackPoints.Length; ++i)
            {
                Vector3 worldPos = attackPoints[i].attackRoot.position +
                                   attackPoints[i].attackRoot.TransformVector(attackPoints[i].offset);
                _mPreviousPos[i] = worldPos;
            }
        }

        public void EndAttack()
        {
            _mInAttack = false;
        }

        private void FixedUpdate()
        {
            if (_mInAttack)
            {
                for (int i = 0; i < attackPoints.Length; ++i)
                {
                    AttackPoint pts = attackPoints[i];

                    Vector3 worldPos = pts.attackRoot.position + pts.attackRoot.TransformVector(pts.offset);
                    Vector3 attackVector = worldPos - _mPreviousPos[i];

                    if (attackVector.magnitude < 0.001f)
                    {
                        // A zero vector for the sphere cast don't yield any result, even if a collider overlap
                        // the "sphere" created by radius. 
                        // so we set a very tiny microscopic forward cast to be sure it will catch anything
                        // overlaping that "stationary" sphere cast
                        attackVector = Vector3.forward * 0.0001f;
                    }


                    Ray r = new Ray(worldPos, attackVector.normalized);

                    int contacts = Physics.SphereCastNonAlloc(r, pts.radius, _sRaycastHitCache, 
                        attackVector.magnitude, ~0, QueryTriggerInteraction.Ignore);

                    for (int k = 0; k < contacts; ++k)
                    {
                        Collider col = _sRaycastHitCache[k].collider;

                        if (col != null)
                            CheckDamage(col, pts);
                    }

                    _mPreviousPos[i] = worldPos;
                }
            }
        }

        private bool CheckDamage(Collider other, AttackPoint pts)
        {
            IDamageable d = other.GetComponent<IDamageable>();
            
            if (d == null)
            {
                return false;
            }

            if (d.GetGameObject() == _mOwner)
                return true; //ignore self harm, but do not end the attack (we don't "bounce" off ourselves)

            if ((targetLayers.value & (1 << other.gameObject.layer)) == 0)
            {
                //hit an object that is not in our layer, this end the attack. we "bounce" off it
                return false;
            }

            if (hitAudio.canPlay)
            {
                hitAudio.PlayRandomClip();
            }

            DamageMessage data;

            data.amount = baseDamage;
            data.damager = this;
            data.direction = _mDirection.normalized;
            data.damageSource = _mOwner.transform.position;
            data.throwing = _mIsThrowingHit;
            data.stopCamera = false;

            d.ApplyDamage(data);

            if (hitParticlePrefab != null)
            {
                m_ParticlesPool[m_CurrentParticle].transform.position = pts.attackRoot.transform.position;
                m_ParticlesPool[m_CurrentParticle].time = 0;
                m_ParticlesPool[m_CurrentParticle].Play();
                m_CurrentParticle = (m_CurrentParticle + 1) % PARTICLE_COUNT;
            }

            return true;
        }
    }
}