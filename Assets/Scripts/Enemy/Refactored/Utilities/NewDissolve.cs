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

        private float _dissolveAmount;
        
        private static readonly int Dissolve = Shader.PropertyToID(DissolveProperty);

        const string DissolveProperty = "_Dissolve";

        private void Start()
        {
            dissolveMaterial.SetFloat(Dissolve, 0);
            _dissolveAmount = startDissolve;
            DOTween.To(() => _dissolveAmount, x => _dissolveAmount = x, endDissolve, timer)
                .SetEase(curve)
                .OnComplete(() => Destroy(gameObject));
        }

        private void Update()
        {
            dissolveMaterial.SetFloat(Dissolve, _dissolveAmount);
        }
    }
}