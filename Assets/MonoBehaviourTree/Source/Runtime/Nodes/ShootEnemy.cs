using UnityEngine;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Combat/[NEW] Shoot Target Service")]
    public class ShootEnemy : Leaf
    {
        public AnimatorReference animator;
        
        private static readonly int ShootHash = Animator.StringToHash("Shoot");
        private bool _hasStartedShooting;
        
        public override NodeResult Execute()
        {
            if (animator.Value == null)
            {
                Debug.Log("No animator set.");
                return NodeResult.failure;
            }
            
            AnimatorStateInfo stateInfo = animator.Value.GetCurrentAnimatorStateInfo(0);
            
            if (!_hasStartedShooting)
            {
                animator.Value.SetTrigger(ShootHash);
                _hasStartedShooting = true;
                return NodeResult.running;
            }
            
            // Check if the animation is playing
            if (stateInfo.IsName("Shoot") && stateInfo.normalizedTime < 1.0f)
            {
                return NodeResult.running; 
            }

            // Check if the animation has finished
            if (stateInfo.IsName("Shoot") && stateInfo.normalizedTime >= 1.0f)
            {
                _hasStartedShooting = false;
                return NodeResult.success;
            }

            // default
            return NodeResult.failure;
        }

        public override bool IsValid()
        {
            return !(animator == null);  
        }
    }
}