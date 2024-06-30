using UnityEngine;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Combat/Attack Target Service")]
    public class MeleeAttackEnemy : Leaf
    {
        public AnimatorReference animator;
        
        private static readonly int AttackHash = Animator.StringToHash("Attack");
        private bool _hasStartedAttacking;
        
        public override NodeResult Execute()
        {
            if (animator.Value == null)
            {
                Debug.Log("No animator set.");
                return NodeResult.failure;
            }
            
            AnimatorStateInfo stateInfo = animator.Value.GetCurrentAnimatorStateInfo(0);
            
            if (!_hasStartedAttacking)
            {
                animator.Value.SetTrigger(AttackHash);
                _hasStartedAttacking = true;
                return NodeResult.running;
            }
            
            // Check if the animation is playing
            if (stateInfo.IsName("Attack") && stateInfo.normalizedTime < 1.0f)
            {
                return NodeResult.running; 
            }

            // Check if the animation has finished
            if (stateInfo.normalizedTime >= 1f)
            {
                _hasStartedAttacking = false;
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