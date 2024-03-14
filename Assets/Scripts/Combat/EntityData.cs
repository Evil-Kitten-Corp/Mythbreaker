using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(fileName = "EntityData", menuName = "New Entity Combat Data", order = 0)]
    public class EntityData : ScriptableObject
    {
        public float maxHealth;
        public float baseDamage;
    }
}