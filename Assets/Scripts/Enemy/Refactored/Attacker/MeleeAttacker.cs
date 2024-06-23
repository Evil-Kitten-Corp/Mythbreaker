using System;
using System.Collections.Generic;
using UnityEngine;

namespace FinalScripts.Refactored.Attacker
{
    public class MeleeAttacker : MonoBehaviour
    {
        public int damage = 1;

        [Serializable]
        public class AttackPoint
        {
            public float radius;
            public Vector3 offset;
            public Transform attackRoot;

#if UNITY_EDITOR
            //editor only as it's only used in editor to display the path of the attack that is used by the raycast
            [NonSerialized] public List<Vector3> previousPositions = new List<Vector3>();
#endif
        }

        public ParticleSystem hitParticlePrefab;
        public LayerMask targetLayers;

        public AttackPoint[] attackPoints = Array.Empty<AttackPoint>();

        [Header("Audio")] public RandomAudioPlayer hitAudio;
        public RandomAudioPlayer attackAudio;

        public bool ThrowingHit
        {
            get => _isThrowingHit;
            set => _isThrowingHit = value;
        }

        private GameObject _owner;

        private Vector3[] _previousPos = null;
        private Vector3 _direction;

        private bool _isThrowingHit = false;
        private bool _inAttack = false;

        const int PARTICLE_COUNT = 10;
        private readonly ParticleSystem[] _particlesPool = new ParticleSystem[PARTICLE_COUNT];
        private int _currentParticle = 0;

        private static readonly RaycastHit[] s_RaycastHitCache = new RaycastHit[32];
        protected static Collider[] s_ColliderCache = new Collider[32];

        private void Awake()
        {
            if (hitParticlePrefab != null)
            {
                for (int i = 0; i < PARTICLE_COUNT; ++i)
                {
                    _particlesPool[i] = Instantiate(hitParticlePrefab);
                    _particlesPool[i].Stop();
                }
            }
        }

        public void SetOwner(GameObject owner)
        {
            _owner = owner;
        }

        public void BeginAttack(bool throwingAttack)
        {
            if (attackAudio != null)
                attackAudio.PlayRandomClip();
            ThrowingHit = throwingAttack;

            _inAttack = true;

            _previousPos = new Vector3[attackPoints.Length];

            for (int i = 0; i < attackPoints.Length; ++i)
            {
                Vector3 worldPos = attackPoints[i].attackRoot.position +
                                   attackPoints[i].attackRoot.TransformVector(attackPoints[i].offset);
                _previousPos[i] = worldPos;

#if UNITY_EDITOR
                attackPoints[i].previousPositions.Clear();
                attackPoints[i].previousPositions.Add(_previousPos[i]);
#endif
            }
        }

        public void EndAttack()
        {
            _inAttack = false;


#if UNITY_EDITOR
            for (int i = 0; i < attackPoints.Length; ++i)
            {
                attackPoints[i].previousPositions.Clear();
            }
#endif
        }

        private void FixedUpdate()
        {
            if (_inAttack)
            {
                for (int i = 0; i < attackPoints.Length; ++i)
                {
                    AttackPoint pts = attackPoints[i];

                    Vector3 worldPos = pts.attackRoot.position + pts.attackRoot.TransformVector(pts.offset);
                    Vector3 attackVector = worldPos - _previousPos[i];

                    if (attackVector.magnitude < 0.001f)
                    { 
                        attackVector = Vector3.forward * 0.0001f;
                    }


                    Ray r = new Ray(worldPos, attackVector.normalized);

                    int contacts = Physics.SphereCastNonAlloc(r, pts.radius, s_RaycastHitCache, 
                        attackVector.magnitude, ~0, QueryTriggerInteraction.Ignore);

                    for (int k = 0; k < contacts; ++k)
                    {
                        Collider col = s_RaycastHitCache[k].collider;

                        if (col != null)
                            CheckDamage(col, pts);
                    }

                    _previousPos[i] = worldPos;

#if UNITY_EDITOR
                    pts.previousPositions.Add(_previousPos[i]);
#endif
                }
            }
        }

        private bool CheckDamage(Collider other, AttackPoint pts)
        {
            AttributesManager attributesManager = other.GetComponent<AttributesManager>();
            
            if (attributesManager == null)
            {
                return false;
            }

            if (hitAudio != null)
            {
                hitAudio.PlayRandomClip();
            }
            
            attributesManager.TakeDamage(damage);

            if (hitParticlePrefab != null)
            {
                _particlesPool[_currentParticle].transform.position = pts.attackRoot.transform.position;
                _particlesPool[_currentParticle].time = 0;
                _particlesPool[_currentParticle].Play();
                _currentParticle = (_currentParticle + 1) % PARTICLE_COUNT;
            }

            return true;
        }

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            for (int i = 0; i < attackPoints.Length; ++i)
            {
                AttackPoint pts = attackPoints[i];

                if (pts.attackRoot != null)
                {
                    Vector3 worldPos = pts.attackRoot.TransformVector(pts.offset);
                    Gizmos.color = new Color(1.0f, 1.0f, 1.0f, 0.4f);
                    Gizmos.DrawSphere(pts.attackRoot.position + worldPos, pts.radius);
                }

                if (pts.previousPositions.Count > 1)
                {
                    UnityEditor.Handles.DrawAAPolyLine(10, pts.previousPositions.ToArray());
                }
            }
        }

#endif
    }
}