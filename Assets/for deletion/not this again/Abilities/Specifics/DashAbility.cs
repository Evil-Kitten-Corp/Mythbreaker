using Cinemachine;
using DG.Tweening;
using not_this_again.Weapons;
using TNRD;
using UnityEngine;
using UnityEngine.Rendering;

namespace not_this_again.Abilities.Specifics
{
    public class DashAbility : Ability
    {
        [Header("Connections")]
        [SerializeField] private Animator animator = default;
        [SerializeField] private CinemachineFreeLook originalCam = default;
        public SerializableInterface<IDamageable> damageable = default;
        
        [Header("Visuals")]
        [SerializeField] private Renderer skinnedMesh = default;
        [SerializeField] private ParticleSystem dashParticle = default;
        [SerializeField] private Volume dashVolume = default;
        
        public override void MyAbility()
        {
            animator.SetTrigger("Dash");
            dashParticle.Play();

            Sequence dash = DOTween.Sequence()
                .AppendCallback(() => damageable.Value.IsInvulnerable = true)
                .Insert(0, transform.DOMove(transform.position + (transform.forward * 5), .2f))
                .AppendCallback(() => dashParticle.Stop())
                .Insert(0, skinnedMesh.material.DOFloat(1, "FresnelAmount", .1f))
                .Append(skinnedMesh.material.DOFloat(0, "FresnelAmount", .35f))
                .AppendCallback(() => damageable.Value.IsInvulnerable = false);


            DOVirtual.Float(0, 1, .1f, SetDashVolumeWeight)
                .OnComplete(() => DOVirtual.Float(1, 0, .5f, SetDashVolumeWeight));

            DOVirtual.Float(40, 50, .1f, SetCameraFOV)
                .OnComplete(() => DOVirtual.Float(50, 40, .5f, SetCameraFOV));
        }
        
        void SetDashVolumeWeight(float weight)
        {
            dashVolume.weight = weight;
        }

        void SetCameraFOV(float fov)
        {
            originalCam.m_Lens.FieldOfView = fov;
        }
    }
}