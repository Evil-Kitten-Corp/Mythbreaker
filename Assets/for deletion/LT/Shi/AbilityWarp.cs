using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace LT.Shi
{
    public class WarpController : MonoBehaviour
    {
        public vThirdPersonController input;
        public Animator anim;

        public bool isLocked;
    
        public CinemachineImpulseSource impulse;
        public Volume postVolume;
        public string moveSpeedFloat;

        [Space]

        public List<Transform> screenTargets = new();
        public Transform target;
        public float warpDuration = .5f;

        [Space]

        public Transform sword;
        public Transform swordHand;
        private Vector3 swordOrigRot;
        private Vector3 swordOrigPos;
        private MeshRenderer swordMesh;

        [Space]
        public Material glowMaterial;

        [Space]

        [Header("Particles")]
        public ParticleSystem blueTrail;
        public ParticleSystem whiteTrail;
        public ParticleSystem swordParticle;

        [Space]

        [Header("Prefabs")]
        public GameObject hitParticle;

        [Space]

        [Header("Canvas")]
        public Image aim;
        public Image lockAim;
        public Vector2 uiOffset;

        // Start is called before the first frame update
        void Start()
        {
            Cursor.visible = false;

            //input = GetComponent<MovementInput>();
            swordOrigRot = sword.localEulerAngles;
            swordOrigPos = sword.localPosition;
            swordMesh = sword.GetComponentInChildren<MeshRenderer>();
            swordMesh.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            anim.SetFloat(moveSpeedFloat, input.moveSpeed);
            UserInterface();

            if (input.lockMovement)
                return;

            if (screenTargets.Count < 1)
                return;

            if (!isLocked)
            {
                target = screenTargets[TargetIndex()];
            }

            if (Input.GetMouseButtonDown(1)) {
                LockInterface(true);
                isLocked = true;
            }

            if (Input.GetMouseButtonUp(1) && !input.lockMovement)
            {
                LockInterface(false);
                isLocked = false;
            }

            if (!isLocked)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                input.RotateToPosition(target.position);
                transform.LookAt(target);
                input.lockMovement = true;
                swordParticle.Play();
                swordMesh.enabled = true;
                anim.SetTrigger("Slash");
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.visible = false;
            }
        }

        private void UserInterface()
        {

            aim.transform.position = Camera.main.WorldToScreenPoint(target.position + (Vector3)uiOffset);

            if (input.lockMovement)
                return;

            Color c = screenTargets.Count < 1 ? Color.clear : Color.white;
            aim.color = c;
        }

        void LockInterface(bool state)
        {
            float size = state ? 1 : 2;
            float fade = state ? 1 : 0;
            lockAim.DOFade(fade, .15f);
            lockAim.transform.DOScale(size, .15f).SetEase(Ease.OutBack);
            lockAim.transform.DORotate(Vector3.forward * 180, .15f, RotateMode.FastBeyond360).From();
            aim.transform.DORotate(Vector3.forward * 90, .15f, RotateMode.LocalAxisAdd);
        }

        public void Warp()
        {
            GameObject clone = Instantiate(gameObject, transform.position, transform.rotation);
            clone.transform.localScale = transform.localScale;
            clone.GetComponent<Rigidbody>().isKinematic = true;
            Destroy(clone.GetComponent<WarpController>().sword.gameObject);
            Destroy(clone.GetComponent<Animator>());
            Destroy(clone.GetComponent<WarpController>());
            Destroy(clone.GetComponent<vThirdPersonController>());
            Destroy(clone.GetComponent<CharacterController>());

            SkinnedMeshRenderer[] skinMeshList = clone.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer smr in skinMeshList)
            {
                smr.material = glowMaterial;
                smr.material.DOFloat(1, "_AlphaThreshold", 5f).OnComplete(()=>Destroy(clone));
            }

            ShowBody(false);
            anim.speed = 0;

            transform.DOMove(target.position, warpDuration).SetEase(Ease.InExpo).OnComplete(()=>FinishWarp());

            sword.parent = null;
            sword.DOMove(target.position, warpDuration/1.2f);
            sword.DOLookAt(target.position, .2f, AxisConstraint.None);

            //Particles
            blueTrail.Play();
            whiteTrail.Play();

            //Lens Distortion
            DOVirtual.Float(0, -80, .2f, DistortionAmount);
            DOVirtual.Float(1, 2f, .2f, ScaleAmount);
        }

        void FinishWarp()
        {
            ShowBody(true);

            sword.parent = swordHand;
            sword.localPosition = swordOrigPos;
            sword.localEulerAngles = swordOrigRot;

            SkinnedMeshRenderer[] skinMeshList = GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer smr in skinMeshList)
            {
                GlowAmount(30);
                DOVirtual.Float(30, 0, .5f, GlowAmount);
            }

            Instantiate(hitParticle, sword.position, Quaternion.identity);

            //target.GetComponentInParent<Animator>().SetTrigger("Hit");
            target.parent.DOMove(target.position + transform.forward, .5f);

            StartCoroutine(HideSword());
            StartCoroutine(PlayAnimation());
            StartCoroutine(StopParticles());

            isLocked = false;
            LockInterface(false);
            aim.color = Color.clear;

            //Shake
            impulse.GenerateImpulse(Vector3.right);

            //Lens Distortion
            DOVirtual.Float(-80, 0, .2f, DistortionAmount);
            DOVirtual.Float(2f, 1, .1f, ScaleAmount);
        }

        IEnumerator PlayAnimation()
        {
            yield return new WaitForSeconds(.2f);
            anim.speed = 1;
        }

        IEnumerator StopParticles()
        {
            yield return new WaitForSeconds(.2f);
            blueTrail.Stop();
            whiteTrail.Stop();
        }

        IEnumerator HideSword()
        {
            yield return new WaitForSeconds(.8f);
            swordParticle.Play();

            GameObject swordClone = Instantiate(sword.gameObject, sword.position, sword.rotation);

            swordMesh.enabled = false;

            MeshRenderer swordMr = swordClone.GetComponentInChildren<MeshRenderer>();
            Material[] materials = swordMr.materials;

            for (int i = 0; i < materials.Length; i++)
            {
                Material m = glowMaterial;
                materials[i] = m;
            }

            swordMr.materials = materials;

            foreach (var t in swordMr.materials)
            {
                t.DOFloat(1, "_AlphaThreshold", .3f).OnComplete(() => Destroy(swordClone));
            }

            input.lockMovement = false;
        }


        void ShowBody(bool state)
        {
            SkinnedMeshRenderer[] skinMeshList = GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer smr in skinMeshList)
            {
                smr.enabled = state;
            }
        }

        void GlowAmount(float x)
        {
            SkinnedMeshRenderer[] skinMeshList = GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer smr in skinMeshList)
            {
                smr.material.SetVector("_FresnelAmount", new Vector4(x, x, x, x));
            }
        }

        void DistortionAmount(float x)
        {
            postVolume.profile.TryGet<LensDistortion>(out var len);
            len.intensity.value = x;
        }
        void ScaleAmount(float x)
        {
            postVolume.profile.TryGet<LensDistortion>(out var len);
            len.scale.value = x;
        }

        public int TargetIndex()
        {
            float[] distances = new float[screenTargets.Count];

            for (int i = 0; i < screenTargets.Count; i++)
            {
                distances[i] = Vector2.Distance(Camera.main.WorldToScreenPoint(screenTargets[i].position), 
                    new Vector2(Screen.width / 2, Screen.height / 2));
            }

            float minDistance = Mathf.Min(distances);
            int index = 0;

            for (int i = 0; i < distances.Length; i++)
            {
                if (minDistance == distances[i])
                    index = i;
            }

            return index;
        }
    }
}