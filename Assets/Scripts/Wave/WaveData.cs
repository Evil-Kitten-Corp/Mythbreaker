using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace FinalScripts
{
    [CreateAssetMenu(fileName = "WaveData", menuName = "Mythbreaker/Wave", order = 0)]
    public class WaveData : ScriptableObject
    {
        [Title("Spawning")] 
        
        [TableList(Draggable = true, HideAddButton = false,
            HideRemoveButton = false, AlwaysExpanded = true)]
        public List<SpawnData> spawnData;

        [Serializable]
        public struct SpawnData
        {
            public EnemyStats enemyType;
            public int amountToSpawn;
            public Transform spawnPoint;
            [Tooltip("This should never be less than 1")] public float delayBetweenSpawns;
        }
    }
}