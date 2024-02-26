using System.Collections;
using System.Collections.Generic;
using Characters;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Ability_Behaviours.Ability_Instances
{
    public class Warp : Ability
    {
        [Header("[ Props ]")]
        public GameObject playerClone;
        public Material glowMaterial;
        public List<ParticleSystem> particles;
        public ParticleSystem weaponParticle;
        public GameObject hitParticle;

        [Header("[ References ]")]
        public PlayerManager player;
        public Transform target;
        public float warpDuration;
        
        private Volume _postVolume;
        private VolumeProfile _postProfile;
        private Transform _sword;
        private Transform _swordHand;
        private Vector3 _swordOrigPos;
        private Vector3 _swordOrigRot;
        private CinemachineImpulseSource _impulse;
        
        private static readonly int Hit = Animator.StringToHash("Hit");
        private static readonly int FresnelAmount = Shader.PropertyToID("_FresnelAmount");

        private void Start()
        {
            if (Camera.main != null) _postVolume = Camera.main.GetComponent<Volume>();
            _postProfile = _postVolume.profile;
            _impulse = FindObjectOfType<CinemachineImpulseSource>();
        }

        protected override IEnumerator Apply()
        {
            GameObject clone = Instantiate(playerClone, player.transform.position, player.transform.rotation);
            SkinnedMeshRenderer[] skinMeshList = clone.GetComponentsInChildren<SkinnedMeshRenderer>();

            foreach (var smr in skinMeshList)
            {
                smr.material = glowMaterial;
                smr.material.DOFloat(2, "_AlphaThreshold", 5f).OnComplete(() => Destroy(clone));
            }
            
            ShowBody(false);
            player.anim.speed = 0;

            transform.DOMove(target.position, warpDuration).SetEase(Ease.InExpo).OnComplete(FinishWarp);

            if (player.hasWeapon)
            {
                _sword = player.weapon.transform;
                
                _swordHand = _sword.parent;
                _swordOrigPos = _sword.localPosition;
                _swordOrigRot = _sword.localEulerAngles;
                
                _sword.parent = null;
                _sword.DOMove(target.position, warpDuration / 1.2f);
                _sword.DOLookAt(target.position, .2f, AxisConstraint.None);

                foreach (var ps in particles)
                {
                    ps.Play();
                }
                
                _sword.DORotate(new Vector3(0, 90, 0), .3f);
                
                DOVirtual.Float(0, -80, .2f, DistortionAmount);
                DOVirtual.Float(1, 2f, .2f, ScaleAmount);
            }

            return base.Apply();
        }
        
        void DistortionAmount(float x)
        {
            _postProfile.TryGet(out LensDistortion ld);
            ld.intensity.value = x;
        }
        
        void ScaleAmount(float x)
        {
            _postProfile.TryGet(out LensDistortion ld);
            ld.scale.value = x;
        }

        void FinishWarp()
        {
            ShowBody(true);

            if (_sword != null)
            {
                _sword.parent = _swordHand;
                _sword.localPosition = _swordOrigPos;
                _sword.localEulerAngles = _swordOrigRot;
            }

            SkinnedMeshRenderer[] skinMeshList = GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer smr in skinMeshList)
            {
                GlowAmount(30);
                DOVirtual.Float(30, 0, .5f, GlowAmount);
            }

            Instantiate(hitParticle, _sword.position, Quaternion.identity);

            target.GetComponentInParent<Animator>().SetTrigger(Hit);
            target.parent.DOMove(target.position + transform.forward, .5f);

            StartCoroutine(HideSword());
            StartCoroutine(PlayAnimation());
            StartCoroutine(StopParticles());

            //Shake
            _impulse.GenerateImpulse(Vector3.right);

            //Lens Distortion
            DOVirtual.Float(-80, 0, .2f, DistortionAmount);
            DOVirtual.Float(2f, 1, .1f, ScaleAmount);
        }
        
        IEnumerator PlayAnimation()
        {
            yield return new WaitForSeconds(.2f);
            player.anim.speed = 1;
        }

        IEnumerator StopParticles()
        {
            yield return new WaitForSeconds(.2f);

            foreach (var ps in particles)
            {
                ps.Stop();
            }
        }

        IEnumerator HideSword()
        {
            yield return new WaitForSeconds(.8f);
            weaponParticle.Play();

            GameObject swordClone = Instantiate(_sword.gameObject, _sword.position, _sword.rotation);

            _sword.GetComponent<MeshRenderer>().enabled = false;

            MeshRenderer swordMR = swordClone.GetComponentInChildren<MeshRenderer>();
            Material[] materials = swordMR.materials;

            for (int i = 0; i < materials.Length; i++)
            {
                Material m = glowMaterial;
                materials[i] = m;
            }

            swordMR.materials = materials;

            foreach (var t in swordMR.materials)
            {
                t.DOFloat(1, "_AlphaThreshold", .3f).OnComplete(() => Destroy(swordClone));
            }
        }
        
        void GlowAmount(float x)
        {
            SkinnedMeshRenderer[] skinMeshList = GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer smr in skinMeshList)
            {
                smr.material.SetVector(FresnelAmount, new Vector4(x, x, x, x));
            }
        }

        private void ShowBody(bool b)
        {
            SkinnedMeshRenderer[] skinnedMeshRenderers = player.GetComponentsInChildren<SkinnedMeshRenderer>();

            foreach (var smr in skinnedMeshRenderers)
            {
                smr.enabled = b;
            }
        }
    }
}