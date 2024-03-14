using System;
using not_this_again.Enums;
using TriInspector;

namespace not_this_again.Character.Data
{
    [Serializable]
    public struct CombatData
    {
        [Title("Combat")]
        public CombatType combatType;
        public AttackType attackType;
        public AttackDirection attackDirection;
    }
}