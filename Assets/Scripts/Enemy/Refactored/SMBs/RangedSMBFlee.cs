using Enemy.Refactored.Utilities;
using UnityEngine;

namespace FinalScripts.Refactored.SMBs
{
    public class RangedSMBFlee: SceneLinkedSMB<RangedEnemy>
    {
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            MonoBehaviour.Controller.SetFollowNavmeshAgent(true);
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            MonoBehaviour.FindTarget();
            MonoBehaviour.CheckNeedFleeing();
        }
    }
}