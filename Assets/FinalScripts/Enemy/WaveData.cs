using AYellowpaper.SerializedCollections;
using SolidUtilities.Collections;
using TriInspector;
using UnityEngine;

namespace FinalScripts
{
    [CreateAssetMenu(fileName = "WaveData", menuName = "Mythbreaker/Wave", order = 0)]
    public class WaveData : ScriptableObject
    {
        [Title("Spawning")] [SerializedDictionary("Enemy", "Amount To Spawn")]
        public SerializableDictionary<EnemyStats, int> spawnData;
    }
}