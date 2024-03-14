using Cinemachine;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Base
{
    public class EntityReferenceHelper : MonoBehaviour
    {
        public vThirdPersonController input;
        public Animator anim;
        
        [Space]
        public CinemachineImpulseSource impulse;
        public Volume postVolume;
        
        [Space]
        public Transform weapon;
        public Transform swordHand;
        public Transform spine;
        [Tooltip("Throw")] public Transform curvePoint;
        public Material glowMaterial;
        
        [Space] [Header("Wrap")]
        public ParticleSystem blueTrail;
        public ParticleSystem whiteTrail;
        public ParticleSystem swordParticle;
        public GameObject hitParticle;
        
        [Space]
        public Image aim;
        public Image lockAim;

        [Space] [Header("Throw")]
        public ParticleSystem glowParticle;
        public ParticleSystem catchParticle;
        public ParticleSystem trailParticle;
        public TrailRenderer trailRenderer;
    
    }
}