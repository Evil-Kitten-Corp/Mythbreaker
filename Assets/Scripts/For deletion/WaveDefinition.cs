using SolidUtilities.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "WaveDef", menuName = "Wave Definition", order = 0)]
    public class WaveDefinition : ScriptableObject
    {
        public SerializableDictionary<GameObject, int> enemiesToSpawn;
    }
}