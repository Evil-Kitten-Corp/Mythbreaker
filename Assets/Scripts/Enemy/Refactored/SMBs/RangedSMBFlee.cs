using Enemy.Refactored.Utilities;
using UnityEngine;

namespace FinalScripts.Refactored.SMBs
{
    public class RangedSMBFlee: SceneLinkedSMB<RangedEnemy>
    {
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            MonoBehaviour.Controller.SetFollowNavmeshAgent(true);
            Debug.Log("Robot navmesh ON.");
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log("Robot is fleeing.");
            MonoBehaviour.FindTarget();
            MonoBehaviour.CheckNeedFleeing();
        }
    }
}