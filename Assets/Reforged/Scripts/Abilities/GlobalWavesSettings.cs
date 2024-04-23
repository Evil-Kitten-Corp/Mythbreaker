using System.Collections.Generic;
using Minimalist.Bar.Utility;

namespace Abilities
{
    public class GlobalWavesSettings : Singleton<GlobalWavesSettings>
    {
        private Wave _currentWave;
        public List<Wave> waves;

        private void Start()
        {
            if (waves.Count > 0)
                _currentWave = waves[0];
        }

        public Wave NextWave()
        {
            int currentIndex = waves.IndexOf(_currentWave);
            
            if (currentIndex >= 0 && currentIndex + 1 < waves.Count)
                return waves[currentIndex + 1];
            
            return null;
        }
    }

    [System.Serializable]
    public class Wave
    {
        public bool complete;
    }
}