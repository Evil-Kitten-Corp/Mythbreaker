using TriInspector;
using UnityEngine;

namespace FinalScripts
{
    [CreateAssetMenu(fileName = "Enemy Stats", menuName = "Mythbreaker/Enemy", order = 0)]
    public class EnemyStats : ScriptableObject
    {
        public GameObject prefab;

        public float health;
        public float speed;
        public float attackDamage;

        public bool rangedEnemy;
        [EnableIf(nameof(rangedEnemy))] public float range = 10f;
    }
}