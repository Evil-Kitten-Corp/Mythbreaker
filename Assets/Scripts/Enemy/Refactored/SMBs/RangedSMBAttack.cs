using Enemy.Refactored.Utilities;
using UnityEngine;

namespace FinalScripts.Refactored.SMBs
{
    public class RangedSMBAttack: SceneLinkedSMB<RangedEnemy>
    {
        private Vector3 _attackPos;

        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (MonoBehaviour.Target == null)
            {
                animator.Play("Idle");
                return;
            }

            MonoBehaviour.Controller.SetFollowNavmeshAgent(false);

            MonoBehaviour.RememberTargetPosition();
            Vector3 toTarget = MonoBehaviour.Target.transform.position - MonoBehaviour.transform.position;
            toTarget.y = 0;

            MonoBehaviour.transform.forward = toTarget.normalized;
            MonoBehaviour.Controller.SetForward(MonoBehaviour.transform.forward);
            Debug.Log("Robot is attacking.");

            if (MonoBehaviour.attackAudio != null)
            {
                MonoBehaviour.attackAudio.PlayRandomClip(); 
            }
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            MonoBehaviour.FindTarget();
        }
    }
}