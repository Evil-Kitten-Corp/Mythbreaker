using Enemy.Refactored.Utilities;
using UnityEngine;

namespace FinalScripts.Refactored.MoreSMBs
{
    public class MeleeSMBSpot: SceneLinkedSMB<MeleeEnemy>
    {
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            MonoBehaviour.Spotted();
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            MonoBehaviour.FindTarget();

            if (MonoBehaviour.Target == null)
            {
                MonoBehaviour.StopPursuit();
                return;
            }

            Vector3 toTarget = MonoBehaviour.Target.transform.position - MonoBehaviour.transform.position;

            float onUp = Vector3.Dot(toTarget, MonoBehaviour.transform.up);
            toTarget -= MonoBehaviour.transform.up * onUp;

            toTarget.Normalize();

            MonoBehaviour.transform.forward =
                Vector3.Lerp(MonoBehaviour.transform.forward, toTarget, stateInfo.normalizedTime);
        }
    }
}