using UnityEngine;

namespace FinalScripts.Refactored
{
    public class RagdollReplacerSMB : StateMachineBehaviour
    {
        public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        {
            RagdollReplacer replacer = animator.GetComponent<RagdollReplacer>();
            replacer.Replace();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            RagdollReplacer replacer = animator.GetComponent<RagdollReplacer>();
            replacer.Replace();
        }
    }
}