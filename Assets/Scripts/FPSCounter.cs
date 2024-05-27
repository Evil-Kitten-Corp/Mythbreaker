using System.Linq;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class FPSCounter : MonoBehaviour
    {
        public TMP_Text fpsCounter;
        
        private int _lastFrameIndex;
        private float[] _frameDeltaTimeArray;

        private void Awake()
        {
            _frameDeltaTimeArray = new float[50];
        }

        private void Update()
        {
            _frameDeltaTimeArray[_lastFrameIndex] = Time.unscaledDeltaTime;
            _lastFrameIndex = (_lastFrameIndex + 1) % _frameDeltaTimeArray.Length;

            fpsCounter.text = Mathf.RoundToInt(CalculateFPS()) + " FPS";
        }

        private float CalculateFPS()
        {
            float t = _frameDeltaTimeArray.Sum();

            return _frameDeltaTimeArray.Length / t;
        }
    }
}