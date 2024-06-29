using DG.Tweening;
using UnityEngine;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Combat/Rotate Towards")]
    public class RotateTowardsTarget: Leaf
    {
        public TransformReference sourceTransform;
        public TransformReference targetTransform;
        public float speed = 1f;
        
        public override NodeResult Execute()
        {
            Transform target = targetTransform.Value;
            Transform obj = sourceTransform.Value;

            obj.DODynamicLookAt(target.position, speed);
            
            float dot = Vector3.Dot(obj.forward, (target.position - obj.position).normalized);
            
            return dot > 0.7f ? NodeResult.success : NodeResult.running;
        }
    }
}