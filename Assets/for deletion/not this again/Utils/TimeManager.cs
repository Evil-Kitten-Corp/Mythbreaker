using BrunoMikoski.ServicesLocation;
using UnityEngine;

namespace not_this_again.Utils
{
    /// <summary>
    /// Used to make a slow-motion-type effect for extra dramatization of combos
    /// </summary>
    public class TimeManager: MonoBehaviour
    {
        public float slowMotionTime;
        public bool isSlowMotion;
        
        private void Awake()
        {
            ServiceLocator.Instance.RegisterInstance(this);
        }

        private void Update()
        {
            SlowMotionTimer();
        }

        private void SlowMotionTimer()
        {
            if (isSlowMotion && slowMotionTime > 0f)
            {
                slowMotionTime -= Time.unscaledDeltaTime;
                if (slowMotionTime <= 0f)
                {
                    OffSlowMotion();
                }
            }
        }

        public void OnSlowMotion(float timeScale, float timer = 0f)
        {
            Time.timeScale = timeScale;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            isSlowMotion = true;
            slowMotionTime = timer;
        }

        public void OffSlowMotion()
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            isSlowMotion = false;
        }
    }
}