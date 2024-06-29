using UnityEngine;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Combat/Rotate Away")]
    public class RotateAwayFromTarget: Leaf
    {
        public TransformReference sourceTransform;
        public TransformReference targetTransform;
        public float speed = 1f;
        
        public override NodeResult Execute()
        {
            Transform target = targetTransform.Value;
            Transform obj = sourceTransform.Value;
 
            Vector3 awayDirection = obj.position - target.position;
            Quaternion awayRotation= Quaternion.LookRotation(awayDirection);
            
            obj.rotation = Quaternion.Slerp(obj.rotation, awayRotation, Time.deltaTime * speed);
            
            float dot = Vector3.Dot(obj.forward, (target.position - obj.position).normalized);
            
            return dot < -0.7f ? NodeResult.success : NodeResult.running;
        }
    }
}