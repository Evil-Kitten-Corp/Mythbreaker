using Enemy.Refactored.Utilities;
using UnityEngine;

namespace FinalScripts.Refactored.SMBs
{
    public class MeleeSMBHit: SceneLinkedSMB<MeleeEnemy>
    {
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.ResetTrigger("Attack");
        }

        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            MonoBehaviour.Controller.ClearForce();
        }
    }
}