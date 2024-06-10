using UnityEngine;

namespace FinalScripts
{
    [ExecuteAlways]
    public class Laser : MonoBehaviour
    {
        public Transform laserBody;
        public float length;
        [SerializeField] private float scaleFactor;
        float _startThickness;
        public float thickness;

        [SerializeField] ParticleSystem sourceEffect;
        [SerializeField] ParticleSystem hitEffect;
        [SerializeField] Transform hitPoint;

        private void Start()
        {
            _startThickness = thickness;
            scaleFactor = transform.localScale.x;
            
            if (transform.parent != null)
            {
                CalcScaleFactor(transform.parent);
            }
        }

        public void Activate()
        {
            if (!sourceEffect.isPlaying)
            {
                sourceEffect.Play();
            }
        }

        public void HitActivate()
        {
            if (!hitEffect.isPlaying)
            {
                hitEffect.Play();
            }
        }

        public void HitDeactivate()
        {
            hitEffect.Stop();
        }

        public void Deactivate()
        {
            sourceEffect.Stop();
        }

        void CalcScaleFactor(Transform parent)
        {
            scaleFactor *= parent.localScale.z;
            if (parent.parent != null)
            {
                CalcScaleFactor(parent.parent);
            }
        }

        public void ResetLaser()
        {
            Vector3 scale = laserBody.localScale;
            scale.y = 0;
            scale.x = 0;
            scale.z = 0;
            laserBody.localScale = scale;
            thickness = _startThickness;
        }

        private void Update()
        {
            Vector3 scale = laserBody.localScale;
            scale.y = thickness;
            scale.x = thickness;
            scale.z = (length / scaleFactor);
            laserBody.localScale = scale;

            hitEffect.transform.forward = hitPoint.transform.forward;
            hitEffect.transform.position = hitPoint.transform.position;
        }
    }
}