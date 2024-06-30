using System;
using DG.Tweening;
using UnityEngine;

namespace FinalScripts.Refactored.Utilities
{
    public class NewDissolve: MonoBehaviour
    {
        public float startDissolve = 0;
        public float endDissolve = 1;

        public float timer;
        
        public AnimationCurve curve;

        public Material dissolveMaterial;

        public MeshRenderer mr;
        public SkinnedMeshRenderer sr;

        private float _dissolveAmount;
        
        private static readonly int Dissolve = Shader.PropertyToID(DissolveProperty);
        private Material _cachedMat;

        const string DissolveProperty = "_Dissolve";
        public bool canStart = true;

        private void Start()
        {
            //cache
            if (mr != null)
            {
                mr.material = new Material(dissolveMaterial);
                _cachedMat = mr.material;
            }
            else if (sr != null)
            {
                sr.material = new Material(dissolveMaterial);
                _cachedMat = sr.material;
            }
            else
            {
                Debug.Log("Changing OG material!");
                _cachedMat = dissolveMaterial;
            }

            if (!canStart)
            {
                return;
            }
            
            ForceStart();
        }

        private void Update()
        {
            _cachedMat.SetFloat(Dissolve, _dissolveAmount);
        }

        public void ForceStart()
        {
            _cachedMat.SetFloat(Dissolve, 0);
            _dissolveAmount = startDissolve;
            DOTween.To(() => _dissolveAmount, x => _dissolveAmount = x, endDissolve, timer)
                .SetEase(curve)
                .OnComplete(() => Destroy(gameObject));
        }
    }
}