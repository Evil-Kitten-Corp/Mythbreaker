using Enemy.Refactored.Utilities;
using UnityEngine;

namespace FinalScripts.Refactored.MoreSMBs
{
    public class RangedSMBCooldown: SceneLinkedSMB<RangedEnemy>
    {
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            MonoBehaviour.FindTarget();
            MonoBehaviour.CheckNeedFleeing();
        }
    }
}