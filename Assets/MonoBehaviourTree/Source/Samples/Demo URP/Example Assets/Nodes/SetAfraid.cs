using FinalScripts.Refactored;
using UnityEngine;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode(name = "Combat/Set Afraid")]
    public class SetAfraid : Leaf
    {
        public RangedEnemyBT rangedComponent;
        [SerializeField] public bool afraid;

        public override NodeResult Execute()
        {
            rangedComponent.Afraid(afraid);
            return NodeResult.success;
        }
    }
}