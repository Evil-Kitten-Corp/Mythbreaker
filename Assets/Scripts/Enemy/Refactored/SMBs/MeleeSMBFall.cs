using Enemy.Refactored.Utilities;
using UnityEngine;

namespace FinalScripts.Refactored.MoreSMBs
{
    public class MeleeSMBFall : SceneLinkedSMB<MeleeEnemy>
    {
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (MonoBehaviour != null && MonoBehaviour.Controller != null)
            {
                MonoBehaviour.Controller.AddForce(Vector3.zero);
            }
        }

        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            MonoBehaviour.Controller.ClearForce();
        }
    }
}