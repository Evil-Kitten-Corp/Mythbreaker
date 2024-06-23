using Enemy.Refactored.Utilities;
using UnityEngine;

namespace FinalScripts.Refactored.MoreSMBs
{
    public class MeleeSMBReturn : SceneLinkedSMB<MeleeEnemy>
    {
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            MonoBehaviour.WalkBackToBase();
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);

            MonoBehaviour.FindTarget();

            if(MonoBehaviour.Target != null)
            {
                MonoBehaviour.StartPursuit();
            }
            else
            {
                MonoBehaviour.WalkBackToBase();
            }
        }
    }
}