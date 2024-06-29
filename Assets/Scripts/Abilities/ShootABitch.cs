using System.Collections;
using System.Collections.Generic;
using FinalScripts.Refactored;
using UnityEngine;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Combat/Shoot Target Service")]
    public class ShootABitch : Leaf
    {
        public TransformReference sourceTransform;
        public TransformReference targetTransform;

        private EnemyBT _enemyBt;
        
        public override void OnEnter()
        {
            Transform t = sourceTransform.Value;

            if (t != null && t.TryGetComponent(out _enemyBt))
            {
                Debug.Log("Found enemy BT component!");
            }
            else
            {
                Debug.Log("Error: did not found enemy BT component.");
            }
        }
        
        public override NodeResult Execute()
        {
            if (_enemyBt != null) 
            {
                if (_enemyBt.Attack(targetTransform.Value))
                {
                    return NodeResult.success;
                }
            }
            
            // By default return failure
            return NodeResult.failure;
        }

        public override bool IsValid()
        {
            return !(targetTransform == null || sourceTransform == null);  
        }
    }
}