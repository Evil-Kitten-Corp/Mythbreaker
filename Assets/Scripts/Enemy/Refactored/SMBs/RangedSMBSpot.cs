using Enemy.Refactored.Utilities;
using UnityEngine;

namespace FinalScripts.Refactored.MoreSMBs
{
    public class RangedSMBSpot: SceneLinkedSMB<RangedEnemy>
    {
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            MonoBehaviour.Spotted();
        }
    } 
}