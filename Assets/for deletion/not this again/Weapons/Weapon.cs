using Cinemachine;
using TriInspector;
using UnityEngine;

namespace not_this_again.Weapons
{
    [RequireComponent(typeof(Collider))]
    public abstract class Weapon : MonoBehaviour
    {
        [Title("Base Stats")]
        public int baseDamage;
        public LayerMask targetLayers;
        
        [Title("Equipment Sockets")]
        [SerializeReference] public WeaponSocket equipSocket;
        [SerializeReference] public WeaponSocket unequipSocket;
        
        [Title("Effects")]
        public ParticleSystem hitParticlePrefab;
        [SerializeReference] public RandomAudioPlay hitAudio;
        public CinemachineCollisionImpulseSource hitShake;
        
        /* Privates */
        protected Character.PlayerController _mOwner;
        protected Vector3[] _mPreviousPos = null;
        protected Vector3 _mDirection;
        protected bool _mIsThrowingHit = false;
        protected bool _mInAttack = false;
        protected RaycastHit[] _sRaycastHitCache = new RaycastHit[32];

        public Collider WeaponCollider;
        
        public void SetOwner(Character.PlayerController owner)
        {
            _mOwner = owner;
        }

        public virtual void Attack(bool throwingAttack)
        {
            
        }
    }
}